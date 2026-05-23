# 🐾 DevOps Challenge - CLYVO VET

## 📋 Sobre o Projeto
Este repositório contém a infraestrutura conteinerizada e os scripts de automatização para a API da **CLYVO VET**. O projeto consiste numa Web API desenvolvida em **.NET 9.0** integrada com um banco de dados **Oracle Express Edition (XE)**. 

O foco desta entrega é garantir que a aplicação funcione de forma isolada, segura e automatizada, tanto em ambiente de desenvolvimento local quanto em produção na nuvem da Microsoft Azure.

---

## 🎥 Vídeo de Demonstração
> []

No vídeo demonstramos:
1. Execução do Script Azure CLI provisionando a infraestrutura do zero.
2. Construção e subida dos containers com Docker Compose na Máquina Virtual.
3. Demonstração da API (Swagger) e do Banco de Dados rodando em nuvem.

---

## 🚀 Tecnologias e Ferramentas

* **Backend:** .NET 9.0 (ASP.NET Core)
* **Banco de Dados:** Oracle Express Edition (XE)
* **Orquestração de Containers:** Docker & Docker Compose
* **Cloud Provider:** Microsoft Azure
* **Infraestrutura como Código:** Bash Script (Azure CLI)
* **Controle de Versão:** Git & GitHub

---

## 🛠️ Boas Práticas Implementadas (Checklist DevOps)

Para garantir a segurança e resiliência da aplicação, as seguintes regras foram rigorosamente aplicadas:

- [x] **Privilégio Mínimo (Non-Root):** O Dockerfile possui a instrução USER app, garantindo que a aplicação rode sem privilégios de administrador dentro do container.
- [x] **Persistência de Dados:** O docker-compose.yml utiliza volumes nomeados (oracle_data) atrelados ao diretório /opt/oracle/oradata, evitando perda de dados do banco de dados na recriação dos containers.
- [x] **Rede Isolada:** Criação de rede customizada (clyvo_network) do tipo bridge, isolando a comunicação entre API e Banco de Dados.
- [x] **Automatização de Infra (IaC):** Uso de script .sh com comandos nativos do Azure CLI (az vm create, az vm open-port, az vm extension set) para evitar trabalho manual no portal.

---

## 📂 Estrutura de Arquivos DevOps

* Dockerfile: Receita de construção da imagem da API C#. Possui sistema de build em múltiplos estágios (multi-stage build) para deixar a imagem final mais leve.
* docker-compose.yml: Orquestrador dos serviços. Sobe o banco de dados Oracle e a API simultaneamente, passando as variáveis de ambiente corretas (Strings de conexão e ambiente de Development para expor o Swagger).
* azure-script.sh: Script responsável por criar o Resource Group, a Máquina Virtual, abrir as portas de firewall (80, 8080, 1521) e instalar o ecossistema Docker diretamente na nuvem.

---

## 💻 Como Executar Localmente

Pré-requisitos: Docker Desktop em execução.

1. Clone o repositório:
```bash
git clone [https://github.com/Challenge-CLYVO/Devops-Challenge.git](https://github.com/Challenge-CLYVO/Devops-Challenge.git)
cd Devops-Challenge
```

2. Suba os containers:
```bash
docker compose up -d --build
```

3. Acesse a documentação da API:
* URL: http://localhost:8080/swagger

---

## ☁️ Implantação na Nuvem (Microsoft Azure)

O ambiente de nuvem foi planejado para rodar no Canadá Central (canadacentral), utilizando uma VM Standard_B2as_v2 para otimização de custos e recursos.

Passo a Passo do Deploy:
1. Acesse o Azure Cloud Shell (Bash) através do Portal da Azure.
2. Baixe o código e execute o script de provisionamento:
```bash
git clone [https://github.com/Challenge-CLYVO/Devops-Challenge.git](https://github.com/Challenge-CLYVO/Devops-Challenge.git)
cd Devops-Challenge
chmod +x azure-script.sh
./azure-script.sh
```

3. Aguarde a mensagem "Infraestrutura provisionada com sucesso no Canadá!" e conecte-se à VM utilizando o IP público gerado:
```bash
ssh azureuser@<IP_PUBLICO_GERADO>
```

4. Dentro da VM, inicie a aplicação:
```bash
git clone [https://github.com/Challenge-CLYVO/Devops-Challenge.git](https://github.com/Challenge-CLYVO/Devops-Challenge.git)
cd Devops-Challenge
docker compose up -d --build
```

5. Teste o funcionamento no navegador: http://<IP_PUBLICO_GERADO>:8080/swagger

---

## 👥 Autores do Projeto (FIAP)
* Henrique Teixeira - RM: 563088
* Lucas Rafael Solimene - RM: 565194
* Samyr Couto Oliveira - RM: 565562
