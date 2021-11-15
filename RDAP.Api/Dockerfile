#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 5002

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["RDAP.Api/RDAP.Api.csproj", "RDAP.Api/"]
RUN dotnet restore "RDAP.Api/RDAP.Api.csproj"
COPY . .
WORKDIR "/src/RDAP.Api"
RUN dotnet build "RDAP.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RDAP.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RDAP.Api.dll"]