output "alb_dns_name" {
  description = "ALB DNS Name"
  value       = aws_alb.application_load_balancer.dns_name
}

output "alb_arn" {
  description = "ALB ARN"
  value       = aws_alb.application_load_balancer.arn
}

output "task_definition_arn" {
  description = "Task Definition ARN"
  value       = aws_ecs_task_definition.task_definition.arn
}

output "service_id" {
  description = "Service ID"
  value       = aws_ecs_service.service.id
}