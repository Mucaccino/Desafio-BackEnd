# Variáveis
DOCKER_COMPOSE = docker-compose
DOTNET = dotnet
ENTITIES_PROJECT_DIR = entities
POSTGRESQL_CONTAINER = postgres
RABBITMQ_CONTAINER = rabbitmq
MINIO_CONTAINER = minio
SEQ_CONTAINER = seq

# Alvos
.PHONY: all build docker update-db

all: build setup up

build:
	$(DOCKER_COMPOSE) build

up:
	$(DOCKER_COMPOSE) up

setup: up-postgres update-db

up-postgres:
	$(DOCKER_COMPOSE) up -d $(POSTGRESQL_CONTAINER)

update-db:
	$(DOTNET) ef database update --project $(ENTITIES_PROJECT_DIR)

up-rabbitmq:
	$(DOCKER_COMPOSE) up -d $(RABBITMQ_CONTAINER)

up-minio:
	$(DOCKER_COMPOSE) up -d $(MINIO_CONTAINER)

up-seq:
	$(DOCKER_COMPOSE) up -d $(SEQ_CONTAINER)

up-services: up-postgres up-rabbitmq up-minio up-seq