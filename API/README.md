# 🐾 PetCare API

API REST desenvolvida em ASP.NET Core para gerenciamento de informações relacionadas à saúde e ao cuidado de animais de estimação. A aplicação permite administrar pets, tutores, vacinas, aplicações de vacina, clínicas veterinárias, consultas e históricos de saúde.

O projeto utiliza arquitetura em camadas, Entity Framework Core com Oracle, autenticação JWT, documentação via Swagger, testes automatizados e recursos de monitoramento e observabilidade.

## 👥 Integrantes

- **Lucas Rafael Solimene** — RM 565194
- **Samyr Couto Oliveira** — RM 565562
- **Henrique Teixeira Cesar** — RM 563088

## 🎯 Objetivo do projeto

Desenvolver uma API para centralizar e organizar informações relacionadas à saúde dos animais, permitindo o gerenciamento de seus dados, o registro de vacinas e consultas e o acompanhamento do histórico de saúde.

A aplicação foi estruturada para separar as responsabilidades de apresentação, regras de negócio, domínio e persistência, facilitando a manutenção, os testes e a evolução do sistema.

## 🛠️ Tecnologias utilizadas

- **C# e .NET 9** — desenvolvimento da aplicação.
- **ASP.NET Core Web API** — criação dos endpoints REST.
- **Entity Framework Core** — persistência e acesso aos dados.
- **Oracle Database** — banco de dados relacional.
- **JWT Bearer** — validação de tokens de autenticação.
- **Swagger / OpenAPI** — documentação e exploração dos endpoints.
- **AutoMapper** — mapeamento entre entidades e DTOs.
- **Serilog** — registro estruturado de logs.
- **OpenTelemetry** — instrumentação de traces e métricas.
- **Prometheus** — exposição de métricas para monitoramento.
- **Health Checks** — verificação da disponibilidade da aplicação e de dependências.
- **xUnit** — execução de testes automatizados.
- **Moq** — criação de objetos simulados nos testes.
- **Microsoft.AspNetCore.Mvc.Testing** — testes de integração da API.
- **Git e GitHub** — versionamento e colaboração.

## 🏗️ Arquitetura do projeto

A solução utiliza uma arquitetura em camadas, separando responsabilidades para reduzir o acoplamento e facilitar a manutenção.

```text
Dotnet-Challenge/
│
├── PetCare.API/
│   ├── Controllers/
│   ├── HealthChecks/
│   ├── Middlewares/
│   ├── Metrics/
│   ├── appsettings.json
│   ├── appsettings.Development.json
│   └── Program.cs
│
├── PetCare.Application/
│   ├── DTOs/
│   ├── Interfaces/
│   ├── Mappings/
│   └── Services/
│
├── PetCare.Domain/
│   └── Entities/
│
├── PetCare.Infrastructure/
│   ├── Data/
│   └── Repositories/
│
├── PetCare.Tests.Unit/
│
├── PetCare.Tests.Integration/
│
├── Challenge.sln
├── Dockerfile
└── README.md
```

### Responsabilidades das camadas

**PetCare.API**

Responsável pelos endpoints HTTP, controllers, autenticação, autorização, middlewares, Swagger, health checks e configuração da aplicação.

**PetCare.Application**

Contém os serviços de aplicação, interfaces, DTOs e mapeamentos utilizados na execução dos casos de uso.

**PetCare.Domain**

Contém as entidades e os conceitos centrais do domínio da aplicação.

**PetCare.Infrastructure**

Responsável pelo acesso ao banco de dados, contexto do Entity Framework Core e implementação dos repositórios.

**PetCare.Tests.Unit**

Contém testes unitários para verificar comportamentos isolados das partes testadas.

**PetCare.Tests.Integration**

Contém testes de integração para verificar o comportamento da API e a interação entre seus componentes.

### Fluxo de execução

```text
Cliente HTTP
    |
    v
PetCare.API - Controllers
    |
    v
PetCare.Application - Services
    |
    v
Interfaces de repositório
    |
    v
PetCare.Infrastructure
    |
    v
Entity Framework Core
    |
    v
Oracle Database
```

Os controllers recebem as requisições HTTP, os serviços executam as operações de aplicação e os repositórios fazem o acesso aos dados.

## 📦 Funcionalidades

- Gerenciamento de pets.
- Gerenciamento de tutores.
- Gerenciamento de vacinas.
- Registro e gerenciamento de aplicações de vacina.
- Gerenciamento de clínicas veterinárias.
- Gerenciamento de consultas.
- Gerenciamento do histórico de saúde dos pets.
- Emissão de token JWT para testes.
- Documentação interativa com Swagger.
- Registro de logs com Serilog.
- Identificação de requisições por Correlation ID.
- Tratamento centralizado de exceções.
- Health checks.
- Instrumentação de traces e métricas.
- Testes unitários e de integração.

## 🚀 Como executar o projeto

### 1. Pré-requisitos

Antes de começar, instale:

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- Git
- Acesso a uma instância Oracle compatível com a configuração do projeto
- Um editor ou IDE compatível com .NET, como Visual Studio ou Visual Studio Code

Verifique a instalação do .NET:

```powershell
dotnet --version
```

O SDK instalado deve ser compatível com o target framework `net9.0` dos projetos.

### 2. Clonar o repositório

```powershell
git clone https://github.com/Challenge-CLYVO/Dotnet-Challenge.git
cd Dotnet-Challenge
```

### 3. Restaurar as dependências

Na raiz da solução, execute:

```powershell
dotnet restore .\Challenge.sln
```

### 4. Configurar o banco Oracle

A aplicação utiliza a connection string denominada `RecommendaContextOracle`.

Configure a conexão por variável de ambiente, evitando armazenar credenciais reais no repositório.

No PowerShell, substitua os valores de exemplo pelas credenciais e pelo endereço fornecidos para o seu ambiente:

```powershell
$env:ConnectionStrings__RecommendaContextOracle = "Data Source=oracle.fiap.com.br:1521/orcl;User ID=SEU_USUARIO;Password=SUA_SENHA;"
```

A configuração acima vale para a sessão atual do PowerShell. Configure novamente a variável em uma nova sessão ou utilize o mecanismo de configuração de segredos apropriado ao seu ambiente.

**Importante:** não publique usuário, senha, chaves de autenticação ou outros segredos no GitHub. O endereço do banco também pode precisar ser ajustado conforme o ambiente utilizado.

### 5. Configurar o JWT

A aplicação valida os seguintes parâmetros:

- `Jwt:Key`
- `Jwt:Issuer`
- `Jwt:Audience`

A chave precisa ter pelo menos 32 bytes, conforme a validação implementada no `Program.cs`.

Configure os parâmetros no ambiente de execução:

```powershell
$env:Jwt__Key = "SUBSTITUA_POR_UMA_CHAVE_SEGURA_DE_PELO_MENOS_32_BYTES"
$env:Jwt__Issuer = "PetCare.API"
$env:Jwt__Audience = "PetCare.Client"
```

Use uma chave aleatória e exclusiva para cada ambiente. Não reutilize uma chave publicada em um repositório ou compartilhada em documentação.

### 6. Compilar a solução

```powershell
dotnet build .\Challenge.sln
```

### 7. Executar a API

```powershell
dotnet run --project .\PetCare.API\PetCare.API.csproj --urls http://localhost:5100
```

Com a aplicação em execução, os endereços locais configurados são:

| Recurso | Endereço |
|---|---|
| Swagger UI | http://localhost:5100/swagger |
| Health check | http://localhost:5100/health |
| Métricas Prometheus | http://localhost:5100/metrics |

Esses endereços pressupõem que a aplicação esteja em execução na porta `5100`.

### 8. Configuração do banco e migrações

A API utiliza Entity Framework Core com Oracle. Antes de executar operações que dependam do banco, verifique se:

- A instância Oracle está acessível.
- A connection string corresponde ao ambiente.
- O usuário possui as permissões necessárias.
- O esquema do banco está compatível com o modelo da aplicação.

Se o ambiente precisar de migrações, confira primeiro as migrações existentes e siga o procedimento adotado pelo projeto para aplicar as alterações. Não execute comandos de exclusão do banco como parte da instalação normal.

## 📡 Endpoints da API

A API utiliza rotas no padrão `/api/[controller]`.

### Autenticação

| Método | Endpoint | Descrição |
|---|---|---|
| POST | `/api/Auth/login` | Emite um token JWT para testes |

Exemplo de resposta:

```json
{
  "token": "SEU_TOKEN_JWT"
}
```

**Limitação da implementação atual:** o método `Login()` não recebe usuário ou senha e emite um token com a identidade fixa `Teste` e a função `User`. Portanto, esse endpoint não deve ser considerado um mecanismo de autenticação real por credenciais.

### Pets

O controller de pets exige autenticação por meio do atributo `[Authorize]`.

| Método | Endpoint | Descrição |
|---|---|---|
| GET | `/api/Pet` | Lista os pets |
| GET | `/api/Pet/{id}` | Consulta um pet pelo ID |
| POST | `/api/Pet` | Cadastra um pet |
| PUT | `/api/Pet/{id}` | Atualiza um pet |
| DELETE | `/api/Pet/{id}` | Exclui um pet |

Exemplo de criação:

```json
{
  "nome": "Rex",
  "idade": 5,
  "especie": "Cachorro",
  "raca": "Labrador",
  "idTutor": 1
}
```

O nome e a espécie são obrigatórios. A idade aceita valores entre 0 e 50, a raça é opcional e o identificador do tutor deve ser maior que zero.

### Tutores

| Método | Endpoint | Descrição |
|---|---|---|
| GET | `/api/Tutor` | Lista os tutores |
| GET | `/api/Tutor/{id}` | Consulta um tutor |
| POST | `/api/Tutor` | Cadastra um tutor |
| PUT | `/api/Tutor/{id}` | Atualiza um tutor |
| DELETE | `/api/Tutor/{id}` | Exclui um tutor |

Exemplo de criação:

```json
{
  "nome": "Maria Silva",
  "telefone": "11987654321",
  "email": "maria@example.com"
}
```

Nome, telefone e e-mail são obrigatórios. Os campos possuem validações de formato e de tamanho.

### Vacinas

| Método | Endpoint | Descrição |
|---|---|---|
| GET | `/api/Vacina` | Lista as vacinas |
| GET | `/api/Vacina/{id}` | Consulta uma vacina |
| POST | `/api/Vacina` | Cadastra uma vacina |
| PUT | `/api/Vacina/{id}` | Atualiza uma vacina |
| DELETE | `/api/Vacina/{id}` | Exclui uma vacina |

Exemplo de criação:

```json
{
  "nome": "Antirrábica",
  "descricao": "Vacina contra a raiva"
}
```

O nome é obrigatório e a descrição é opcional.

### Aplicações de vacina

| Método | Endpoint | Descrição |
|---|---|---|
| GET | `/api/AplicacaoVacina` | Lista as aplicações |
| GET | `/api/AplicacaoVacina/{id}` | Consulta uma aplicação |
| POST | `/api/AplicacaoVacina` | Registra uma aplicação |
| PUT | `/api/AplicacaoVacina/{id}` | Atualiza uma aplicação |
| DELETE | `/api/AplicacaoVacina/{id}` | Exclui uma aplicação |

Exemplo de criação:

```json
{
  "dataAplicacao": "2026-10-16T10:00:00",
  "idVacina": 1,
  "idPet": 1
}
```

O registro relaciona uma vacina a um pet e informa a data da aplicação. Os identificadores devem ser maiores que zero.

### Clínicas

| Método | Endpoint | Descrição |
|---|---|---|
| GET | `/api/Clinica` | Lista as clínicas |
| GET | `/api/Clinica/{id}` | Consulta uma clínica |
| POST | `/api/Clinica` | Cadastra uma clínica |
| PUT | `/api/Clinica/{id}` | Atualiza uma clínica |
| DELETE | `/api/Clinica/{id}` | Exclui uma clínica |

Exemplo de criação:

```json
{
  "nome": "Clínica Pet Saúde",
  "endereco": "Rua das Flores, 100",
  "telefone": "1133334444"
}
```

Os três campos são obrigatórios. O nome aceita até 100 caracteres, o endereço até 200 e o telefone até 20.

### Consultas

| Método | Endpoint | Descrição |
|---|---|---|
| GET | `/api/Consulta` | Lista as consultas |
| GET | `/api/Consulta/{id}` | Consulta um registro |
| POST | `/api/Consulta` | Cadastra uma consulta |
| PUT | `/api/Consulta/{id}` | Atualiza uma consulta |
| DELETE | `/api/Consulta/{id}` | Exclui uma consulta |

Exemplo de criação:

```json
{
  "dataConsulta": "2026-10-20T14:30:00",
  "descricao": "Consulta de rotina",
  "idPet": 1,
  "idClinica": 1
}
```

A data, o identificador do pet e o identificador da clínica são obrigatórios. A descrição é opcional e aceita até 200 caracteres.

### Histórico de saúde

| Método | Endpoint | Descrição |
|---|---|---|
| GET | `/api/HistoricoSaude` | Lista os registros |
| GET | `/api/HistoricoSaude/{id}` | Consulta um registro |
| POST | `/api/HistoricoSaude` | Cadastra um registro |
| PUT | `/api/HistoricoSaude/{id}` | Atualiza um registro |
| DELETE | `/api/HistoricoSaude/{id}` | Exclui um registro |

Exemplo de criação:

```json
{
  "descricao": "Retorno após tratamento",
  "dataRegistro": "2026-10-16T10:00:00",
  "idPet": 1
}
```

A descrição, a data do registro e o identificador do pet são obrigatórios. A descrição aceita até 255 caracteres.

### Autorização das requisições

Para endpoints protegidos, envie o token no cabeçalho HTTP:

```http
Authorization: Bearer SEU_TOKEN_JWT
```

No Swagger, utilize o recurso **Authorize** para informar o token.

A aplicação valida assinatura, emissor, audiência e validade do JWT. O código apresentado marca explicitamente o `PetController` com `[Authorize]`; não se deve presumir que todos os outros controllers estejam protegidos sem verificar a configuração de autorização correspondente.

## 📋 Códigos de resposta HTTP

Os controllers utilizam os seguintes retornos nos fluxos de sucesso:

| Código | Significado |
|---|---|
| `200 OK` | Consulta realizada com resposta de sucesso |
| `201 Created` | Recurso criado |
| `204 No Content` | Atualização ou exclusão concluída sem conteúdo na resposta |
| `500 Internal Server Error` | O endpoint de login retorna esse código quando a chave JWT não está configurada |

As respostas para erros de validação, recursos inexistentes e falhas de autorização dependem do comportamento dos serviços, dos middlewares e da configuração da aplicação.

## ❤️ Health Checks

A aplicação disponibiliza o endpoint:

```http
GET /health
```

A resposta é produzida em formato JSON e apresenta o status geral, o status de cada verificação e a duração total.

As verificações configuradas no `Program.cs` incluem:

- **API:** verificação interna da aplicação.
- **Oracle:** verificação da conexão com o contexto de banco de dados.
- **External service:** verificação de um serviço externo configurado.

O serviço externo utiliza a configuração `HealthChecks:ExternalServiceUrl`, cujo valor de desenvolvimento apresentado é `https://www.google.com`.

O health check ajuda a identificar problemas na disponibilidade da aplicação e de suas dependências.

## 📝 Logs e rastreabilidade

A aplicação utiliza Serilog para registrar eventos e informações sobre as requisições.

Os logs são enviados para:

- Console.
- Arquivos diários no diretório `logs/`, com o padrão `petcare-.log`.

A configuração mantém até sete arquivos de log, conforme os parâmetros definidos no `Program.cs`.

### Correlation ID

O middleware `CorrelationIdMiddleware` é utilizado para auxiliar a rastreabilidade das requisições. O identificador permite relacionar eventos registrados durante o processamento de uma chamada.

### Tratamento de exceções

O middleware `ExceptionMiddleware` centraliza o tratamento de exceções não tratadas durante o processamento HTTP.

### Logging HTTP

O Serilog utiliza `UseSerilogRequestLogging()` para registrar informações sobre as requisições processadas pela aplicação.

## 📊 Observabilidade e métricas

A aplicação utiliza OpenTelemetry para instrumentação de traces e métricas.

A configuração inclui instrumentação para:

- Requisições ASP.NET Core.
- Chamadas HTTP por `HttpClient`.
- Operações do Entity Framework Core.
- Métricas de runtime do .NET.

O endpoint de métricas é disponibilizado por:

```http
GET /metrics
```

As métricas são expostas em formato compatível com o scraping do Prometheus.

O código também registra o contador `petcare_http_errors_total`, destinado à contagem de erros HTTP. A atualização desse contador depende da instrumentação implementada no código de métricas.

A configuração de tracing inclui exportação para o console, facilitando a observação durante o desenvolvimento.

## 🧪 Testes automatizados

A solução contém projetos de testes unitários e de integração.

### Executar todos os testes da solução

Na raiz do repositório:

```powershell
dotnet test .\Challenge.sln
```

### Executar os testes unitários

```powershell
dotnet test .\PetCare.Tests.Unit\PetCare.Tests.Unit.csproj
```

### Executar os testes de integração

```powershell
dotnet test .\PetCare.Tests.Integration\PetCare.Tests.Integration.csproj
```

### Resultado da validação da Sprint 4

Na última execução validada:

| Categoria | Aprovados | Falhas |
|---|---:|---:|
| Testes unitários | 60 | 0 |
| Testes de integração | 11 | 0 |
| **Total** | **71** | **0** |

A compilação da solução também foi concluída com zero avisos e zero erros nessa validação.

Os resultados acima correspondem à execução registrada durante o desenvolvimento da Sprint 4. Ao modificar o código ou o ambiente, execute novamente os testes para confirmar o estado atual.


## 🔗 Repositório

Link: https://github.com/Challenge-CLYVO/Dotnet-Challenge
