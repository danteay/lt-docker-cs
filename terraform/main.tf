terraform {
  required_version = ">= 1.0.0" # Ensure that the Terraform version is 1.0.0 or higher

  required_providers {
    aws = {
      source  = "hashicorp/aws" # Specify the source of the AWS provider
      version = "~> 4.0"        # Use a version of the AWS provider that is compatible with version
    }
  }
}

provider "aws" {
  region = lookup(data.external.env.result, "AWS_REGION", "us-east-1") # Use the AWS_REGION environment variable to set the region
}

resource "aws_ecr_repository" "lt-docker-cs" {
  name                 = "lt-docker-cs" # Set the name of the ECR repository to lt-docker-cs
  image_tag_mutability = "MUTABLE"      # Allow the image tag to be mutable

  # image_scanning_configuration {
  #   scan_on_push = true
  # }
}