#!/bin/sh

docker stop rabbitmq

docker rm -f rabbitmq

dorker rmi -f rabbitmq:3.7
