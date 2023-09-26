#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ConsumerPOC/ConsumerPOC.csproj", "ConsumerPOC/"]
RUN dotnet restore "ConsumerPOC/ConsumerPOC.csproj"
COPY . .
WORKDIR "/src/ConsumerPOC"
RUN dotnet build "ConsumerPOC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ConsumerPOC.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ConsumerPOC.dll"]