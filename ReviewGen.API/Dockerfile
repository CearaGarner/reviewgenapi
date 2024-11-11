FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

USER root
RUN apt-get update 
RUN apt-get --yes install curl
RUN curl --version

USER $APP_UID

WORKDIR /app

EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ReviewGen.API/ReviewGen.API.csproj", "ReviewGen.API/"]
RUN dotnet restore "ReviewGen.API/ReviewGen.API.csproj"
COPY . .
WORKDIR "/src/ReviewGen.API"
RUN dotnet build "ReviewGen.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ReviewGen.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ReviewGen.API.dll"]
