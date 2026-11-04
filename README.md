# PetCare API — Challenge FIAP 2026 | DevOps Tools & Cloud Computing

**Aluno:** Henrique Teixeira Cesar | **RM:** 563088

**Aluno:** Lucas Rafael Solimene | **RM:** 565194

**Aluno:** Samyr Couto Oliveira | **RM:** 565562

**Turma:** 2TDSPF | 
**Empresa:** Clyvo Vet | 
**Disciplina:** DevOps Tools & Cloud Computing — Sprint 4 | 

## Sobre o projeto

O **PetCare API** é uma aplicação desenvolvida em .NET 9 com o objetivo de disponibilizar serviços relacionados ao gerenciamento de informações de pets.

Este projeto faz parte do desafio de DevOps e contempla a preparação da aplicação para execução em contêineres Docker, armazenamento de imagens no Azure Container Registry (ACR) e implantação no Azure Container Instances (ACI), utilizando pipelines de CI/CD no Azure DevOps.

## Tecnologias utilizadas

- **C# / .NET 9** — desenvolvimento da API.
- **ASP.NET Core** — construção dos endpoints REST.
- **Oracle Database** — banco de dados da aplicação.
- **Entity Framework Core** — mapeamento e acesso aos dados.
- **Docker** — conteinerização da aplicação.
- **Azure Container Registry (ACR)** — armazenamento das imagens Docker.
- **Azure Container Instances (ACI)** — execução do contêiner na nuvem.
- **Azure DevOps** — gerenciamento dos pipelines de CI/CD.
- **Git e GitHub** — versionamento do código-fonte.

## Arquitetura da solução

O processo de entrega da aplicação utiliza o seguinte fluxo:

1. O código-fonte é versionado no GitHub.
2. O Azure DevOps executa o pipeline de integração contínua (CI).
3. A aplicação é restaurada, compilada e testada.
4. Uma imagem Docker é construída.
5. A imagem é enviada ao Azure Container Registry.
6. O pipeline de entrega contínua (CD) publica a aplicação no Azure Container Instances.
7. A API fica disponível por meio de um endereço público.

## Estrutura do repositório

```text
Devops-Challenge/
│
├── API/
│   ├── Challenge.sln
│   ├── Dockerfile
│   ├── PetCare.API/
│   ├── PetCare.Application/
│   ├── PetCare.Domain/
│   ├── PetCare.Infrastructure/
│   ├── Tests.Integration/
│   └── Tests.Unit/
│
├── azure-pipelines/
│   ├── azure-pipelines-ci.yml
│   └── azure-pipelines-cd.yml
│
├── docs/
│   └── HOW-TO.md
│
├── .gitignore
└── README.md
```

> Os nomes e caminhos dos arquivos de pipeline devem corresponder aos arquivos efetivamente utilizados na configuração do Azure DevOps.

## Pré-requisitos

Para executar o projeto localmente, é necessário ter:

- .NET SDK 9.0.
- Git.
- Docker Desktop (para execução em contêiner).
- Acesso e configuração do banco de dados Oracle.

Para realizar a implantação na nuvem, também são necessários:

- Conta no Microsoft Azure.
- Organização e projeto no Azure DevOps.
- Azure Container Registry.
- Azure Container Instances.
- Service Connections configuradas no Azure DevOps.

## Como executar localmente

### 1. Clonar o repositório

```powershell
git clone URL_DO_REPOSITORIO
```

### 2. Acessar a pasta do projeto

```powershell
cd Devops-Challenge
```

### 3. Restaurar as dependências

```powershell
dotnet restore .\API\Challenge.sln
```

### 4. Compilar a solução

```powershell
dotnet build .\API\Challenge.sln --configuration Release
```

### 5. Executar os testes

```powershell
dotnet test .\API\Challenge.sln --configuration Release
```

### 6. Executar a API

Configure previamente as variáveis de ambiente e a conexão com o Oracle. Em seguida:

```powershell
dotnet run --project .\API\PetCare.API\PetCare.API.csproj
```

A API será disponibilizada localmente na porta configurada para execução.

## Configuração de variáveis de ambiente

A aplicação utiliza configurações externas para informações sensíveis.

| Variável | Finalidade |
|---|---|
| `ORACLE_CONNECTION_STRING` | Conexão com o banco Oracle |
| `JWT_KEY` | Chave utilizada na configuração JWT |

No Azure DevOps, essas informações devem ser cadastradas como variáveis secretas no grupo de variáveis utilizado pelo pipeline.

**Importante:** não publique senhas, chaves JWT ou strings de conexão no repositório.

## Docker

O projeto possui um `Dockerfile` para construir a imagem da API.

A imagem é preparada para executar a aplicação na porta `8080` dentro do contêiner.

Para construir a imagem manualmente, a partir do diretório `API`:

```powershell
docker build -t petcare-api -f Dockerfile .
```

Para executar localmente:

```powershell
docker run -d -p 8080:8080 --name petcare-api petcare-api
```

## Azure DevOps — CI/CD

O projeto utiliza pipelines no Azure DevOps para automatizar o processo de integração e entrega.

### Continuous Integration (CI)

O pipeline de CI é responsável por:

- Restaurar as dependências do projeto.
- Compilar a solução.
- Executar os testes automatizados.
- Construir a imagem Docker.
- Publicar a imagem no Azure Container Registry.

### Continuous Delivery (CD)

O pipeline de CD é responsável por:

- Autenticar nos serviços do Azure.
- Obter a imagem publicada no ACR.
- Configurar as variáveis necessárias para a aplicação.
- Criar ou atualizar o Azure Container Instance.
- Disponibilizar a API na nuvem.

Os arquivos YAML dos pipelines estão disponíveis no próprio repositório.

## Recursos utilizados no Azure

| Recurso | Identificação |
|---|---|
| Resource Group | `rg-petcare-sprint4` |
| Azure Container Registry | `petcareacr12345` |
| Azure Container Instance | `petcare-api-aci` |
| Região | `South Africa North` |
| Porta do contêiner | `8080` |

## Documentação de implantação

O passo a passo completo da configuração do ambiente, criação dos recursos Azure, configuração das permissões, pipelines e implantação está disponível em:

**[HOW-TO — Guia completo de implantação](docs/HOW-TO.md)**

## Organização Azure DevOps

Acesse a organização utilizada no desenvolvimento e na execução dos pipelines:

**Link da organização:**

https://dev.azure.com/DevOps-2TDSPF-RM565194/Sprint%204%20%E2%80%93%20Azure%20DevOps

## Demonstração em vídeo

O vídeo apresenta o funcionamento do projeto, a configuração da solução e o processo de implantação utilizando Docker, Azure Container Registry, Azure Container Instances e Azure DevOps.

**Vídeo de demonstração no YouTube:**

Link: https://www.youtube.com/watch?v=4Q7KMWOYS2I

## Autores

Projeto desenvolvido como parte do desafio acadêmico de DevOps — 2TDSPF.

---

**PetCare API — DevOps Challenge**  
.NET 9 | Docker | Azure | Azure DevOps | Oracle