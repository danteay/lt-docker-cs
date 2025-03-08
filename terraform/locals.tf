locals {
  aws_region = lookup(data.external.env.result, "AWS_REGION", "us-east-1")

  ecr_repo_name = "lt-docker-cs"

  availability_zones = [
    "us-east-1a",
    "us-east-1b",
    "us-east-1c",
  ]

  cluster_name = "main"

  service_name   = "lt-docker-cs"
  container_port = 5001
  image_tag      = data.external.env.result["IMAGE_TAG"]

}