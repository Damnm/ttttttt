#!/bin/bash

#thanhpmelb
image="thanhpmelb/epay.etc.core.api"

#get timestamp for the tag  
timestamp=$(date +%Y%m%d%H%M%S)

tag=$image:$timestamp  
latest=$image:latest  
  
#build image  
docker build --no-cache -t $tag -f Dockerfile . 
  
docker tag $tag $latest

#push to dockerhub  
docker login  
#sudo docker login -u username -p password  
docker push $image
  
#remove dangling images  
docker system prune -f  
