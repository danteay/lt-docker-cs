# Run the script to get the environment variables of interest.
# This is a data source, so it will run at plan time.
data "external" "env" {
  program = ["${path.module}/envs.sh"]

  # For Windows (or Powershell core on MacOS and Linux),
  # run a Powershell script instead
  #program = ["${path.module}/envs.ps1"]
}
