#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["RDNS.Api/RDNS.Api.csproj", "RDNS.Api/"]
RUN dotnet restore "RDNS.Api/RDNS.Api.csproj"
COPY . .
WORKDIR "/src/RDNS.Api"
RUN dotnet build "RDNS.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RDNS.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RDNS.Api.dll"]