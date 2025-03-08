#!/usr/bin/env bash

LOGSTASH_SEC_ID=$1
POSTGRES_SEC_ID=$2
PHASE_API_KEY=$3
AWS_ACCOUNT_ID=$4
AWS_REGION=$5
VERSION=$6

docker build \
    --build-arg stage=prod \
    --build-arg port=5001 \
    --build-arg logstashConfigs="$LOGSTASH_SEC_ID" \
    --build-arg phaseApiKey="$PHASE_API_KEY" \
    --build-arg postgresSecretName="$POSTGRES_SEC_ID" \
    -t lt-docker-cs app
  
docker tag lt-docker-cs:latest "$AWS_ACCOUNT_ID.dkr.ecr.$AWS_REGION.amazonaws.com/lt-docker-cs:latest"
docker tag lt-docker-cs:latest "$AWS_ACCOUNT_ID.dkr.ecr.$AWS_REGION.amazonaws.com/lt-docker-cs:$VERSION"

docker push "$AWS_ACCOUNT_ID.dkr.ecr.$AWS_REGION.amazonaws.com/lt-docker-cs:latest"
docker push "$AWS_ACCOUNT_ID.dkr.ecr.$AWS_REGION.amazonaws.com/lt-docker-cs:$VERSION"