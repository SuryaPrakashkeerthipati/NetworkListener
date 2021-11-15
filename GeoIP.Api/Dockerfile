#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["GeoIP.Api/GeoIP.Api.csproj", "GeoIP.Api/"]
RUN dotnet restore "GeoIP.Api/GeoIP.Api.csproj"
COPY . .
WORKDIR "/src/GeoIP.Api"
RUN dotnet build "GeoIP.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GeoIP.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GeoIP.Api.dll"]