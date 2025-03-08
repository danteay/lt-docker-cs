#!/bin/sh

# envs.sh

# Change the contents of this output to get the environment variables
# of interest. The output must be valid JSON, with strings for both
# keys and values.
cat <<EOF
{
  "AWS_REGION": "$AWS_REGION",
  "IMAGE_TAG": "$IMAGE_TAG"
}
EOF