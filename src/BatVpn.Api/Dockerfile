#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/BatVpn.Api/BatVpn.Api.csproj", "src/BatVpn.Api/"]
COPY ["BatVpn.Infrastructure/BatVpn.Infrastructure.csproj", "BatVpn.Infrastructure/"]
COPY ["src/BatVpn.Domain/BatVpn.Domain.csproj", "src/BatVpn.Domain/"]
COPY ["src/Batvpn.Persistence/Batvpn.Persistence.csproj", "src/Batvpn.Persistence/"]
COPY ["src/BatVpn.Application/BatVpn.Application.csproj", "src/BatVpn.Application/"]
RUN dotnet restore "src/BatVpn.Api/BatVpn.Api.csproj"
COPY . .
WORKDIR "/src/src/BatVpn.Api"
RUN dotnet build "BatVpn.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BatVpn.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BatVpn.Api.dll"]