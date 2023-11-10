FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS build
WORKDIR /src

COPY ["NotificationService/TaskManager.Notification.API/TaskManager.Notification.API.csproj", "NotificationService/TaskManager.Notification.API/"]
COPY ["NotificationService/TaskManager.Notification.Logic/TaskManager.Notification.Logic.csproj", "NotificationService/TaskManager.Notification.Logic/"]
COPY ["Shared/TaskManager.Core/TaskManager.Core.csproj", "Shared/TaskManager.Core/"]
COPY ["Shared/TaskManager.MessageBroker/TaskManager.MessageBroker.csproj", "Shared/TaskManager.MessageBroker/"]

RUN dotnet restore "NotificationService/TaskManager.Notification.API/TaskManager.Notification.API.csproj"

COPY ["NotificationService/", "NotificationService/"]
COPY ["Shared/", "Shared/"]

WORKDIR "/src/NotificationService/TaskManager.Notification.API/"

RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TaskManager.Notification.API.dll"]
