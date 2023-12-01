#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.
# Base image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Build image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy csproj and restore
COPY ["Bot.Api/Bot.Api.csproj", "Bot.Api/"]
COPY ["Bot.Application/Bot.Application.csproj", "Bot.Application/"]
COPY ["Bot.Domain/Bot.Domain.csproj", "Bot.Domain/"]
COPY ["Bot.Infrastructure/Bot.Infrastructure.csproj", "Bot.Infrastructure/"]

RUN dotnet restore "Bot.Api/Bot.Api.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/Bot.Api"
RUN dotnet build "Bot.Api.csproj" -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "Bot.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage / image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bot.Api.dll"]
