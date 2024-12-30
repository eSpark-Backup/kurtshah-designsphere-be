#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Identity/designsphere_identity.STS.Identity/designsphere_identity.STS.Identity.csproj", "Identity/designsphere_identity.STS.Identity/"]
COPY ["Identity/designsphere_identity.Admin.EntityFramework.Shared/designsphere_identity.Admin.EntityFramework.Shared.csproj", "Identity/designsphere_identity.Admin.EntityFramework.Shared/"]
RUN dotnet restore "./Identity/designsphere_identity.STS.Identity/designsphere_identity.STS.Identity.csproj"
COPY . .
WORKDIR "/src/Identity/designsphere_identity.STS.Identity"
RUN dotnet build "./designsphere_identity.STS.Identity.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./designsphere_identity.STS.Identity.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_FORWARDEDHEADERS_ENABLED=true
ENTRYPOINT ["dotnet", "designsphere_identity.STS.Identity.dll"]