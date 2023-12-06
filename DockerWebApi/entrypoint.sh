#!/bin/sh
set -e
service ssh start
dotnet DockerWebApi.dll
