#!/bin/bash

docker container kill horizon-middleware
set -e

bash run.sh
