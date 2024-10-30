# Use a imagem base do SDK .NET 7
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env

# Defina o diretório de trabalho
WORKDIR /app

# Copie o csproj e restaure as dependências
COPY *.csproj ./
RUN dotnet restore

# Copie o restante dos arquivos e construa o projeto
COPY . ./
RUN dotnet publish -c Release -o out

# Gere a imagem final
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "GreenRoofApi.dll"]