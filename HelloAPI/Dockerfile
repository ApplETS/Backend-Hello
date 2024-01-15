FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY coreAPI.csproj ./
RUN dotnet restore

COPY . .
WORKDIR /src
RUN dotnet build -c $BUILD_CONFIGURATION -o /app/build

COPY . .
WORKDIR /src
RUN dotnet build -c $BUILD_CONFIGURATION -o /app/build

FROM base AS runtime
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HelloAPI.dll"]