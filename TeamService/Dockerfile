FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY ["TeamService/TaskManager.Team.API/TaskManager.Team.API.csproj", "TeamService/TaskManager.Team.API/"]
COPY ["TeamService/TaskManager.Team.Logic/TaskManager.Team.Logic.csproj", "TeamService/TaskManager.Team.Logic/"]
COPY ["TeamService/TaskManager.Team.Repository/TaskManager.Team.Repository.csproj", "TeamService/TaskManager.Team.Repository/"]
COPY ["Shared/TaskManager.Cache/TaskManager.Cache.csproj", "Shared/TaskManager.Cache/"]
COPY ["Shared/TaskManager.Core/TaskManager.Core.csproj", "Shared/TaskManager.Core/"]
COPY ["Shared/TaskManager.Database/TaskManager.Database.csproj", "Shared/TaskManager.Database/"]
COPY ["Shared/TaskManager.MessageBroker/TaskManager.MessageBroker.csproj", "Shared/TaskManager.MessageBroker/"]

RUN dotnet restore "TeamService/TaskManager.Team.API/TaskManager.Team.API.csproj"

COPY ["TeamService/", "TeamService/"]
COPY ["Shared/", "Shared/"]

WORKDIR "/src/TeamService/TaskManager.Team.API/"

RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TaskManager.Team.API.dll"]
