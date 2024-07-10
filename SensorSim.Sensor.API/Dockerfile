FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SensorSim.Sensor.API/SensorSim.Sensor.API.csproj", "SensorSim.Sensor.API/"]
COPY ["SensorSim.Domain.Migrations/SensorSim.Domain.Migrations.csproj", "SensorSim.Domain.Migrations/"]
COPY ["SensorSim.Domain/SensorSim.Domain.csproj", "SensorSim.Domain/"]
COPY ["SensorSim.Infrastructure/SensorSim.Infrastructure.csproj", "SensorSim.Infrastructure/"]
RUN dotnet restore "SensorSim.Sensor.API/SensorSim.Sensor.API.csproj"
COPY . .
WORKDIR "/src/SensorSim.Sensor.API"
RUN dotnet build "SensorSim.Sensor.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SensorSim.Sensor.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SensorSim.Sensor.API.dll"]
