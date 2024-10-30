# Usar a imagem base do SDK .NET 7
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

# Definir o diretório de trabalho dentro do container
WORKDIR /app

# Copiar o arquivo de projeto e restaurar as dependências
COPY GreenRoofApi.csproj ./
RUN dotnet restore

# Copiar todos os outros arquivos do projeto
COPY . .

# Compilar o projeto
RUN dotnet publish -c Release -o /app/out

# Usar a imagem base do runtime .NET 7
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime

# Definir o diretório de trabalho
WORKDIR /app

# Copiar os arquivos publicados para o diretório de trabalho
COPY --from=build /app/out ./

# Definir o comando para iniciar a aplicação
ENTRYPOINT ["dotnet", "GreenRoofApi.dll"]