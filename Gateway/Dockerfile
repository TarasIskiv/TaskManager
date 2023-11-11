FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS build
WORKDIR /src

COPY ["Gateway/TaskManager.APIGateway/TaskManager.APIGateway.csproj", "Gateway/TaskManager.APIGateway/"]


RUN dotnet restore "Gateway/TaskManager.APIGateway/TaskManager.APIGateway.csproj"

COPY ["Gateway/", "Gateway/"]

WORKDIR "/src/Gateway/TaskManager.APIGateway"

RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TaskManager.APIGateway.dll"]
