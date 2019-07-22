#!/bin/sh

# echo "$REGISTRY_PWD" | docker login -u "$REGISTRY_USER" --password-stdin

docker login -u "$REGISTRY_USER" -p "$REGISTRY_PWD"

docker push dorefactor/rd-api:$TRAVIS_TAG

echo "Completed uploading image"