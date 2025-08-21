FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY *.sln ./
COPY TaskManagement.Domain/*.csproj ./TaskManagement.Domain/
COPY TaskManagement.Application/*.csproj ./TaskManagement.Application/
COPY TaskManagement.Infrastructure/*.csproj ./TaskManagement.Infrastructure/
COPY TaskManagement.API/*.csproj ./TaskManagement.API/
COPY TaskManagement.Tests/*.csproj ./TaskManagement.Tests/

RUN dotnet nuget locals all --clear
RUN dotnet restore --no-cache --force

COPY TaskManagement.Domain/ ./TaskManagement.Domain/
COPY TaskManagement.Application/ ./TaskManagement.Application/
COPY TaskManagement.Infrastructure/ ./TaskManagement.Infrastructure/
COPY TaskManagement.API/ ./TaskManagement.API/
COPY TaskManagement.Tests/ ./TaskManagement.Tests/

WORKDIR /app/TaskManagement.API
RUN dotnet build -c Release --no-restore --verbosity normal

RUN dotnet publish -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish .

RUN groupadd -r dotnet && useradd -r -g dotnet dotnet
RUN chown -R dotnet:dotnet /app
USER dotnet

EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

HEALTHCHECK --interval=30s --timeout=10s --start-period=60s --retries=3 \
    CMD curl -f http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "TaskManagement.API.dll"]