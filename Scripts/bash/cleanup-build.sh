#!/bin/sh

echo "Build cleanup"

docker stop rabbitmq
docker rm -f rabbitmq
docker rmi -f rabbitmq:3.7
