FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["core/api.core.csproj", "core/"]
COPY ["emails/api.emails.csproj", "emails/"]
COPY ["files/api.files.csproj", "files/"]
RUN dotnet restore "./core/api.core.csproj"

COPY . .
WORKDIR "/src/core"
RUN dotnet build "./api.core.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./api.core.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --chown=$APP_UID --from=publish /app/publish .
RUN mkdir -p /app/volume # create volume directory to store images
ENTRYPOINT ["dotnet", "api.core.dll"]