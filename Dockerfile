# ==========================================
# ETAPA 1 - BUILD
# ==========================================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src

# Copia os arquivos .csproj primeiro
# para aproveitar o cache do Docker
COPY PetCare.Domain/PetCare.Domain.csproj PetCare.Domain/
COPY PetCare.Application/PetCare.Application.csproj PetCare.Application/
COPY PetCare.Infrastructure/PetCare.Infrastructure.csproj PetCare.Infrastructure/
COPY PetCare.API/PetCare.API.csproj PetCare.API/

# Restaura as dependências da API
RUN dotnet restore PetCare.API/PetCare.API.csproj

# Copia o restante do código
COPY PetCare.Domain/ PetCare.Domain/
COPY PetCare.Application/ PetCare.Application/
COPY PetCare.Infrastructure/ PetCare.Infrastructure/
COPY PetCare.API/ PetCare.API/

# Publica a aplicação
RUN dotnet publish PetCare.API/PetCare.API.csproj \
    -c Release \
    -o /app/publish \
    --no-restore


# ==========================================
# ETAPA 2 - RUNTIME
# ==========================================
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

WORKDIR /app

# Copia somente os arquivos publicados
COPY --from=build /app/publish .

# A aplicação escutará na porta 8080
ENV ASPNETCORE_URLS=http://+:8080

# Ambiente
ENV ASPNETCORE_ENVIRONMENT=Production

# Expõe a porta da API
EXPOSE 8080

# Usuário não-root
USER app

# Inicializa a API
ENTRYPOINT ["dotnet", "PetCare.API.dll"]