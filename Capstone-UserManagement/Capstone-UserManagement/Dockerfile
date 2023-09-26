#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["NuGet.Config", "."]
COPY ["Capstone-UserManagement/Capstone-UserManagement.csproj", "Capstone-UserManagement/"]
COPY ["Publicis.ReportHub.Framework/Publicis.ReportHub.Framework.csproj", "Publicis.ReportHub.Framework/"]
RUN dotnet restore "Capstone-UserManagement/Capstone-UserManagement.csproj"
COPY . .
WORKDIR "/src/Capstone-UserManagement"
RUN dotnet build "Capstone-UserManagement.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Capstone-UserManagement.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Capstone-UserManagement.dll"]