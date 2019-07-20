#!/bin/sh

echo "$REGISTRY_PWD" | docker login -u "$REGISTRY_USER" --password-stdin

docker push dorefactor/rd-api:$TRAVIS_TAG

echo "Completed uploading image"