# envs.ps1

# Change the contents of this output to get the environment variables
# of interest. The output must be valid JSON, with strings for both
# keys and values.
ConvertTo-Json @{
#    foo = $Env:foo
    AWS_REGION = $Env:AWS_REGION
    IMAGE_TAG = $Env:IMAGE_TAG
}