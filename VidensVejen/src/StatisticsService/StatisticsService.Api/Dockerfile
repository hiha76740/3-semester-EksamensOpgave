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
COPY ["src/StatisticsService/StatisticsService.Api/StatisticsService.Api.csproj", "src/StatisticsService/StatisticsService.Api/"]
COPY ["src/StatisticsService/StatisticsService.FacadeLib/StatisticsService.FacadeLib.csproj", "src/StatisticsService/StatisticsService.FacadeLib/"]
COPY ["src/StatisticsService/StatisticsService.InfrastructureLib/StatisticsService.InfrastructureLib.csproj", "src/StatisticsService/StatisticsService.InfrastructureLib/"]
COPY ["src/StatisticsService/StatisticsService.DomainLib/StatisticsService.DomainLib.csproj", "src/StatisticsService/StatisticsService.DomainLib/"]
COPY ["src/Shared/VidensVejen.SharedKernelLib/VidensVejen.SharedKernelLib.csproj", "src/Shared/VidensVejen.SharedKernelLib/"]
COPY ["src/StatisticsService/StatisticsService.ApplicationLib/StatisticsService.ApplicationLib.csproj", "src/StatisticsService/StatisticsService.ApplicationLib/"]
COPY ["src/Shared/VidensVejen.ContractsLib/VidensVejen.ContractsLib.csproj", "src/Shared/VidensVejen.ContractsLib/"]
COPY ["src/Shared/VidensVejen.BuildingBlocksLib/VidensVejen.BuildingBlocksLib.csproj", "src/Shared/VidensVejen.BuildingBlocksLib/"]
COPY ["VidensVejen.ServiceDefaults/VidensVejen.ServiceDefaults.csproj", "VidensVejen.ServiceDefaults/"]
RUN dotnet restore "./src/StatisticsService/StatisticsService.Api/StatisticsService.Api.csproj"
COPY . .
WORKDIR "/src/src/StatisticsService/StatisticsService.Api"
RUN dotnet build "./StatisticsService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./StatisticsService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StatisticsService.Api.dll"]
