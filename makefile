# Variáveis
DOCKER_COMPOSE = docker-compose
DOTNET = dotnet
ENTITIES_PROJECT_DIR = entities
POSTGRESQL_CONTAINER = postgres
RABBITMQ_CONTAINER = rabbitmq
MINIO_CONTAINER = minio

# Alvos
.PHONY: all build docker update-db

all: build

build:
	$(DOTNET) build

docker:
	$(DOCKER_COMPOSE) up

setup-postgres: up-postgres update-postgres

up-postgres:
	$(DOCKER_COMPOSE) up -d $(POSTGRESQL_CONTAINER)

update-postgres:
	$(DOTNET) ef database update --project $(ENTITIES_PROJECT_DIR)

up-rabbitmq:
	$(DOCKER_COMPOSE) up -d $(RABBITMQ_CONTAINER)

up-minio:
	$(DOCKER_COMPOSE) up -d $(MINIO_CONTAINER)

up-services: up-postgres up-rabbitmq up-minio