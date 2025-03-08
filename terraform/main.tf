terraform {
  required_version = ">= 1.0.0" # Ensure that the Terraform version is 1.0.0 or higher

  required_providers {
    aws = {
      source  = "hashicorp/aws" # Specify the source of the AWS provider
      version = "~> 4.0"        # Use a version of the AWS provider that is compatible with version
    }

    external = {
      source  = "hashicorp/external"
      version = "~> 2.3"
    }
  }

  backend "s3" {
    bucket = "lt-docker-cs-terraform-state"
    key = "state"
    region = "us-east-1"
    dynamodb_table = "lt-docker-cs-terraform-locks"
  }
}

module "ecr" {
  source = "./modules/ecr"

  ecr_repo_name = local.ecr_repo_name
}

module "vpc" {
  source = "./modules/vpc"

  availability_zones = local.availability_zones
}

module "ecs" {
  source = "./modules/ecs"

  cluster_name = local.cluster_name
}

module "lt-docker-cs-service" {
  source = "./modules/service-fargate"

  ecr_repo_url   = module.ecr.repository_url
  container_port = local.container_port
  service_name   = local.service_name
  vpce_id        = module.vpc.default_vpc_id
  cluster_id     = module.ecs.cluster_id
  aws_region     = local.aws_region
  image_tag      = local.image_tag
  desired_count  = 3

  subnet_ids = [
    module.vpc.default_subnet_a_id,
    module.vpc.default_subnet_b_id,
    module.vpc.default_subnet_c_id
  ]
}
