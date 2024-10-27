# Fase de base
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

# Fase de build
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["GreenRoofApi.csproj", "./"]
RUN dotnet restore "./GreenRoofApi.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "GreenRoofApi.csproj" -c Release -o /app/build

# Fase de publish
FROM build AS publish
RUN dotnet publish "GreenRoofApi.csproj" -c Release -o /app/publish

# Fase final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GreenRoofApi.dll"]
