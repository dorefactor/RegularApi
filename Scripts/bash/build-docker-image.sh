#!/bin/sh

echo "Generating Docker Image: dorefactor/rd-api:$TRAVIS_TAG"

docker build -t dorefactor/rd-api:$TRAVIS_TAG -f Docker/Dockerfile .

echo "Image generated"

docker images