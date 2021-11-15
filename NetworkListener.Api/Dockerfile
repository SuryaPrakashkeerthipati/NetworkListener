#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["NetworkListener.Api/NetworkListener.Api.csproj", "NetworkListener.Api/"]
RUN dotnet restore "NetworkListener.Api/NetworkListener.Api.csproj"
COPY . .
WORKDIR "/src/NetworkListener.Api"
RUN dotnet build "NetworkListener.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NetworkListener.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NetworkListener.Api.dll"]