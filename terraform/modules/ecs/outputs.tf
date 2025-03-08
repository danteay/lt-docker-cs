output "cluster_name" {
  description = "Name of the created ECS Cluster"
  value       = aws_ecs_cluster.cluster.name
}

output "cluster_arn" {
  description = "ARN of the created ECS Cluster"
  value       = aws_ecs_cluster.cluster.arn
}

output "cluster_id" {
  description = "ID of the created ECS Cluster"
  value       = aws_ecs_cluster.cluster.id
}