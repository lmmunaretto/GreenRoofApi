# Etapa 1: Imagem base para construir a aplicação
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

# Copiar arquivos .csproj e restaurar dependências
COPY *.csproj ./
RUN dotnet restore

# Copiar o restante dos arquivos do projeto e compilar a aplicação
COPY . ./
RUN dotnet publish -c Release -o out

# Etapa 2: Imagem para rodar a aplicação
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build-env /app/out .

# Variável de ambiente para a porta que o Railway vai usar
ENV ASPNETCORE_URLS=http://+:8080

# Expor a porta
EXPOSE 8080

# Comando para rodar a aplicação
ENTRYPOINT ["dotnet", "GreenRoofApi.dll"]

