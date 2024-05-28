# Variables
DOCKER_COMPOSE = docker-compose
DOTNET = dotnet

# Targets
.PHONY: all build setup-db up-services

# Build, setup and up
all: setup

# Up services and setup database
setup: up-services update-db

# Update database
update-db:
	$(DOTNET) ef database update --project entities

# Up services and projects
up-all: up-services up-projects

# Up only services
up-services:
	$(DOCKER_COMPOSE) up -d --build

# Up projects without services
up-projects:
	$(DOCKER_COMPOSE) -f docker-compose.yml -f docker-compose.projects.yml up -d --build

# Down containers
down:
	$(DOCKER_COMPOSE) -f docker-compose.yml -f docker-compose.projects.yml down

# Run all tests
run-tests: up-services
	$(DOTNET) test ./tests/tests.csproj