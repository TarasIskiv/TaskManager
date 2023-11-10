FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS base
WORKDIR /app
COPY . .
EXPOSE 80

RUN dotnet restore TaskManager.APIGateway.csproj
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine AS publish
COPY --from=base app/publish .
ENTRYPOINT ["dotnet", "TaskManager.APIGateway.dll"]