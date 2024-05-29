FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy everything
COPY workers/ ./workers
COPY models/ ./models
COPY utils/ ./utils

# Restore as distinct layers
WORKDIR /app/workers
RUN dotnet restore

# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/workers/out .

ENTRYPOINT ["dotnet", "workers.dll"]