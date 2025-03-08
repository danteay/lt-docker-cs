variable "ecr_repo_url" {
  description = "ECR Repo URL"
  type        = string
}

variable "container_port" {
  description = "Container Port"
  type        = number
}

variable "service_name" {
  description = "ECS Service Name"
  type        = string
}

variable "task_memory" {
  description = "Task Memory"
  type        = number
  default     = 512
}

variable "task_cpu" {
  description = "Task CPU"
  type        = number
  default     = 256
}

variable "vpce_id" {
  description = "VPC Endpoint ID"
  type        = string
}

variable "subnet_ids" {
  description = "Subnet IDs"
  type        = list(string)
}

variable "cluster_id" {
  description = "ECS Cluster ID"
  type        = string
}

variable "desired_count" {
  description = "Desired Count"
  type        = number
  default     = 1
}

variable "image_tag" {
  description = "Docker Image Tag"
  type        = string
  default     = "latest"
}

variable "aws_region" {
  description = "AWS Region"
  type        = string
  default     = "us-east-1"
}