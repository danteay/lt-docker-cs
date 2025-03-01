// Set outputs from ecr repository
output "LtDockerCsRepositoryUrl" {
  value = aws_ecr_repository.lt-docker-cs.repository_url
}

output "LtDockerCsRepositoryArn" {
  value = aws_ecr_repository.lt-docker-cs.arn
}