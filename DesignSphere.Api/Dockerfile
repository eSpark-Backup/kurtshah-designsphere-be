# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 443
EXPOSE 80


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["DesignSphere.Api/DesignSphere.Api.csproj", "DesignSphere.Api/"]
COPY ["DesignSphere.Infrastructure/DesignSphere.Infrastructure.csproj", "DesignSphere.Infrastructure/"]
COPY ["BuildingBlocks/SieveCustom/SieveCustom.csproj", "BuildingBlocks/SieveCustom/"]
COPY ["DesignSphere.Application/DesignSphere.Application.csproj", "DesignSphere.Application/"]
COPY ["BuildingBlocks/BuildingBlocks.Common/BuildingBlocks.Common.csproj", "BuildingBlocks/BuildingBlocks.Common/"]
COPY ["BuildingBlocks/EventBus.Messages/EventBus.Messages.csproj", "BuildingBlocks/EventBus.Messages/"]
COPY ["DesignSphere.Domain/DesignSphere.Domain.csproj", "DesignSphere.Domain/"]
RUN dotnet restore "./DesignSphere.Api/DesignSphere.Api.csproj"
COPY . .
WORKDIR "/src/DesignSphere.Api"
RUN dotnet build "./DesignSphere.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DesignSphere.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_FORWARDEDHEADERS_ENABLED=true
ENTRYPOINT ["dotnet", "DesignSphere.Api.dll"]