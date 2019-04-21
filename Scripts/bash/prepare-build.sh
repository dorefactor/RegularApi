#!/bin/sh

echo "Run configuration or migration scripts"

docker pull rabbitmq:3.7

docker run -d --hostname rabbitmq-host --name rabbitmq rabbitmq:3.7
