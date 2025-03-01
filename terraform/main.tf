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
  region = "us-east-1" # Set the AWS region to US East (N. Virginia)
}

resource "aws_ecr_repository" "lt-docker-cs" {
  name                 = "lt-docker-cs" # Set the name of the ECR repository to lt-docker-cs
  image_tag_mutability = "MUTABLE"      # Allow the image tag to be mutable

  # image_scanning_configuration {
  #   scan_on_push = true
  # }
}