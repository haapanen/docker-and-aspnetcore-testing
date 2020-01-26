FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["BackgroundService/BackgroundService.csproj", "BackgroundService/"]
COPY ["ApplicationCore/ApplicationCore.csproj", "ApplicationCore/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
RUN dotnet restore "BackgroundService/BackgroundService.csproj"
COPY . .
WORKDIR "/src/BackgroundService"
RUN dotnet build "BackgroundService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BackgroundService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BackgroundService.dll"]
