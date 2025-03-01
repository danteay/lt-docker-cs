# Define the workspace name and directory path
DEFAULT_WORKSPACE = prod

# Define default goal
.DEFAULT_GOAL := help

.PHONY: help format validate init workspace apply lint

help: ## Show this help message
	@echo "Usage: make [target] tf_dir=<path> [workspace=dev]\n"
	@grep -E '^[a-zA-Z_-]+:.*?## .*$$' $(MAKEFILE_LIST) | awk 'BEGIN {FS = ":.*?## "}; {printf "\033[36m%-20s\033[0m %s\n", $$1, $$2}'

format: ## Format Terraform code
	@echo "Formatting Terraform code..."
	terraform fmt -recursive -write=true

lint: ## Lint Terraform code
	@echo "Linting Terraform code..."
	tflint --recursive --fix

validate: ## Validate Terraform configuration
	@echo "Validating Terraform configuration..."
	terraform -chdir=terraform validate

init: ## Initialize Terraform
	@echo "Initializing Terraform..."
	terraform -chdir=terraform init

workspace: ## Create and select Terraform workspace
	@echo "Creating and selecting workspace $(DEFAULT_WORKSPACE)..."
	terraform -chdir=terraform workspace select $(DEFAULT_WORKSPACE) || terraform -chdir=terraform workspace new $(DEFAULT_WORKSPACE)

plan: init workspace ## Generate the Terraform plan
	@echo "Generating Terraform plan..."
	terraform -chdir=terraform plan

apply: init workspace ## Apply the Terraform plan
	@echo "Applying Terraform configuration..."
	terraform -chdir=terraform apply -auto-approve

destroy: init workspace ## Destroy the Terraform plan
	@echo "Destroying Terraform configuration..."
	terraform -chdir=terraform destroy