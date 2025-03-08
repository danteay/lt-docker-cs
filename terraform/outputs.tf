output "LtDockerCsRepositoryUrl" {
  value = module.ecr.repository_url
}

output "ECSClusterName" {
  value = module.ecs.cluster_name
}

output "LtDockerCsAlbDsnName" {
  value = module.lt-docker-cs-service.alb_dns_name
}