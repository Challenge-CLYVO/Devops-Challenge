# HOW TO — Sprint 4

## DevOps Tools & Cloud Computing — PetCare

**Stack:** .NET 9, Oracle FIAP, GitHub, Azure DevOps, Azure Container Registry (ACR) e Azure Container Instances (ACI).

**Objetivo:** automatizar a compilação, os testes, a geração de artefatos e a publicação da API PetCare na nuvem, mantendo o Oracle FIAP como banco de dados.

---

# 1. Configuração do Agent do Azure DevOps

O projeto utiliza um agente local do Azure Pipelines para executar as etapas de CI na máquina configurada.

## 1.1. Instalar o Agent

1. Acessar o projeto no Azure DevOps.
2. Abrir **Project Settings**.
3. Acessar **Agent Pools**.
4. Selecionar o pool `Default`.
5. Abrir a opção para adicionar um novo agente.
6. Selecionar o sistema operacional Windows.
7. Baixar o pacote do Azure Pipelines Agent.
8. Extrair os arquivos na pasta:

```text
C:\agent
```

## 1.2. Configurar o Agent

Abrir o PowerShell como administrador e acessar a pasta:

```powershell
Set-Location "C:\agent"
```

Executar o script de configuração:

```powershell
.\config.cmd
```

Durante a configuração:

- Informar a URL da organização Azure DevOps.
- Selecionar o pool `Default`.
- Informar o nome do agente: `AgentLocal`.
- Manter a pasta de trabalho padrão, quando solicitado.
- Configurar a autenticação conforme o método disponibilizado pelo Azure DevOps.

O token de configuração deve ser utilizado somente no processo de instalação. Não colocar tokens em arquivos do projeto ou no repositório.

## 1.3. Iniciar o Agent

Após concluir a configuração, iniciar o agente:

```powershell
.\run.cmd
```

Manter essa janela aberta enquanto as pipelines precisarem utilizar o agente.

Se o agente tiver sido configurado como serviço do Windows, conferir se o serviço está em execução.

## 1.4. Validar o Agent

No Azure DevOps:

1. Acessar **Project Settings**.
2. Abrir **Agent Pools**.
3. Selecionar `Default`.
4. Conferir se o agente `AgentLocal` aparece como **Online**.

**Importante:** a máquina que executa o Agent precisa estar ligada e conectada à internet durante a execução das pipelines.

O pipeline de CI utiliza o pool `Default`, portanto o agente local precisa estar disponível para executar os trabalhos.

---

# 2. Arquitetura final

A arquitetura planejada será:

| Componente                        | Tecnologia                             |
| --------------------------------- | -------------------------------------- |
| Código-fonte                      | GitHub                                 |
| Branch de desenvolvimento/entrega | develop                                |
| Branch de execução do CI          | master                                 |
| Backend                           | ASP.NET Core .NET 9                    |
| Banco de dados                    | Oracle FIAP                            |
| Integração contínua               | Azure DevOps CI                        |
| Agente de execução                | Azure Pipelines Agent — AgentLocal     |
| Artefatos                         | Azure DevOps Pipeline Artifacts        |
| Imagem Docker                     | Azure Container Registry               |
| Entrega contínua                  | Azure DevOps CD                        |
| Hospedagem da API                 | Azure Container Instances              |
| Monitoramento básico              | Health Checks, Serilog e OpenTelemetry |

**Fluxo planejado:**

1. Desenvolvedor altera o código na branch `develop`.
2. Desenvolvedor envia as alterações ao GitHub.
3. As alterações são integradas à branch `master` por Pull Request.
4. Azure DevOps CI detecta a alteração na `master`.
5. O AgentLocal executa as etapas configuradas no CI.
6. CI restaura dependências e compila a solução.
7. CI executa testes unitários e de integração.
8. CI gera e publica o artefato.
9. CI constrói e publica a imagem Docker no ACR.
10. Azure DevOps CD detecta a conclusão bem-sucedida do CI.
11. CD publica a imagem no ACI.
12. A API publicada conecta-se ao Oracle FIAP.
13. Usuário acessa a aplicação pela internet.

**Importante:** o roteiro exige CI disparado na `master`, mas também exige o código de entrega na `develop`. Por isso, manteremos as duas branches e utilizaremos Pull Request para integrar as alterações.

---

# 3. Preparação do ambiente

## 3.1. Ferramentas necessárias

Instalar ou conferir:

- Visual Studio ou Visual Studio Code;
- .NET SDK 9;
- Git;
- Docker Desktop;
- Azure CLI;
- Conta GitHub;
- Conta Azure;
- Acesso ao Oracle FIAP;
- Oracle SQL Developer.

Verificar as instalações no PowerShell:

```powershell
dotnet --version
git --version
docker --version
az version
```

O SDK deve apresentar uma versão `9.0.x`.

## 3.2. Entrar na pasta do projeto

Acessar a pasta do repositório:

```powershell
Set-Location "C:\Users\USER\Documents\2TDSPF\Devops\devops-challenge"
```

Caso o projeto esteja em outra pasta, utilizar o diretório correto.

Conferir a estrutura:

```powershell
Get-ChildItem
Get-ChildItem -Recurse -Filter "Challenge.sln"
Get-ChildItem -Recurse -Filter "sql_challenge.sql"
```

Conferir os projetos:

```powershell
dotnet sln .\API\Challenge.sln list
```

---

# 4. Conferência do GitHub

## 4.1. Conferir branches e repositório

```powershell
git status -sb
git branch -a -vv
git remote -v
```

Conferir se as branches `develop` e `master` existem.

Se `master` ainda não existir no remoto, e a `develop` já estiver atualizada, criar a branch:

```powershell
git checkout develop
git push origin develop:master
```

Depois, no GitHub, conferir as duas branches e configurar `master` como branch de integração, se necessário.

Não excluir `develop`.

## 4.2. Enviar alterações para develop

Sempre que fizer uma alteração:

```powershell
git checkout develop
git status
```

Adicionar, registrar e enviar as alterações conforme o fluxo de versionamento do projeto.

Mensagem de commit de exemplo:

```text
chore: prepare project for DevOps deployment
```

Utilizar uma mensagem específica para cada alteração.

## 4.3. Integrar develop com master

No GitHub:

1. Abrir Pull Requests.
2. Criar Pull Request.
3. Base: `master`.
4. Compare: `develop`.
5. Revisar as alterações.
6. Fazer Merge.

Esse merge deverá disparar automaticamente o CI configurado para a branch `master`.

---

# 5. Preparação do Oracle FIAP

## 5.1. Configurar conexão no SQL Developer

Criar uma conexão com os dados fornecidos pela FIAP:

| Campo           | Valor               |
| --------------- | ------------------- |
| Connection Name | Oracle FIAP PetCare |
| Username        | Seu usuário/RM FIAP |
| Password        | Sua senha FIAP      |
| Hostname        | oracle.fiap.com.br  |
| Port            | 1521                |
| Service Name    | orcl                |

Testar a conexão.

Também é possível verificar a porta no PowerShell:

```powershell
Test-NetConnection oracle.fiap.com.br -Port 1521
```

Esse teste confirma a conectividade a partir do computador local. Ele não comprova que o Azure conseguirá acessar o banco.

## 5.2. Criar as tabelas

Utilizar o arquivo:

```text
Database-Challenge/docker-entrypoint-initdb.d/sql_challenge.sql
```

Abrir no SQL Developer e executar com F5.

**Atenção:** o script começa com oito comandos `DROP TABLE`. Eles excluem tabelas existentes e seus relacionamentos.

Em uma conta FIAP que já possua dados, não executar esses comandos sem antes conferir o conteúdo. Se o schema estiver vazio, executar as instruções de criação a partir do primeiro `CREATE TABLE`.

## 5.3. Conferir as tabelas

```sql
SELECT TABLE_NAME
FROM USER_TABLES
ORDER BY TABLE_NAME;
```

Devem aparecer:

```text
APLICACAO_VACINA
CLINICA
CONSULTA
HISTORICO_SAUDE
LOG_ERRO
PET
TUTOR
VACINA
```

## 5.4. Conferir as chaves primárias

```sql
SELECT
    TABLE_NAME,
    CONSTRAINT_NAME,
    CONSTRAINT_TYPE
FROM USER_CONSTRAINTS
WHERE CONSTRAINT_TYPE = 'P'
ORDER BY TABLE_NAME;
```

## 5.5. Conferir as chaves estrangeiras

```sql
SELECT
    TABLE_NAME,
    CONSTRAINT_NAME,
    R_CONSTRAINT_NAME
FROM USER_CONSTRAINTS
WHERE CONSTRAINT_TYPE = 'R'
ORDER BY TABLE_NAME;
```

## 5.6. Conferir a geração automática dos IDs

Existe um ponto importante no projeto:

O `AppDbContext.cs` configura os IDs de Tutor, Pet, Clínica, Consulta, Vacina, Aplicação de Vacina e Histórico de Saúde com `ValueGeneratedOnAdd()`.

Entretanto, o script SQL fornecido define `GENERATED ALWAYS AS IDENTITY` somente para `LOG_ERRO`.

Precisamos alinhar a geração desses sete IDs antes de validar o CRUD no Oracle.

Executar:

```sql
SELECT
    TABLE_NAME,
    COLUMN_NAME,
    GENERATION_TYPE
FROM USER_TAB_IDENTITY_COLS
ORDER BY TABLE_NAME;
```

Conferir as sequences existentes:

```sql
SELECT SEQUENCE_NAME
FROM USER_SEQUENCES
ORDER BY SEQUENCE_NAME;
```

Não considerar o banco pronto para o CRUD até confirmar que a geração de IDs no Oracle está compatível com o mapeamento do Entity Framework.

A solução poderá envolver colunas Identity, sequences e defaults, ou ajuste do mapeamento da aplicação. A escolha deve ser validada antes de alterar o banco.

## 5.7. Conferir os registros

```sql
SELECT COUNT(*) AS TOTAL FROM TUTOR;
SELECT COUNT(*) AS TOTAL FROM PET;
SELECT COUNT(*) AS TOTAL FROM CLINICA;
SELECT COUNT(*) AS TOTAL FROM CONSULTA;
SELECT COUNT(*) AS TOTAL FROM VACINA;
SELECT COUNT(*) AS TOTAL FROM APLICACAO_VACINA;
SELECT COUNT(*) AS TOTAL FROM HISTORICO_SAUDE;
SELECT COUNT(*) AS TOTAL FROM LOG_ERRO;
```

---

# 6. Configuração segura da aplicação .NET

## 6.1. Remover segredos do appsettings.json

No arquivo:

```text
API/PetCare.API/appsettings.json
```

A connection string Oracle e a chave JWT não devem permanecer com valores sensíveis.

Manter a estrutura sem credenciais:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "HealthChecks": {
    "ExternalServiceUrl": "https://www.google.com"
  },
  "Jwt": {
    "Issuer": "PetCare.API",
    "Audience": "PetCare.Client"
  }
}
```

A aplicação já lê as configurações usando:

```csharp
builder.Configuration.GetConnectionString(
    "RecommendaContextOracle");
```

E:

```csharp
builder.Configuration["Jwt:Key"];
```

Portanto, podemos fornecer esses valores por User Secrets no ambiente local e por variáveis protegidas no Azure.

A chave JWT que já foi colocada no arquivo de configuração deve ser considerada exposta e substituída por uma nova.

## 6.2. Configurar User Secrets

Executar:

```powershell
dotnet user-secrets init --project .\API\PetCare.API\PetCare.API.csproj
```

Gerar uma chave JWT nova:

```powershell
$bytes = New-Object byte[] 64
$rng = [System.Security.Cryptography.RNGCryptoServiceProvider]::Create()
$rng.GetBytes($bytes)
$rng.Dispose()

$JWT_KEY = [Convert]::ToBase64String($bytes)

Write-Output $JWT_KEY
```

Salvar:

```powershell
dotnet user-secrets set "Jwt:Key" "$JWT_KEY" --project .\API\PetCare.API\PetCare.API.csproj
```

Configurar a connection string Oracle com seu usuário e senha FIAP:

```powershell
$oracleUser = Read-Host "Informe seu usuario FIAP"
$oraclePassword = Read-Host "Informe sua senha FIAP" -AsSecureString

$ptr = [Runtime.InteropServices.Marshal]::SecureStringToBSTR($oraclePassword)

try {
    $plainPassword = [Runtime.InteropServices.Marshal]::PtrToStringBSTR($ptr)

    $connectionString = "Data Source=oracle.fiap.com.br:1521/orcl;User ID=$oracleUser;Password=$plainPassword;"

    dotnet user-secrets set "ConnectionStrings:RecommendaContextOracle" "$connectionString" --project .\API\PetCare.API\PetCare.API.csproj
}
finally {
    [Runtime.InteropServices.Marshal]::ZeroFreeBSTR($ptr)
    Remove-Variable plainPassword -ErrorAction SilentlyContinue
    Remove-Variable connectionString -ErrorAction SilentlyContinue
}
```

## 6.3. Conferir os segredos locais

```powershell
dotnet user-secrets list --project .\API\PetCare.API\PetCare.API.csproj
```

Não publicar a saída desse comando em vídeos, prints ou repositórios, pois ela contém valores sensíveis.

## 6.4. Testar a aplicação localmente

```powershell
dotnet restore .\API\Challenge.sln

dotnet build .\API\Challenge.sln --configuration Release

dotnet test .\API\Challenge.sln --configuration Release
```

Iniciar a API:

```powershell
dotnet run --project .\API\PetCare.API\PetCare.API.csproj
```

Testar:

```powershell
Invoke-WebRequest "http://localhost:5100/health"
```

Abrir:

```text
http://localhost:5100/swagger
```

Confirmar a conexão com o Oracle e o funcionamento da API.

---

# 7. Configuração do Azure

## 7.1. Entrar na conta Azure

```powershell
az login
```

Conferir as assinaturas:

```powershell
az account list --output table
```

Selecionar a assinatura:

```powershell
az account set --subscription "ID-DA-SUA-SUBSCRIPTION"
```

Conferir:

```powershell
az account show --output table
```

## 7.2. Registrar os provedores

```powershell
az provider register --namespace Microsoft.ContainerRegistry

az provider register --namespace Microsoft.ContainerInstance
```

## 7.3. Criar Resource Group

O Resource Group utilizado no projeto:

```powershell
$RESOURCE_GROUP = "rg-petcare-sprint4"
$LOCATION = "southafricanorth"
```

Criar:

```powershell
az group create `
    --name $RESOURCE_GROUP `
    --location $LOCATION
```

## 7.4. Criar Azure Container Registry

Escolher um nome único, somente com letras minúsculas e números.

Nome utilizado:

```powershell
$ACR_NAME = "petcareacr12345"
```

Criar:

```powershell
az acr create `
    --resource-group $RESOURCE_GROUP `
    --name $ACR_NAME `
    --sku Basic `
    --admin-enabled true
```

Consultar:

```powershell
az acr show `
    --name $ACR_NAME `
    --resource-group $RESOURCE_GROUP `
    --query loginServer `
    --output tsv
```

O endereço esperado é:

```text
petcareacr12345.azurecr.io
```

O ACR armazenará as imagens Docker da API.

## 7.5. Permissões necessárias no Azure

As conexões de serviço do Azure DevOps precisam ter permissões compatíveis com as operações executadas pelas pipelines.

Para este projeto, são necessárias duas permissões distintas:

| Identidade                                      | Permissão   | Escopo                              |
| ----------------------------------------------- | ----------- | ----------------------------------- |
| Service Principal da conexão `sc-azure-petcare` | Contributor | Resource Group `rg-petcare-sprint4` |
| Service Principal da conexão `sc-petcare-acr`   | AcrPush     | ACR `petcareacr12345`               |

### Contributor

A identidade utilizada pela conexão `sc-azure-petcare` precisa ter a função **Contributor** no Resource Group `rg-petcare-sprint4`, ou em um escopo superior.

Essa permissão permite que a pipeline de CD execute operações de gerenciamento dos recursos Azure, como excluir e criar o Azure Container Instance.

O papel Contributor deve ser atribuído à identidade correta da conexão de serviço, e não apenas ao usuário que criou a conexão.

### AcrPush

A identidade utilizada pela conexão `sc-petcare-acr` precisa ter a função **AcrPush** no ACR.

Essa permissão permite autenticar e publicar imagens Docker no Azure Container Registry.

No modo de permissões clássico do ACR, a função `AcrPush` é utilizada para permitir o envio de imagens.

**Importante:** Contributor e AcrPush não são a mesma permissão. A primeira permite gerenciar recursos Azure; a segunda permite publicar imagens no registro de containers.

Conferir as atribuições de função do ACR:

```powershell
az role assignment list `
    --scope "/subscriptions/ID-DA-SUA-SUBSCRIPTION/resourceGroups/rg-petcare-sprint4/providers/Microsoft.ContainerRegistry/registries/petcareacr12345" `
    --output table
```

Conferir também as permissões do Resource Group no Portal Azure:

**Resource Group → Access control (IAM) → Role assignments.**

---

# 8. Configuração do Docker

O projeto já possui Dockerfile na pasta `API`.

O Dockerfile utiliza:

- .NET SDK 9 para compilação;
- ASP.NET Runtime 9 para execução;
- Porta 8080;
- Ambiente Production;
- Usuário não-root.

## 8.1. Testar construção da imagem

Na raiz do repositório:

```powershell
docker build -t petcare-api:local -f .\API\Dockerfile .\API
```

Conferir:

```powershell
docker images
```

## 8.2. Executar localmente

Depois de configurar User Secrets, preparar as variáveis de ambiente no PowerShell.

A connection string e a chave JWT devem ser fornecidas como variáveis de ambiente, sem inserir valores sensíveis diretamente no comando ou no repositório.

As variáveis utilizadas pela aplicação são:

```text
ASPNETCORE_ENVIRONMENT
ASPNETCORE_URLS
ConnectionStrings__RecommendaContextOracle
Jwt__Key
Jwt__Issuer
Jwt__Audience
```

Executar o container fornecendo as variáveis necessárias:

```powershell
docker run --rm `
-p 8080:8080 `
-e ASPNETCORE_ENVIRONMENT `
-e ASPNETCORE_URLS `
-e ConnectionStrings__RecommendaContextOracle `
-e Jwt__Key="$JWT_KEY" `
-e Jwt__Issuer="PetCare.API" `
-e Jwt__Audience="PetCare.Client" `
petcare-api:local

```

As variáveis de conexão e JWT precisam estar definidas na sessão antes de iniciar o container.

Testar:

```powershell
Invoke-WebRequest "http://localhost:8080/health"
```

---

# 9. Configuração do projeto Azure DevOps

## 9.1. Criar projeto

Acessar:

[https://dev.azure.com/](https://dev.azure.com/)

Criar projeto com:

| Campo             | Valor                                                           |
| ----------------- | --------------------------------------------------------------- |
| Project Name      | Sprint 4 – Azure DevOps                                         |
| Description       | Projeto para entrega da Sprint 4 do professor NOME DO PROFESSOR |
| Visibility        | Private                                                         |
| Version Control   | Git                                                             |
| Work Item Process | Scrum ou Agile                                                  |

Substituir o nome do professor na descrição.

## 9.2. Cadastrar integrantes

Utilizar o padrão:

```text
RM00000 - Nome do Integrante - Turma
```

Cadastrar todos os integrantes.

## 9.3. Conectar GitHub

No Azure DevOps:

1. Acessar Repos.
2. Selecionar conexão/importação de repositório GitHub.
3. Autorizar o acesso à conta GitHub.
4. Selecionar o repositório PetCare.
5. Confirmar a conexão.

## 9.4. Convidar professor

Acessar:

**Project Settings → Permissions ou Teams.**

Adicionar o professor ao projeto com nível de acesso Basic.

Confirmar que ele consegue acessar:

- Projeto;
- Repos;
- Pipelines;
- Artefatos.

---

# 10. Criar conexões de serviço no Azure DevOps

Acessar:

**Project Settings → Service Connections → New Service Connection.**

Serão criadas duas conexões: uma para gerenciar os recursos Azure e outra para publicar imagens no ACR.

## 10.1. Conexão Azure — sc-azure-petcare

Selecionar:

```text
Azure Resource Manager
```

Configurar a autenticação utilizando Service Principal, conforme a opção disponível na organização.

Nome da conexão:

```text
sc-azure-petcare
```

Selecionar a assinatura Azure e configurar o acesso ao escopo necessário.

A identidade associada a essa conexão precisa possuir a função:

```text
Contributor
```

Escopo:

```text
rg-petcare-sprint4
```

Essa permissão será utilizada pela pipeline CD para gerenciar o Azure Container Instance.

Autorizar o acesso para as pipelines.

## 10.2. Conexão ACR — sc-petcare-acr

Criar outra Service Connection.

Selecionar:

```text
Docker Registry
```

Em Registry Type, selecionar:

```text
Azure Container Registry
```

Em Authentication Type, utilizar:

```text
Service Principal
```

Nome da conexão:

```text
sc-petcare-acr
```

Selecionar:

- A assinatura Azure utilizada no projeto;
- O Azure Container Registry `petcareacr12345`.

Conferir se a identidade associada à conexão possui a função:

```text
AcrPush
```

O escopo dessa permissão deve ser o ACR `petcareacr12345`.

**Atenção:** a identidade que possui `AcrPush` precisa ser a mesma identidade utilizada pela conexão `sc-petcare-acr`. Conferir o Client ID da conexão e a identidade que recebeu a atribuição no ACR.

Se a conexão utilizar uma identidade diferente daquela que possui `AcrPush`, a publicação da imagem poderá falhar com erro de autenticação.

Autorizar o acesso para as pipelines.

**Importante:** não utilizar Managed Service Identity automaticamente. O AgentLocal está sendo executado em uma máquina Windows local; MSI só deve ser utilizado quando a infraestrutura e a identidade gerenciada estiverem efetivamente configuradas para isso.

---

# 11. Criar variáveis protegidas

Acessar:

**Pipelines → Library → Variable Groups → Add.**

Nome:

```text
PetCare-Production
```

Criar as variáveis:

| Nome                     | Conteúdo                      | Secret |
| ------------------------ | ----------------------------- | ------ |
| AZURE_RESOURCE_GROUP     | rg-petcare-sprint4            | Não    |
| ACR_NAME                 | petcareacr12345               | Não    |
| ACI_NAME                 | petcare-api-aci               | Não    |
| ACI_DNS_LABEL            | petcare-api-lucas-sprint4     | Não    |
| ORACLE_CONNECTION_STRING | Connection string Oracle FIAP | Sim    |
| JWT_KEY                  | Nova chave JWT                | Sim    |

Marcar as duas últimas como secret.

Não colocar senhas, tokens ou credenciais diretamente nos arquivos YAML.

As variáveis devem corresponder aos nomes esperados pela aplicação e pela pipeline.

## 11.1. Autorizar o Variable Group

Depois de criar o grupo:

1. Abrir o grupo `PetCare-Production`.
2. Conferir os nomes das variáveis.
3. Conferir se `ORACLE_CONNECTION_STRING` e `JWT_KEY` estão marcadas como secret.
4. Autorizar o uso do grupo pelas pipelines, conforme a configuração de segurança do Azure DevOps.

---

# 12. Pipeline CI — Integração Contínua

O CI deve executar automaticamente após alterações na branch `master`.

O arquivo YAML do CI já está no repositório GitHub. Portanto, não é necessário recriar ou copiar o código YAML para este HOW TO.

Arquivo:

```text
azure-pipelines-ci.yml
```

## 12.1. Criar pipeline no Azure DevOps

1. Acessar Pipelines.
2. Selecionar New Pipeline.
3. Escolher GitHub.
4. Selecionar o repositório PetCare.
5. Selecionar Existing Azure Pipelines YAML file.
6. Escolher a branch `master`.
7. Selecionar `azure-pipelines-ci.yml`.
8. Salvar.

Nome da pipeline:

```text
PetCare-CI
```

Conferir se o pipeline utiliza o pool:

```text
Default
```

O AgentLocal precisa estar Online para executar as tarefas.

## 12.2. Validar CI

Após o primeiro commit integrado à `master`, verificar:

- Restore;
- Build;
- Testes;
- Publicação do artefato;
- Build Docker;
- Push da imagem no ACR.

Conferir os artefatos em:

**Pipelines → Execução → Related → Artifacts.**

Conferir a imagem no ACR:

```powershell
az acr repository list `
    --name petcareacr12345 `
    --output table
```

A imagem da API deverá aparecer no repositório do ACR.

**Importante:** se o push apresentar erro de autenticação, conferir se a conexão `sc-petcare-acr` está selecionada na tarefa Docker e se sua identidade possui `AcrPush` no ACR correto.

---

# 13. Pipeline CD — Entrega Contínua

O CD deverá iniciar automaticamente após uma execução bem-sucedida do CI na branch `master`.

O arquivo YAML do CD já está no repositório GitHub. Portanto, não é necessário recriar ou copiar o código YAML para este HOW TO.

Arquivo:

```text
azure-pipelines-cd.yml
```

## 13.1. Criar pipeline CD

No Azure DevOps:

1. Acessar Pipelines.
2. Selecionar New Pipeline.
3. Selecionar o repositório GitHub.
4. Selecionar Existing Azure Pipelines YAML file.
5. Escolher a branch que contém o arquivo.
6. Selecionar `azure-pipelines-cd.yml`.
7. Salvar.

Nome:

```text
PetCare-CD
```

Conferir se a pipeline utiliza:

- A conexão Azure `sc-azure-petcare`;
- O Variable Group `PetCare-Production`;
- A imagem publicada pelo CI no ACR.

O nome `PetCare-CI` utilizado como origem do pipeline de CI no arquivo de CD deve corresponder exatamente ao nome da pipeline criada no Azure DevOps.

## 13.2. Validar CD

Depois de uma execução bem-sucedida do CI na `master`, verificar se o CD foi disparado automaticamente.

Conferir:

- Download do artefato;
- Acesso ao ACR;
- Criação do container;
- Provisionamento;
- URL pública;
- Health check.

A identidade da conexão `sc-azure-petcare` precisa ter Contributor no Resource Group para executar as operações de implantação.

A conexão `sc-petcare-acr` precisa ter AcrPush para a publicação da imagem pelo CI.

A exclusão e recriação do container pode gerar alguns minutos de indisponibilidade durante o deploy.

---

# 14. Validar aplicação na nuvem

Consultar o container:

```powershell
az container show `
    --resource-group "rg-petcare-sprint4" `
    --name "petcare-api-aci" `
    --output table
```

Consultar o endereço:

```powershell
$FQDN = az container show `
    --resource-group "rg-petcare-sprint4" `
    --name "petcare-api-aci" `
    --query ipAddress.fqdn `
    --output tsv
```

Testar health check:

```powershell
Invoke-WebRequest "http://$FQDN`:8080/health"
```

Abrir Swagger:

```powershell
Start-Process "http://$FQDN`:8080/swagger"
```

O endereço retornado pelo ACI deverá ser utilizado na demonstração.

**Atenção ao Oracle FIAP:** a conexão deve ser testada a partir do container no Azure. O teste local de porta 1521 não comprova que a rede da FIAP aceitará conexões originadas no Azure. Se o Oracle FIAP bloquear o acesso externo ou exigir uma rede não disponível no ACI, será necessário validar uma solução de conectividade permitida antes de considerar o deploy concluído.

---

# 15. Testar CRUD completo

O projeto possui o controlador `PetController`, com:

| Método | Endpoint        | Operação         |
| ------ | --------------- | ---------------- |
| GET    | `/api/Pet`      | Consultar todos  |
| GET    | `/api/Pet/{id}` | Consultar por ID |
| POST   | `/api/Pet`      | Inserir          |
| PUT    | `/api/Pet/{id}` | Atualizar        |
| DELETE | `/api/Pet/{id}` | Excluir          |

Os endpoints de Pet exigem JWT.

## 15.1. Obter token

O projeto possui:

```text
POST /api/Auth/login
```

Esse endpoint retorna um token sem exigir corpo de autenticação no código atual.

No PowerShell:

```powershell
$BASE_URL = "http://SEU-FQDN:8080"

$response = Invoke-RestMethod `
    -Method Post `
    -Uri "$BASE_URL/api/Auth/login"

$TOKEN = $response.token

$headers = @{
    Authorization = "Bearer $TOKEN"
}
```

## 15.2. Consultar tutores

Antes de cadastrar um pet, conferir se existe um tutor no banco:

```sql
SELECT *
FROM TUTOR
ORDER BY ID_TUTOR;
```

Utilizar um `ID_TUTOR` existente.

## 15.3. Inserir pet

```powershell
$body = @{
    nome = "Pet DevOps"
    idade = 3
    especie = "Cachorro"
    raca = "Labrador"
    idTutor = 1
} | ConvertTo-Json

$created = Invoke-RestMethod `
    -Method Post `
    -Uri "$BASE_URL/api/Pet" `
    -Headers $headers `
    -ContentType "application/json" `
    -Body $body
```

Substituir `idTutor = 1` por um tutor realmente existente.

## 15.4. Conferir no Oracle

```sql
SELECT
    ID_PET,
    NOME,
    IDADE,
    ESPECIE,
    RACA,
    ID_TUTOR
FROM PET
WHERE NOME = 'Pet DevOps';
```

Anotar o ID gerado.

## 15.5. Consultar pet

```powershell
$petId = 1

Invoke-RestMethod `
    -Method Get `
    -Uri "$BASE_URL/api/Pet/$petId" `
    -Headers $headers
```

## 15.6. Atualizar pet

```powershell
$body = @{
    nome = "Pet DevOps Atualizado"
    idade = 4
    especie = "Cachorro"
    raca = "Labrador"
    idTutor = 1
} | ConvertTo-Json

Invoke-RestMethod `
    -Method Put `
    -Uri "$BASE_URL/api/Pet/$petId" `
    -Headers $headers `
    -ContentType "application/json" `
    -Body $body
```

Conferir no Oracle:

```sql
SELECT ID_PET, NOME, IDADE
FROM PET
WHERE ID_PET = 1;
```

## 15.7. Excluir pet

```powershell
Invoke-RestMethod `
    -Method Delete `
    -Uri "$BASE_URL/api/Pet/$petId" `
    -Headers $headers
```

Conferir:

```sql
SELECT *
FROM PET
WHERE ID_PET = 1;
```

O resultado deve ser vazio.

**Importante:** substituir o ID de exemplo pelo ID real criado no teste. Antes da exclusão, verificar se o pet não possui registros dependentes em Consulta, Aplicacao_vacina ou Historico_saude, pois as chaves estrangeiras podem impedir a exclusão.

Guardar prints ou gravação mostrando cada operação e o respectivo SELECT no Oracle.