FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY core/api.core.csproj ./core/
COPY emails/api.emails.csproj ./emails/
COPY tests/api.tests.csproj ./tests/
COPY Hello.sln ./
RUN dotnet restore

COPY . .
WORKDIR /src
RUN dotnet build -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS runtime
WORKDIR /app
COPY --chown=app --from=publish /app/publish .
ENTRYPOINT ["dotnet", "api.core.dll"]