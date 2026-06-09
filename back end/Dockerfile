FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Focus.AI.Api/Focus.AI.Api.csproj", "Focus.AI.Api/"]
COPY ["Focus.AI.Application/Focus.AI.Application.csproj", "Focus.AI.Application/"]
COPY ["Focus.AI.Domain/Focus.AI.Domain.csproj", "Focus.AI.Domain/"]
COPY ["Focus.AI.Infrastructure/Focus.AI.Infrastructure.csproj", "Focus.AI.Infrastructure/"]
RUN dotnet restore "./Focus.AI.Api/Focus.AI.Api.csproj"
COPY . .
WORKDIR "/src/Focus.AI.Api"
RUN dotnet build "./Focus.AI.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Focus.AI.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Focus.AI.Api.dll"]
