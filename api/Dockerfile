FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy everything
COPY api/ ./api
COPY models/ ./models
COPY entities/ ./entities
COPY utils/ ./utils
COPY tests/ ./tests

# Restore as distinct layers
WORKDIR /app/api
RUN dotnet restore

# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build-env /app/api/out .

EXPOSE 5000

ENTRYPOINT ["dotnet", "api.dll"]