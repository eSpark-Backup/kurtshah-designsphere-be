# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Identity/designsphere_identity.Admin.Api/designsphere_identity.Admin.Api.csproj", "Identity/designsphere_identity.Admin.Api/"]
COPY ["Identity/designsphere_identity.Admin.EntityFramework.MySql/designsphere_identity.Admin.EntityFramework.MySql.csproj", "Identity/designsphere_identity.Admin.EntityFramework.MySql/"]
COPY ["Identity/designsphere_identity.Admin.EntityFramework.Shared/designsphere_identity.Admin.EntityFramework.Shared.csproj", "Identity/designsphere_identity.Admin.EntityFramework.Shared/"]
COPY ["Identity/designsphere_identity.Admin.EntityFramework.PostgreSQL/designsphere_identity.Admin.EntityFramework.PostgreSQL.csproj", "Identity/designsphere_identity.Admin.EntityFramework.PostgreSQL/"]
COPY ["Identity/designsphere_identity.Admin.EntityFramework.SqlServer/designsphere_identity.Admin.EntityFramework.SqlServer.csproj", "Identity/designsphere_identity.Admin.EntityFramework.SqlServer/"]
COPY ["Identity/designsphere_identity.Shared/designsphere_identity.Shared.csproj", "Identity/designsphere_identity.Shared/"]
RUN dotnet restore "./Identity/designsphere_identity.Admin.Api/designsphere_identity.Admin.Api.csproj"
COPY . .
WORKDIR "/src/Identity/designsphere_identity.Admin.Api"
RUN dotnet build "./designsphere_identity.Admin.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./designsphere_identity.Admin.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "designsphere_identity.Admin.Api.dll"]