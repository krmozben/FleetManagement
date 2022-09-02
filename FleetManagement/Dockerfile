#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["FleetManagement/FleetManagement.csproj", "FleetManagement/"]
COPY ["FleetManagement.Application/FleetManagement.Application.csproj", "FleetManagement.Application/"]
COPY ["FleetManagement.Domain/FleetManagement.Domain.csproj", "FleetManagement.Domain/"]
COPY ["FleetManagement.DomainCore/FleetManagement.DomainCore.csproj", "FleetManagement.DomainCore/"]
COPY ["FleetManagement.Infrastructure/FleetManagement.Infrastructure.csproj", "FleetManagement.Infrastructure/"]
RUN dotnet restore "FleetManagement/FleetManagement.csproj"
COPY . .
WORKDIR "/src/FleetManagement"
RUN dotnet build "FleetManagement.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FleetManagement.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FleetManagement.dll"]