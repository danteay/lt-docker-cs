variable env {
  description = "The environment to deploy the infrastructure to"
  type        = string
  // load from workspace 
  default     = terraform.workspace
}