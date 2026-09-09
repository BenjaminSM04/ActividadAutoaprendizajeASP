# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["VaultContactos.csproj", "./"]
RUN dotnet restore "VaultContactos.csproj"

COPY . .
RUN dotnet publish "VaultContactos.csproj" -c Release -o /app/publish --no-restore /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

COPY --from=build /app/publish .
RUN mkdir -p /app/data

ENTRYPOINT ["dotnet", "VaultContactos.dll"]
