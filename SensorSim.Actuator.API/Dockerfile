﻿FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SensorSim.Actuator.API/SensorSim.Actuator.API.csproj", "SensorSim.Actuator.API/"]
COPY ["SensorSim.Domain/SensorSim.Domain.csproj", "SensorSim.Domain/"]
COPY ["SensorSim.Infrastructure/SensorSim.Infrastructure.csproj", "SensorSim.Infrastructure/"]
RUN dotnet restore "SensorSim.Actuator.API/SensorSim.Actuator.API.csproj"
COPY . .
WORKDIR "/src/SensorSim.Actuator.API"
RUN dotnet build "SensorSim.Actuator.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SensorSim.Actuator.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SensorSim.Actuator.API.dll"]
