# Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY GiroDaCopa.sln ./
COPY src/GiroDaCopa.Api/GiroDaCopa.Api.csproj src/GiroDaCopa.Api/
COPY src/GiroDaCopa.Application/GiroDaCopa.Application.csproj src/GiroDaCopa.Application/
COPY src/GiroDaCopa.Domain/GiroDaCopa.Domain.csproj src/GiroDaCopa.Domain/
COPY src/GiroDaCopa.Infrastructure/GiroDaCopa.Infrastructure.csproj src/GiroDaCopa.Infrastructure/
COPY src/GiroDaCopa.Persistence/GiroDaCopa.Persistence.csproj src/GiroDaCopa.Persistence/

RUN dotnet restore src/GiroDaCopa.Api/GiroDaCopa.Api.csproj

COPY src/ src/

RUN dotnet publish src/GiroDaCopa.Api/GiroDaCopa.Api.csproj \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT=Production

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "GiroDaCopa.Api.dll"]
