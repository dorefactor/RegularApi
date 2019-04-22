#!/bin/sh

echo "Run configuration or migration scripts"

docker pull rabbitmq:3.7
docker run -d -p 0.0.0.0:5672:5672 --hostname rabbitmq-host --name rabbitmq rabbitmq:3.7
