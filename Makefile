# Define the workspace name and directory path
DEFAULT_WORKSPACE = prod
SHELL := /bin/bash

# Define default goal
.DEFAULT_GOAL := help

.PHONY: help format validate init workspace apply lint

help: ## Show this help message
	@echo "Usage: make [target] tf_dir=<path> [workspace=dev]\n"
	@grep -E '^[a-zA-Z_-]+:.*?## .*$$' $(MAKEFILE_LIST) | awk 'BEGIN {FS = ":.*?## "}; {printf "\033[36m%-20s\033[0m %s\n", $$1, $$2}'

tf_format: ## Format Terraform code
	@echo "Formatting Terraform code..."
	terraform fmt -recursive -write=true

tf_lint: ## Lint Terraform code
	@echo "Linting Terraform code..."
	tflint --recursive --fix

tf_validate: ## Validate Terraform configuration
	@echo "Validating Terraform configuration..."
	terraform -chdir=terraform validate

tf_init: ## Initialize Terraform
	@echo "Initializing Terraform..."
	terraform -chdir=terraform init

tf_workspace: ## Create and select Terraform workspace
	@echo "Creating and selecting workspace $(DEFAULT_WORKSPACE)..."
	terraform -chdir=terraform workspace select $(DEFAULT_WORKSPACE) || terraform -chdir=terraform workspace new $(DEFAULT_WORKSPACE)

tf_plan: tf_init tf_workspace ## Generate the Terraform plan
	@echo "Generating Terraform plan..."
	terraform -chdir=terraform plan

tf_apply: tf_format tf_lint tf_init tf_workspace ## Apply the Terraform plan
	@echo "Applying Terraform configuration..."
	terraform -chdir=terraform apply -auto-approve

tf_destroy: tf_init tf_workspace ## Destroy the Terraform plan
	@echo "Destroying Terraform configuration..."
	terraform -chdir=terraform destroy -auto-approve

net_format: ## format .Net Core code
	@echo "Formatting .Net Core code..."
	dotnet dotnet-csharpier .

run_elk: ## Run ELK stack
	@echo "Running ELK stack..."
	docker-compose up elasticsearch kibana --build -d

run_dev: run_elk ## Run development environment
	@echo "Running development environment..."

	env STAGE=dev pkl eval -f yaml app/config/pkl/main.pkl > app/config/dev.yml
	docker-compose up postgres -d
	docker-compose up app --build