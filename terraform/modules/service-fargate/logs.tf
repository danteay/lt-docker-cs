resource "aws_cloudwatch_log_group" "log_group" {
  name              = "/ecs/${var.service_name}"
  retention_in_days = 7
}

resource "aws_cloudwatch_log_stream" "log_stream" {
  name           = "${var.service_name}-log-stream"
  log_group_name = aws_cloudwatch_log_group.log_group.name
}