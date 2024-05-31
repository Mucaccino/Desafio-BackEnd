# Variables
DOCKER_COMPOSE = docker-compose
DOTNET = dotnet
TESTS_PROJECT = Motto.Tests
EF_PROJECT = Motto.Data

# Targets
.PHONY: all build setup-db up-services

# Build, setup and up
all: setup

# Up services and setup database
setup: services update

# Update database
update:
	$(DOTNET) ef database update --project $(EF_PROJECT)

# Up services and projects
all: services projects

# Up only services
services:
	$(DOCKER_COMPOSE) up -d --build

# Up projects without services
projects:
	$(DOCKER_COMPOSE) -f docker-compose.yml -f docker-compose.projects.yml up -d --build

# Down containers
down:
	$(DOCKER_COMPOSE) -f docker-compose.yml -f docker-compose.projects.yml down

# Run all tests
tests: services
	$(DOTNET) test ./$(TESTS_PROJECT)/$(TESTS_PROJECT).csproj