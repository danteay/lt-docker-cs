output "default_vpc_id" {
  description = "Logical ID for default VPC"
  value       = aws_default_vpc.default.id
}

output "default_vpc_arn" {
  description = "AWS ARN for default VPC"
  value       = aws_default_vpc.default.arn
}

output "default_subnet_a_id" {
  description = "Logical ID for default subnet-a"
  value       = aws_default_subnet.default_subnet_a.id
}

output "default_subnet_a_arn" {
  description = "AWS ARN for default subnet-a"
  value       = aws_default_subnet.default_subnet_a.arn
}

output "default_subnet_b_id" {
  description = "Logical ID for default subnet-b"
  value       = aws_default_subnet.default_subnet_b.id
}

output "default_subnet_b_arn" {
  description = "AWS ARN for default subnet-b"
  value       = aws_default_subnet.default_subnet_b.arn
}

output "default_subnet_c_id" {
  description = "Logical ID for default subnet-c"
  value       = aws_default_subnet.default_subnet_c.id
}

output "default_subnet_c_arn" {
  description = "AWS ARN for default subnet-c"
  value       = aws_default_subnet.default_subnet_c.arn
}