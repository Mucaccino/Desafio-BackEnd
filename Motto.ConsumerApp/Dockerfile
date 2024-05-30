FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy everything
COPY Motto.ConsumerApp/ ./Motto.ConsumerApp
COPY Motto.Data/ ./Motto.Data
COPY Motto.Utils/ ./Motto.Utils

# Restore as distinct layers
WORKDIR /app/Motto.ConsumerApp
RUN dotnet restore

# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/Motto.ConsumerApp/out .

ENTRYPOINT ["dotnet", "Motto.ConsumerApp.dll"]