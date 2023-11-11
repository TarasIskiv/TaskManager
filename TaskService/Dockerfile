FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine AS base
WORKDIR /app
RUN apk add --no-cache icu-libs krb5-libs libgcc libintl libssl1.1 libstdc++ zlib
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS build
WORKDIR /src

COPY ["TaskService/TaskManager.Task.API/TaskManager.Task.API.csproj", "TaskService/TaskManager.Task.API/"]
COPY ["TaskService/TaskManager.Task.Logic/TaskManager.Task.Logic.csproj", "TaskService/TaskManager.Task.Logic/"]
COPY ["TaskService/TaskManager.Task.Repository/TaskManager.Task.Repository.csproj", "TaskService/TaskManager.Task.Repository/"]
COPY ["Shared/TaskManager.Cache/TaskManager.Cache.csproj", "Shared/TaskManager.Cache/"]
COPY ["Shared/TaskManager.Core/TaskManager.Core.csproj", "Shared/TaskManager.Core/"]
COPY ["Shared/TaskManager.Database/TaskManager.Database.csproj", "Shared/TaskManager.Database/"]
COPY ["Shared/TaskManager.MessageBroker/TaskManager.MessageBroker.csproj", "Shared/TaskManager.MessageBroker/"]

RUN dotnet restore "TaskService/TaskManager.Task.API/TaskManager.Task.API.csproj"

COPY ["TaskService/", "TaskService/"]
COPY ["Shared/", "Shared/"]

WORKDIR "/src/TaskService/TaskManager.Task.API/"

RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TaskManager.Task.API.dll"]
