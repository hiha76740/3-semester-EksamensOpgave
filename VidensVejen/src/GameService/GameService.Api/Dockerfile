# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base

RUN apt-get update \
    && apt-get install -y curl \
    && rm -rf /var/lib/apt/lists/*

USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/GameService/GameService.Api/GameService.Api.csproj", "src/GameService/GameService.Api/"]
COPY ["src/GameService/GameService.FacadeLib/GameService.FacadeLib.csproj", "src/GameService/GameService.FacadeLib/"]
COPY ["src/GameService/GameService.InfrastructureLib/GameService.InfrastructureLib.csproj", "src/GameService/GameService.InfrastructureLib/"]
COPY ["src/GameService/GameService.DomainLib/GameService.DomainLib.csproj", "src/GameService/GameService.DomainLib/"]
COPY ["src/Shared/VidensVejen.SharedKernelLib/VidensVejen.SharedKernelLib.csproj", "src/Shared/VidensVejen.SharedKernelLib/"]
COPY ["src/GameService/GameService.ApplicationLib/GameService.ApplicationLib.csproj", "src/GameService/GameService.ApplicationLib/"]
COPY ["src/Shared/VidensVejen.ContractsLib/VidensVejen.ContractsLib.csproj", "src/Shared/VidensVejen.ContractsLib/"]
COPY ["src/Shared/VidensVejen.BuildingBlocksLib/VidensVejen.BuildingBlocksLib.csproj", "src/Shared/VidensVejen.BuildingBlocksLib/"]
COPY ["VidensVejen.ServiceDefaults/VidensVejen.ServiceDefaults.csproj", "VidensVejen.ServiceDefaults/"]
RUN dotnet restore "./src/GameService/GameService.Api/GameService.Api.csproj"
COPY . .
WORKDIR "/src/src/GameService/GameService.Api"
RUN dotnet build "./GameService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./GameService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GameService.Api.dll"]
