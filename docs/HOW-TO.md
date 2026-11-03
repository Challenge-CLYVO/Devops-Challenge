# HOW-TO — DevOps Sprint 4
## PetCare API .NET 9 com Oracle, GitHub, Azure DevOps, Docker, ACR e Azure Container Instances

---

## 1. Objetivo

Este guia apresenta o processo completo de configuração, publicação e validação da API PetCare utilizando:

- **.NET 9** — desenvolvimento da API.
- **Oracle FIAP** — banco de dados.
- **GitHub** — versionamento do código-fonte.
- **Azure DevOps** — automação CI/CD.
- **Azure Pipelines** — execução dos processos de integração e entrega.
- **Docker** — criação da imagem da aplicação.
- **Azure Container Registry (ACR)** — armazenamento da imagem Docker.
- **Azure Container Instances (ACI)** — execução da API na nuvem.
- **Agente self-hosted Windows** — execução dos pipelines na máquina configurada.

### Arquitetura da solução

```text
Desenvolvedor
     |
     v
GitHub
     |
     v
Azure DevOps — CI
     |
     +--> Restore
     +--> Build
     +--> Testes
     +--> Publicação do artefato
     +--> Build da imagem Docker
     +--> Push para o ACR
                 |
                 v
        Azure Container Registry
                 |
                 v
Azure DevOps — CD
     |
     +--> Download do artefato
     +--> Identificação da imagem
     +--> Configuração dos segredos
     +--> Criação/atualização do ACI
                 |
                 v
        Azure Container Instances
                 |
                 v
             PetCare API
                 |
                 v
             Oracle FIAP
```

---

## 2. Informações do ambiente utilizado

Os valores abaixo correspondem aos recursos utilizados no projeto.

| Item | Configuração |
|---|---|
| Projeto | PetCare |
| Framework | .NET 9 |
| Repositório local | `C:\Users\USER\Documents\2TDSPF\Devops\Devops-Challenge` |
| Solução | `API\Challenge.sln` |
| Projeto de inicialização | `API\PetCare.API\PetCare.API.csproj` |
| Porta local | `5100` |
| Porta no container | `8080` |
| Ambiente | Production |
| Assinatura Azure | `2026-2TDSPF-LUCAS-RAFAEL-SOLIMENE-RM565194` |
| Resource Group | `rg-petcare-sprint4` |
| Região | `southafricanorth` |
| ACR | `petcareacr12345` |
| Login Server | `petcareacr12345.azurecr.io` |
| ACI | `petcare-api-aci` |
| DNS Label | `petcare-api-lucas-sprint4` |
| FQDN | `petcare-api-lucas-sprint4.southafricanorth.azurecontainer.io` |
| CPU do ACI | 1 core |
| Memória do ACI | 1.5 GB |
| Sistema operacional | Linux |

### Identificadores Azure

**Subscription ID**

```text
4a768455-d8f3-4183-baf3-a527004fca82
```

**Tenant ID**

```text
11dbbfe2-89b8-4549-be10-cec364e59551
```

**Resource Group ID**

```text
/subscriptions/4a768455-d8f3-4183-baf3-a527004fca82/resourceGroups/rg-petcare-sprint4
```

---

## 3. Estrutura do repositório

O repositório contém a solução .NET e os arquivos de automação.

Estrutura principal:

```text
Devops-Challenge/
│
├── API/
│   ├── Challenge.sln
│   │
│   ├── PetCare.API/
│   ├── PetCare.Application/
│   ├── PetCare.Domain/
│   ├── PetCare.Infrastructure/
│   │
│   ├── Tests.Integration/
│   ├── Tests.Unit/
│   │
│   └── Dockerfile
│
└── azure-pipelines/
    ├── CI
    └── CD
```

> Os nomes e caminhos dos arquivos de pipeline devem ser conferidos no repositório. Os YAMLs de CI e CD já fazem parte do projeto e não precisam ser recriados seguindo este documento.

---

## 4. Preparação do ambiente local

### 4.1. Instalar as ferramentas

Antes de executar o projeto, é necessário ter:

- .NET SDK 9.
- Git.
- Docker Desktop.
- Azure CLI.
- Acesso ao GitHub.
- Acesso ao Azure DevOps.
- Acesso à assinatura Azure.

Verificar a instalação do .NET:

```powershell
dotnet --version
```

Verificar o Git:

```powershell
git --version
```

Verificar o Docker:

```powershell
docker --version
```

Verificar se o Docker está funcionando:

```powershell
docker info
```

Verificar o Azure CLI:

```powershell
az version
```

Caso o Docker Desktop não esteja iniciado, abra-o antes de executar os comandos Docker.

### 4.2. Acessar o repositório

```powershell
Set-Location "C:\Users\USER\Documents\2TDSPF\Devops\Devops-Challenge"
```

Conferir a pasta:

```powershell
Get-Location
```

Conferir os arquivos:

```powershell
Get-ChildItem
```

### 4.3. Conferir o Git

```powershell
git status
```

```powershell
git branch
```

```powershell
git remote -v
```

O repositório remoto deve apontar para o GitHub utilizado no projeto.

### 4.4. Restaurar, compilar e testar a solução

```powershell
dotnet restore ".\API\Challenge.sln"
```

```powershell
dotnet build ".\API\Challenge.sln" --configuration Release --no-restore
```

Executar os testes:

```powershell
dotnet test ".\API\Challenge.sln" --configuration Release --no-build
```

A execução deve terminar sem falhas nos testes.

### 4.5. Executar a API localmente

```powershell
dotnet run --project ".\API\PetCare.API\PetCare.API.csproj" --urls "http://localhost:5100"
```

Acessar o Swagger:

```text
http://localhost:5100/swagger
```

Para testar a documentação diretamente pelo PowerShell:

```powershell
Invoke-WebRequest `
  -Uri "http://localhost:5100/swagger/index.html" `
  -UseBasicParsing
```

---

## 5. Configuração do GitHub

O código-fonte deve estar em um repositório GitHub acessível pelo Azure DevOps.

Conferir o repositório remoto:

```powershell
git remote -v
```

Conferir a branch:

```powershell
git branch --show-current
```

A branch utilizada como referência para os pipelines deste projeto é `master`.

Para enviar as alterações:

```powershell
git add .
```

```powershell
git commit -m "chore: prepare PetCare DevOps deployment"
```

```powershell
git push origin master
```

> Caso a alteração já esteja commitada, não é necessário criar outro commit. O importante é que o código e os arquivos de pipeline estejam publicados na branch configurada como gatilho.

---

## 6. Configuração do Azure DevOps

### 6.1. Acessar a organização

Organização:

```text
https://dev.azure.com/DevOps-2TDSPF-RM565194/
```

Projeto:

```text
Sprint 4 – Azure DevOps
```

### 6.2. Configurar o repositório

O projeto Azure DevOps utiliza o repositório GitHub como origem do código.

No Azure DevOps:

1. Acesse o projeto.
2. Entre em **Project Settings**.
3. Confira a conexão com o GitHub.
4. Acesse **Pipelines**.
5. Confira os pipelines CI e CD.
6. Verifique se os arquivos YAML apontam para os caminhos corretos do repositório.

### 6.3. Configurar o agente self-hosted

Foi utilizado um agente Windows local.

Nome do agente:

```text
AgentLocal
```

Pool:

```text
Default
```

Diretório:

```text
C:\agent
```

O agente precisa estar registrado na organização e no pool utilizado pelos pipelines.

Para iniciar manualmente o agente:

```powershell
Set-Location "C:\agent"
```

```powershell
.\run.cmd
```

A mensagem abaixo confirma que ele está aguardando trabalhos:

```text
Listening for Jobs
```

**Importante:** mantenha a janela do agente aberta durante a execução dos pipelines, caso ele esteja sendo iniciado manualmente.

---

## 7. Autenticação no Azure

Abra o PowerShell e execute:

```powershell
az login
```

Caso existam várias assinaturas, selecione a assinatura do projeto:

```powershell
az account set --subscription "4a768455-d8f3-4183-baf3-a527004fca82"
```

Conferir a assinatura ativa:

```powershell
az account show --output table
```

Conferir o usuário autenticado:

```powershell
az account show --query user.name --output tsv
```

---

## 8. Criar e conferir o Resource Group

Nome:

```text
rg-petcare-sprint4
```

Região:

```text
southafricanorth
```

Criar ou garantir a existência do Resource Group:

```powershell
az group create `
  --name rg-petcare-sprint4 `
  --location southafricanorth `
  --output table
```

Conferir:

```powershell
az group show `
  --name rg-petcare-sprint4 `
  --output table
```

---

## 9. Criar e configurar o Azure Container Registry

O Azure Container Registry armazena as imagens Docker utilizadas na publicação da aplicação.

### 9.1. Criar o ACR

Nome:

```text
petcareacr12345
```

SKU:

```text
Standard
```

Criar ou atualizar o registro:

```powershell
az acr create `
  --resource-group rg-petcare-sprint4 `
  --name petcareacr12345 `
  --sku Standard `
  --admin-enabled true `
  --location southafricanorth `
  --output table
```

### 9.2. Conferir o ACR

```powershell
az acr show `
  --name petcareacr12345 `
  --resource-group rg-petcare-sprint4 `
  --output table
```

Conferir o servidor de login:

```powershell
az acr show `
  --name petcareacr12345 `
  --query loginServer `
  --output tsv
```

Resultado esperado:

```text
petcareacr12345.azurecr.io
```

### 9.3. Conferir se o acesso administrativo está habilitado

```powershell
az acr show `
  --name petcareacr12345 `
  --query adminUserEnabled `
  --output tsv
```

Resultado esperado:

```text
true
```

O acesso administrativo foi habilitado para permitir a autenticação de pull da imagem pelo ACI, conforme a configuração utilizada neste projeto.

**Segurança:** não publique senhas do ACR em arquivos, commits, prints ou documentação. Não compartilhe a saída de comandos que exibam credenciais.

---

## 10. Configurar as permissões IAM

A identidade utilizada pela Service Connection do Azure Resource Manager precisa ter permissão para consultar e administrar os recursos do Resource Group.

### 10.1. Identidade utilizada

Object ID do Service Principal:

```text
6edc9b06-1781-4190-81a7-6dfc134cd6f0
```

### 10.2. Conferir as permissões existentes

```powershell
az role assignment list `
  --assignee "6edc9b06-1781-4190-81a7-6dfc134cd6f0" `
  --scope "/subscriptions/4a768455-d8f3-4183-baf3-a527004fca82/resourceGroups/rg-petcare-sprint4" `
  --output table
```

### 10.3. Conceder Contributor

Se a permissão ainda não existir, execute:

```powershell
az role assignment create `
  --assignee-object-id 6edc9b06-1781-4190-81a7-6dfc134cd6f0 `
  --assignee-principal-type ServicePrincipal `
  --role Contributor `
  --scope "/subscriptions/4a768455-d8f3-4183-baf3-a527004fca82/resourceGroups/rg-petcare-sprint4"
```

Conferir novamente:

```powershell
az role assignment list `
  --assignee "6edc9b06-1781-4190-81a7-6dfc134cd6f0" `
  --scope "/subscriptions/4a768455-d8f3-4183-baf3-a527004fca82/resourceGroups/rg-petcare-sprint4" `
  --output table
```

O papel esperado é:

```text
Contributor
```

**Observação:** a permissão Contributor permite administrar os recursos dentro do escopo concedido, mas não concede automaticamente permissão para administrar atribuições IAM.

---

## 11. Configurar as Service Connections

No Azure DevOps, acesse:

```text
Project Settings > Service connections
```

Foram utilizadas duas conexões.

### 11.1. sc-azure-petcare

Tipo:

```text
Azure Resource Manager
```

Finalidade:

- Autenticar o pipeline no Azure.
- Consultar o Resource Group.
- Consultar o ACR.
- Criar, consultar e excluir o ACI.
- Executar as etapas de publicação.

A conexão foi configurada com Workload Identity Federation.

### 11.2. sc-petcare-acr

Tipo:

```text
Azure Container Registry
```

Registro:

```text
petcareacr12345
```

Finalidade:

- Autenticar o pipeline no ACR.
- Construir e enviar a imagem Docker.
- Disponibilizar a imagem para o deploy.

**Importante:** os nomes das conexões precisam corresponder exatamente aos nomes referenciados nos pipelines.

---

## 12. Configurar as variáveis do Azure DevOps

### 12.1. Criar o Variable Group

No Azure DevOps:

1. Acesse **Pipelines**.
2. Entre em **Library**.
3. Clique em **Variable groups**.
4. Abra ou crie o grupo `PetCare-Production`.
5. Adicione as variáveis abaixo.

### 12.2. Variáveis de configuração

| Nome | Valor |
|---|---|
| `AZURE_RESOURCE_GROUP` | `rg-petcare-sprint4` |
| `ACR_NAME` | `petcareacr12345` |
| `ACI_NAME` | `petcare-api-aci` |
| `ACI_DNS_LABEL` | `petcare-api-lucas-sprint4` |
| `ORACLE_CONNECTION_STRING` | Connection string válida do Oracle |
| `JWT_KEY` | Chave secreta para assinatura JWT |

### 12.3. Configurar as variáveis secretas

As variáveis abaixo precisam estar marcadas como secretas:

```text
ORACLE_CONNECTION_STRING
JWT_KEY
```

A connection string deve apontar para o banco Oracle utilizado pela aplicação.

A chave JWT deve ser forte, aleatória e mantida em segredo.

Para gerar uma chave aleatória pelo PowerShell:

```powershell
$bytes = New-Object byte[] 64

[System.Security.Cryptography.RandomNumberGenerator]::Create().GetBytes($bytes)

$jwtKey = [Convert]::ToBase64String($bytes)

$jwtKey
```

Copie o resultado diretamente para o campo `JWT_KEY` do Variable Group.

Não coloque a chave no código-fonte, no GitHub ou em arquivos YAML.

### 12.4. Conferência antes de executar o CD

Antes de iniciar a publicação, confirme que:

- `ORACLE_CONNECTION_STRING` está preenchida.
- `JWT_KEY` está preenchida.
- As duas estão marcadas como secretas.
- O Variable Group está autorizado para os pipelines que precisam utilizá-lo.

Não é necessário exibir o conteúdo dos segredos para verificar se foram configurados.

**Atenção:** se essas variáveis estiverem vazias, o ACI poderá iniciar sem as configurações necessárias para autenticação e conexão com o banco.

---

## 13. Configuração do Docker

### 13.1. Dockerfile

O arquivo utilizado está localizado em:

```text
API\Dockerfile
```

A imagem utiliza:

- .NET SDK 9.0 para compilação.
- .NET Runtime 9.0 para execução.
- Porta 8080.
- Ambiente Production.
- Usuário não privilegiado `app`.

A aplicação é iniciada no container utilizando:

```text
http://+:8080
```

### 13.2. Conferir o Dockerfile

```powershell
Get-Content ".\API\Dockerfile"
```

### 13.3. Conferir o Docker local

```powershell
docker info
```

O Docker Desktop precisa estar em execução para realizar builds locais.

---

## 14. Pipeline CI — Integração Contínua

O pipeline CI automatiza a validação do código e a publicação da imagem Docker.

O gatilho configurado utiliza a branch:

```text
master
```

### 14.1. Etapas executadas pelo CI

1. Detectar alterações na branch configurada.
2. Restaurar os pacotes NuGet.
3. Compilar a solução .NET.
4. Executar os testes.
5. Publicar os arquivos da aplicação.
6. Gerar o artefato `drop`.
7. Construir a imagem Docker.
8. Enviar a imagem ao Azure Container Registry.

### 14.2. Identificação da imagem

O pipeline utiliza o Build ID como tag da imagem.

Exemplo:

```text
petcareacr12345.azurecr.io/petcare-api:16
```

O número `16` representa uma execução específica do pipeline. Em novas execuções, a tag poderá mudar.

### 14.3. Executar o CI

Para iniciar uma execução por meio de uma alteração no repositório:

```powershell
Set-Location "C:\Users\USER\Documents\2TDSPF\Devops\Devops-Challenge"
```

```powershell
git status
```

Após realizar e commitar uma alteração:

```powershell
git add .
```

```powershell
git commit -m "chore: trigger PetCare CI pipeline"
```

```powershell
git push origin master
```

Também é possível executar manualmente pelo Azure DevOps:

```text
Pipelines > CI > Run pipeline
```

### 14.4. Conferir o resultado

No Azure DevOps:

1. Abra o pipeline CI.
2. Acesse a execução mais recente.
3. Confira os logs das etapas.
4. Confirme que Restore, Build e Test foram concluídos.
5. Confirme a publicação do artefato `drop`.
6. Confirme que a imagem foi enviada ao ACR.

### 14.5. Conferir a imagem no ACR

```powershell
az acr repository list `
  --name petcareacr12345 `
  --output table
```

Conferir as tags:

```powershell
az acr repository show-tags `
  --name petcareacr12345 `
  --repository petcare-api `
  --output table
```

A imagem utilizada pelo CD precisa existir no ACR.

---

## 15. Pipeline CD — Entrega Contínua

O pipeline CD automatiza a publicação da API no Azure Container Instances.

Ele utiliza a execução do CI como origem para identificar a imagem correspondente.

### 15.1. Gatilho

O CD está configurado para ser acionado pela conclusão do pipeline CI `PetCare-CI` na branch `master`.

Também pode ser iniciado manualmente pelo Azure DevOps:

```text
Pipelines > CD > Run pipeline
```

### 15.2. Etapas executadas pelo CD

1. Autenticar no Azure utilizando `sc-azure-petcare`.
2. Validar as variáveis necessárias.
3. Consultar o Resource Group.
4. Consultar o Azure Container Registry.
5. Identificar a imagem gerada pelo CI.
6. Obter as informações necessárias para o acesso ao ACR.
7. Remover a instância anterior, quando necessário.
8. Criar o novo ACI.
9. Configurar as variáveis de ambiente.
10. Aguardar e verificar o estado do provisionamento.
11. Validar a inicialização do container.

### 15.3. Tratamento do estado do ACI

A versão utilizada do Azure CLI não reconheceu o comando `az container wait`.

Por isso, a validação do CD utiliza consultas ao recurso com:

```powershell
az container show
```

A verificação deve considerar o estado do provisionamento e o estado atual do container.

Os estados esperados são:

```text
provisioningState: Succeeded
```

```text
currentState.state: Running
```

### 15.4. Resultado esperado do CD

Ao concluir corretamente:

- A imagem deve estar disponível no ACR.
- O ACI deve ser criado.
- O container deve estar em execução.
- As variáveis secretas devem ser fornecidas ao container.
- A API deve estar acessível pelo endereço público.

---

## 16. Validar o Azure Container Instances

### 16.1. Consultar o ACI

```powershell
az container show `
  --resource-group rg-petcare-sprint4 `
  --name petcare-api-aci `
  --output table
```

### 16.2. Consultar o estado do provisionamento

```powershell
az container show `
  --resource-group rg-petcare-sprint4 `
  --name petcare-api-aci `
  --query provisioningState `
  --output tsv
```

Resultado esperado:

```text
Succeeded
```

### 16.3. Consultar o estado atual do container

```powershell
az container show `
  --resource-group rg-petcare-sprint4 `
  --name petcare-api-aci `
  --query "containers[0].instanceView.currentState.state" `
  --output tsv
```

Resultado esperado:

```text
Running
```

### 16.4. Consultar IP e FQDN

```powershell
az container show `
  --resource-group rg-petcare-sprint4 `
  --name petcare-api-aci `
  --query "{IP:ipAddress.ip,FQDN:ipAddress.fqdn}" `
  --output table
```

O IP público pode mudar quando o ACI é recriado. Por isso, sempre consulte o endereço atual em vez de reutilizar um IP antigo.

### 16.5. Consultar os logs

```powershell
az container logs `
  --resource-group rg-petcare-sprint4 `
  --name petcare-api-aci
```

A inicialização correta apresenta mensagens semelhantes a:

```text
Now listening on: http://[::]:8080
Application started. Press Ctrl+C to shut down.
Hosting environment: Production
```

### 16.6. Consultar eventos do container

```powershell
az container show `
  --resource-group rg-petcare-sprint4 `
  --name petcare-api-aci `
  --query "containers[0].instanceView.events" `
  --output table
```

Os eventos devem indicar que a imagem foi baixada e que o container foi iniciado.

### 16.7. Listar os containers

```powershell
az container list `
  --resource-group rg-petcare-sprint4 `
  --output table
```

**Observação:** o comando `az container list` pode apresentar o status do grupo como `Succeeded`, enquanto a consulta `currentState.state` apresenta `Running`. São informações de naturezas diferentes: o provisionamento foi concluído e o container permanece em execução.

---

## 17. Validar a API publicamente

### 17.1. Obter o FQDN automaticamente

```powershell
$fqdn = az container show `
  --resource-group rg-petcare-sprint4 `
  --name petcare-api-aci `
  --query ipAddress.fqdn `
  --output tsv
```

Conferir:

```powershell
$fqdn
```

Resultado esperado:

```text
petcare-api-lucas-sprint4.southafricanorth.azurecontainer.io
```

### 17.2. Abrir o Swagger

Endereço:

```text
http://petcare-api-lucas-sprint4.southafricanorth.azurecontainer.io:8080/swagger
```

Para testar pelo PowerShell:

```powershell
Invoke-WebRequest `
  -Uri "http://${fqdn}:8080/swagger/index.html" `
  -UseBasicParsing
```

A resposta HTTP deve indicar sucesso.

### 17.3. Conferir a resposta HTTP

```powershell
$response = Invoke-WebRequest `
  -Uri "http://${fqdn}:8080/swagger/index.html" `
  -UseBasicParsing

$response.StatusCode
```

Resultado esperado:

```text
200
```

### 17.4. Conferir os logs novamente

```powershell
az container logs `
  --resource-group rg-petcare-sprint4 `
  --name petcare-api-aci
```

Verifique se não existem erros de inicialização ou falhas recorrentes.

---

## 18. Validar a conexão com o Oracle

A aplicação utiliza a variável:

```text
ConnectionStrings__RecommendaContextOracle
```

Essa variável recebe o conteúdo de:

```text
ORACLE_CONNECTION_STRING
```

O valor é configurado no Variable Group `PetCare-Production` e fornecido ao container pelo CD.

### 18.1. Conferir a configuração

No Azure DevOps:

```text
Pipelines > Library > PetCare-Production
```

Verifique se:

- A variável `ORACLE_CONNECTION_STRING` está preenchida.
- Está marcada como secreta.
- O pipeline CD tem acesso ao Variable Group.

Não imprima nem compartilhe a connection string.

### 18.2. Consultar os logs da aplicação

```powershell
az container logs `
  --resource-group rg-petcare-sprint4 `
  --name petcare-api-aci
```

Verifique se existem erros de conexão, autenticação ou mapeamento do Oracle.

### 18.3. Testar o endpoint de saúde

A aplicação possui o endpoint:

```text
/api/oracle/external-service
```

Execute:

```powershell
Invoke-WebRequest `
  -Uri "http://${fqdn}:8080/api/oracle/external-service" `
  -UseBasicParsing
```

O resultado deve ser avaliado conforme a resposta definida pela aplicação. O teste precisa confirmar que o serviço Oracle está acessível, e não apenas que a API está respondendo HTTP.

---

## 19. Validar autenticação JWT

A configuração utiliza:

| Configuração | Valor |
|---|---|
| Issuer | `PetCare.API` |
| Audience | `PetCare.Client` |
| Chave | `JWT_KEY` |

A variável:

```text
JWT_KEY
```

é mapeada para:

```text
Jwt__Key
```

### 19.1. Conferir a variável

No Azure DevOps:

```text
Pipelines > Library > PetCare-Production
```

Confirme que `JWT_KEY` está preenchida e marcada como secreta.

### 19.2. Testar autenticação

No Swagger:

1. Abra o endereço público da API.
2. Localize o endpoint de login.
3. Informe as credenciais de um usuário válido de teste.
4. Execute a requisição.
5. Confira se a API retorna o token JWT.
6. Utilize o recurso **Authorize** do Swagger para autenticar.
7. Execute um endpoint protegido.

A autenticação é considerada validada quando o token é emitido e aceito em uma requisição protegida.

Não coloque credenciais reais ou tokens em commits, prints públicos ou documentação.

---

## 20. Testar as operações da API

A documentação Swagger permite visualizar os endpoints disponíveis e executar as operações.

Acesse:

```text
http://petcare-api-lucas-sprint4.southafricanorth.azurecontainer.io:8080/swagger
```

### 20.1. Testar os endpoints

Verifique as operações implementadas, incluindo:

- Autenticação.
- Cadastro de usuários.
- Cadastro e consulta de pets.
- Atualização de dados.
- Exclusão de registros.
- Demais recursos disponibilizados pela API.

Entre os endpoints utilizados no projeto estão:

```text
/api/Auth/login
/api/Auth/register
/api/Pet
```

As rotas e os formatos dos DTOs devem ser consultados no Swagger da versão publicada.

### 20.2. Critérios de validação

| Operação | Resultado esperado |
|---|---|
| Login | Retorno de autenticação para usuário válido |
| Cadastro | Registro criado corretamente |
| Consulta | Dados retornados pela API |
| Atualização | Dados modificados corretamente |
| Exclusão | Registro removido corretamente |
| Endpoint protegido | Acesso autorizado com token válido |
| Banco Oracle | Persistência dos dados confirmada |

A validação do CRUD deve ser realizada com dados de teste e conferência da persistência no Oracle.

---

## 21. Comandos de diagnóstico e manutenção

### 21.1. Consultar recursos do Resource Group

```powershell
az resource list `
  --resource-group rg-petcare-sprint4 `
  --output table
```

### 21.2. Consultar informações do ACR

```powershell
az acr show `
  --name petcareacr12345 `
  --resource-group rg-petcare-sprint4 `
  --output table
```

### 21.3. Consultar imagens publicadas

```powershell
az acr repository list `
  --name petcareacr12345 `
  --output table
```

```powershell
az acr repository show-tags `
  --name petcareacr12345 `
  --repository petcare-api `
  --output table
```

### 21.4. Consultar o ACI

```powershell
az container show `
  --resource-group rg-petcare-sprint4 `
  --name petcare-api-aci `
  --output table
```

### 21.5. Consultar os logs

```powershell
az container logs `
  --resource-group rg-petcare-sprint4 `
  --name petcare-api-aci
```

### 21.6. Excluir o ACI manualmente

Este comando deve ser utilizado somente quando houver necessidade de excluir o container manualmente:

```powershell
az container delete `
  --resource-group rg-petcare-sprint4 `
  --name petcare-api-aci `
  --yes
```

**Atenção:** depois da exclusão, os comandos `az container show` e `az container logs` não funcionarão até que o ACI seja criado novamente pelo CD.

Não é necessário excluir o ACI manualmente antes de uma publicação normal, pois o pipeline CD realiza o procedimento de substituição.

---

## 22. Problemas encontrados e soluções aplicadas

### 22.1. Pipeline não encontrava o PowerShell Core

**Problema:**

```text
Unable to locate executable file: 'pwsh'
```

**Causa:**

O agente Windows não possuía o executável `pwsh` disponível para a tarefa configurada.

**Solução:**

A tarefa AzureCLI foi configurada para utilizar Windows PowerShell, com:

```text
scriptType: ps
```

em vez de:

```text
scriptType: pscore
```

### 22.2. Erro de permissão no Azure

**Problema:**

A identidade do pipeline não tinha permissões suficientes para consultar e administrar os recursos Azure.

**Solução:**

Foi concedido o papel:

```text
Contributor
```

No escopo do Resource Group:

```text
rg-petcare-sprint4
```

### 22.3. Permissão incorreta inicialmente selecionada

**Problema:**

Foi selecionado inicialmente um papel que não atendia à necessidade de gerenciamento dos recursos.

**Solução:**

A atribuição foi corrigida para:

```text
Contributor
```

### 22.4. Comando `az container wait` não reconhecido

**Problema:**

```text
ERROR: 'wait' is misspelled or not recognized by the system.
```

**Causa:**

A versão instalada do Azure CLI não reconhecia o comando utilizado.

**Solução:**

A validação passou a consultar o estado do recurso com:

```powershell
az container show
```

Verificando:

```text
provisioningState = Succeeded
```

e:

```text
currentState.state = Running
```

### 22.5. Variáveis Oracle e JWT vazias

**Problema:**

O resultado da consulta do ACI apresentou:

```text
ConnectionStrings__RecommendaContextOracle
value: null
```

e:

```text
Jwt__Key
value: null
```

**Causa:**

As variáveis secretas não estavam sendo fornecidas corretamente ao container.

**Solução:**

As variáveis foram preenchidas no Variable Group:

```text
PetCare-Production
```

Após corrigir a configuração, foi executado novamente o processo de CD.

### 22.6. Consulta do ACI após exclusão

**Problema:**

```text
ResourceNotFound
```

**Causa:**

O recurso havia sido excluído pelo comando:

```powershell
az container delete
```

**Solução:**

Executar novamente o pipeline CD para criar o container e, depois, consultar seu estado.

### 22.7. Avisos do ASP.NET Data Protection

Durante a inicialização podem aparecer mensagens como:

```text
Storing keys in a directory '/home/app/.aspnet/DataProtection-Keys'
```

Esses avisos indicam que as chaves do Data Protection podem não persistir após a destruição do container.

A aplicação iniciou corretamente no ambiente utilizado, mas, caso sejam usados recursos que dependam dessas chaves entre reinicializações ou substituições do container, é necessário configurar armazenamento persistente e proteção adequada das chaves.

---

## 23. Segurança e boas práticas

- Manter `ORACLE_CONNECTION_STRING` como variável secreta.
- Manter `JWT_KEY` como variável secreta.
- Não armazenar senhas em arquivos YAML.
- Não enviar credenciais para o GitHub.
- Não compartilhar logs que exibam credenciais.
- Utilizar permissões IAM com escopo adequado.
- Utilizar autenticação por Service Connection.
- Manter o Dockerfile com usuário não privilegiado.
- Validar a imagem antes de publicar.
- Não compartilhar tokens JWT.
- Não reutilizar credenciais expostas em logs.
- Rotacionar credenciais caso tenham sido divulgadas.
- Considerar o uso de identidade gerenciada e permissões específicas de pull do ACR como evolução de segurança.

**Observação sobre o endpoint:** a publicação utilizada neste projeto está configurada com HTTP na porta 8080. Para um ambiente de produção real, deve-se configurar HTTPS, certificado e um ponto de entrada seguro.

---

## 24. Checklist final de execução

### Preparação

- [x] Projeto .NET 9 organizado.
- [x] Solução localizada em `API\Challenge.sln`.
- [x] Projetos da API e testes presentes.
- [x] Repositório GitHub configurado.
- [x] Código consolidado no repositório.
- [x] Compilação e testes locais realizados.

### Azure

- [x] Assinatura Azure selecionada.
- [x] Resource Group criado.
- [x] ACR criado.
- [x] Login Server conferido.
- [x] Acesso administrativo do ACR configurado.
- [x] Permissão IAM Contributor atribuída.
- [x] Service Connection `sc-azure-petcare` configurada.
- [x] Service Connection `sc-petcare-acr` configurada.
- [x] Agente self-hosted `AgentLocal` configurado.

### Variáveis e segredos

- [x] Variable Group `PetCare-Production` criado.
- [x] `AZURE_RESOURCE_GROUP` configurada.
- [x] `ACR_NAME` configurada.
- [x] `ACI_NAME` configurada.
- [x] `ACI_DNS_LABEL` configurada.
- [x] `ORACLE_CONNECTION_STRING` configurada como secreta.
- [x] `JWT_KEY` configurada como secreta.

### CI

- [x] Pipeline CI configurado.
- [x] Restore executado.
- [x] Build executado.
- [x] Testes executados.
- [x] Artefato `drop` publicado.
- [x] Imagem Docker construída.
- [x] Imagem enviada ao ACR.

### CD e publicação

- [x] Pipeline CD configurado.
- [x] Autenticação Azure realizada.
- [x] Imagem correta identificada.
- [x] ACI criado.
- [x] Imagem baixada pelo ACI.
- [x] Container iniciado.
- [x] Estado `Succeeded` confirmado.
- [x] Estado `Running` confirmado.
- [x] Logs de inicialização conferidos.
- [x] Variáveis Oracle e JWT fornecidas ao container.

### Validação da aplicação

- [x] Swagger externo acessível.
- [x] Conexão com Oracle validada.
- [x] Autenticação JWT validada.
- [x] Operações CRUD testadas.
- [x] Persistência dos dados validada.
- [x] Processo CI/CD executado com sucesso.

---

## 25. Resultado final

A entrega da Sprint 4 contempla a automação do processo de integração e publicação da API PetCare.

O código-fonte é armazenado no GitHub. O Azure DevOps executa o pipeline de CI, que restaura, compila e testa a solução .NET, publica o artefato e gera a imagem Docker. Essa imagem é armazenada no Azure Container Registry.

Em seguida, o pipeline de CD utiliza a imagem publicada para criar o Azure Container Instance, configurando as variáveis necessárias para execução da aplicação e conexão com o Oracle.

A aplicação é disponibilizada publicamente por meio do FQDN do ACI, permitindo acessar o Swagger e testar os recursos da API.

**Resultado:** ambiente de integração e entrega contínua configurado e validado, com publicação da API .NET 9 no Azure Container Instances e conexão com o banco Oracle.

---

## 26. Comandos principais para o professor reproduzir a validação

Esta seção reúne os comandos de consulta e teste em uma sequência única. Deve ser executada depois que os recursos Azure estiverem provisionados e o pipeline CD tiver concluído com sucesso.

### Passo 1 — Autenticar no Azure

```powershell
az login
```

### Passo 2 — Selecionar a assinatura

```powershell
az account set --subscription "4a768455-d8f3-4183-baf3-a527004fca82"
```

### Passo 3 — Conferir a assinatura

```powershell
az account show --output table
```

### Passo 4 — Conferir o Resource Group

```powershell
az group show `
  --name rg-petcare-sprint4 `
  --output table
```

### Passo 5 — Conferir o ACR

```powershell
az acr show `
  --name petcareacr12345 `
  --resource-group rg-petcare-sprint4 `
  --output table
```

### Passo 6 — Conferir as imagens publicadas

```powershell
az acr repository show-tags `
  --name petcareacr12345 `
  --repository petcare-api `
  --output table
```

### Passo 7 — Conferir o ACI

```powershell
az container show `
  --resource-group rg-petcare-sprint4 `
  --name petcare-api-aci `
  --output table
```

### Passo 8 — Conferir o provisionamento

```powershell
az container show `
  --resource-group rg-petcare-sprint4 `
  --name petcare-api-aci `
  --query provisioningState `
  --output tsv
```

Resultado esperado:

```text
Succeeded
```

### Passo 9 — Conferir o estado do container

```powershell
az container show `
  --resource-group rg-petcare-sprint4 `
  --name petcare-api-aci `
  --query "containers[0].instanceView.currentState.state" `
  --output tsv
```

Resultado esperado:

```text
Running
```

### Passo 10 — Obter o endereço público

```powershell
$fqdn = az container show `
  --resource-group rg-petcare-sprint4 `
  --name petcare-api-aci `
  --query ipAddress.fqdn `
  --output tsv
```

```powershell
$fqdn
```

### Passo 11 — Consultar os logs

```powershell
az container logs `
  --resource-group rg-petcare-sprint4 `
  --name petcare-api-aci
```

### Passo 12 — Testar o Swagger

```powershell
Invoke-WebRequest `
  -Uri "http://${fqdn}:8080/swagger/index.html" `
  -UseBasicParsing
```

### Passo 13 — Testar o endpoint Oracle

```powershell
Invoke-WebRequest `
  -Uri "http://${fqdn}:8080/api/oracle/external-service" `
  -UseBasicParsing
```

### Passo 14 — Testar autenticação e CRUD

Acesse o Swagger pelo endereço:

```text
http://petcare-api-lucas-sprint4.southafricanorth.azurecontainer.io:8080/swagger
```

Execute os testes de autenticação, autorização, cadastro, consulta, atualização e exclusão utilizando os endpoints e DTOs apresentados na documentação da API.

---

**Fim do HOW-TO — DevOps Sprint 4 | PetCare API .NET 9 | Oracle | Azure DevOps | Docker | ACR | ACI**