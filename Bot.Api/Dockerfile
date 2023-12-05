FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Bot.Api/Bot.Api.csproj", "Bot.Api/"]
COPY ["Bot.Application/Bot.Application.csproj", "Bot.Application/"]
COPY ["Bot.Domain/Bot.Domain.csproj", "Bot.Domain/"]
COPY ["Bot.Infrasturcture/Bot.Infrastructure.csproj", "Bot.Infrastructure/"]
RUN dotnet restore "Bot.Api/Bot.Api.csproj"
COPY . .
WORKDIR "/src/Bot.Api"
RUN dotnet build "Bot.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Bot.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bot.Api.dll"]