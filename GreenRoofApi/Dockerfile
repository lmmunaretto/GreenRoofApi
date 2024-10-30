# Base image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Build image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["GreenRoofApi/GreenRoofApi.csproj", "GreenRoofApi/"]
RUN dotnet restore "GreenRoofApi/GreenRoofApi.csproj"
COPY . .
WORKDIR "/src/GreenRoofApi"
RUN dotnet build "GreenRoofApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GreenRoofApi.csproj" -c Release -o /app/publish

# Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GreenRoofApi.dll"]
