FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SignalR.Server/SignalR.Server.csproj", "SignalR.Server/"]
RUN dotnet restore "SignalR.Server/SignalR.Server.csproj"
COPY . .
WORKDIR "/src/SignalR.Server"
RUN dotnet build "SignalR.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SignalR.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SignalR.Server.dll", "--urls", "http://0.0.0.0:8080"]