# Variables
DOCKER_COMPOSE = docker-compose
DOTNET = dotnet
ENTITIES_PROJECT_DIR = entities
POSTGRESQL_CONTAINER = postgres
RABBITMQ_CONTAINER = rabbitmq
MINIO_CONTAINER = minio
SEQ_CONTAINER = seq

# Targets
.PHONY: all build setup-db up-services

# Build, setup and up
all: build setup-db up

# Build docker images
build:
	$(DOCKER_COMPOSE) build

# Up docker containers
up:
	$(DOCKER_COMPOSE) up

# Up postgres and setup database
setup-db: up-postgres update-db

# Up only postgres
up-postgres:
	$(DOCKER_COMPOSE) up -d $(POSTGRESQL_CONTAINER)

# Update database
update-db:
	$(DOTNET) ef database update --project $(ENTITIES_PROJECT_DIR)

# Up only rabbitmq
up-rabbitmq:
	$(DOCKER_COMPOSE) up -d $(RABBITMQ_CONTAINER)

# Up only minio
up-minio:
	$(DOCKER_COMPOSE) up -d $(MINIO_CONTAINER)

# Up only seq
up-seq:
	$(DOCKER_COMPOSE) up -d $(SEQ_CONTAINER)

# Up all services
up-services: up-postgres update-db up-rabbitmq up-minio up-seq

# Up services and run tests
tests: up-services run-tests

# Run tests
run-tests:
	$(DOTNET) test ./tests/tests.csproj