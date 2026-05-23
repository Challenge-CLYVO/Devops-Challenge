#!/bin/bash

RESOURCE_GROUP="clyvo-rg"
LOCATION="brazilsouth"
VM_NAME="clyvo-vm"
ADMIN_USER="azureuser"

echo "Criando Resource Group..."
az group create --name $RESOURCE_GROUP --location $LOCATION

echo "Criando Máquina Virtual..."
az vm create \
  --resource-group $RESOURCE_GROUP \
  --name $VM_NAME \
  --image Ubuntu2204 \
  --admin-username $ADMIN_USER \
  --generate-ssh-keys \
  --public-ip-sku Standard

echo "Abrindo portas (80, 8080, 1521)..."
az vm open-port --resource-group $RESOURCE_GROUP --name $VM_NAME --port 80 --priority 1001
az vm open-port --resource-group $RESOURCE_GROUP --name $VM_NAME --port 8080 --priority 1002
az vm open-port --resource-group $RESOURCE_GROUP --name $VM_NAME --port 1521 --priority 1003

echo "Instalando Docker e ferramentas essenciais..."
az vm extension set \
  --resource-group $RESOURCE_GROUP \
  --vm-name $VM_NAME \
  --name customScript \
  --publisher Microsoft.Azure.Extensions \
  --settings '{"commandToExecute": "apt-get update && apt-get install -y git nano && curl -fsSL https://get.docker.com -o get-docker.sh && sh get-docker.sh && usermod -aG docker azureuser"}'

echo "Infraestrutura provisionada com sucesso em Paris!"