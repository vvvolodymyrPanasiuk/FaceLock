#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

COPY ["scripts/token-db-init.sql", "scripts/"]

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["WebAPI/FaceLock.WebAPI/FaceLock.WebAPI.csproj", "WebAPI/FaceLock.WebAPI/"]
COPY ["DAL/FaceLock.EF/FaceLock.EF.csproj", "DAL/FaceLock.EF/"]
COPY ["Domain/FaceLock.Domain/FaceLock.Domain.csproj", "Domain/FaceLock.Domain/"]
COPY ["Services/FaceLock.Authentication/FaceLock.Authentication.csproj", "Services/FaceLock.Authentication/"]
COPY ["Services/FaceLock.DataManagement/FaceLock.DataManagement.csproj", "Services/FaceLock.DataManagement/"]
RUN dotnet restore "WebAPI/FaceLock.WebAPI/FaceLock.WebAPI.csproj"
COPY . .
WORKDIR "/src/WebAPI/FaceLock.WebAPI"
RUN dotnet build "FaceLock.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FaceLock.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FaceLock.WebAPI.dll"]