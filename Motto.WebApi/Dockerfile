FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy everything
COPY Motto.WebApi/ ./Motto.WebApi
COPY Motto.Data/ ./Motto.Data
COPY Motto.Domain/ ./Motto.Domain
COPY Motto.Utils/ ./Motto.Utils
COPY Motto.Tests/ ./Motto.Tests

# Restore as distinct layers
WORKDIR /app/Motto.WebApi
RUN dotnet restore

# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Add curl to container
RUN apt-get update && apt-get install -y curl

COPY --from=build-env /app/Motto.WebApi/out .

EXPOSE 5000

ENTRYPOINT ["dotnet", "Motto.WebApi.dll"]