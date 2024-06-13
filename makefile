# Variables
DOCKER_COMPOSE = docker-compose
DOTNET = dotnet
DOCKFX = docfx
NSWAG = nswag
MAKE = make
TESTS_PROJECT = Motto.Tests
EF_PROJECT = Motto.Data
Command := $(firstword $(MAKECMDGOALS))
Arguments := $(wordlist 2,$(words $(MAKECMDGOALS)),$(MAKECMDGOALS))

# Targets
.PHONY: all build setup services projects docs

# Install dependencies, up services and setup database
setup: 
	$(MAKE) dependencies 
	$(MAKE) services
	$(MAKE) update

# Update database
update:
	$(DOTNET) ef database update --project $(EF_PROJECT)

# Docker
docker:
## make docker services
ifeq (services, $(filter services,$(MAKECMDGOALS)))
	$(DOCKER_COMPOSE) up -d --build
## make docker projects
else ifeq (projects, $(filter projects,$(MAKECMDGOALS)))
	$(DOCKER_COMPOSE) -f docker-compose.yml -f docker-compose.projects.yml up -d --build
## make docker down
else ifeq (down, $(filter down,$(MAKECMDGOALS)))
	$(DOCKER_COMPOSE) -f docker-compose.yml -f docker-compose.projects.yml down
## make docker > Up services and projects
else
	$(MAKE) docker services
	$(MAKE) docker projects
endif	

# Run all tests
tests: services
	$(DOTNET) test ./$(TESTS_PROJECT)/$(TESTS_PROJECT).csproj

# Run dotnet clean
clean: 
	$(DOTNET) clean

# Generate
build:
# > make generate typescript
ifeq (typescript, $(filter typescript,$(MAKECMDGOALS)))
	$(NSWAG) openapi2tsclient /input:restapi/swagger.json /clientBaseClass:ClientBase /extensionCode:ClientBase.ts /useTransformOptionsMethod:true /output:restapi/clients.ts 
# > make generate csharp
else ifeq (csharp, $(filter csharp,$(MAKECMDGOALS)))
	$(NSWAG) openapi2csclient /input:restapi/swagger.json /classname:MottoServiceClient /namespace:Motto /output:restapi/clients.cs
# > make generate swagger
else ifeq (swagger, $(filter swagger,$(MAKECMDGOALS)))
	$(NSWAG) aspnetcore2openapi /project:Motto.WebApi/Motto.WebApi.csproj /nobuild:false /output:restapi/swagger.json
# > make generate docs
else ifeq (docs, $(filter docs,$(MAKECMDGOALS)))
	$(MAKE) build swagger
	$(MAKE) build typescript
	$(MAKE) build csharp
	$(DOCKFX)
endif	

# Execute
run:
# > make run webapi
ifeq (webapi, $(filter webapi,$(MAKECMDGOALS)))
	$(DOTNET) run --project ./Motto.WebApi/Motto.WebApi.csproj
# > make run consumer
else ifeq (consumer, $(filter consumer,$(MAKECMDGOALS)))
	$(DOTNET) run --project ./Motto.ConsumerApp/Motto.ConsumerApp.csproj
# > make run docs
else ifeq (docs, $(filter docs,$(MAKECMDGOALS)))
	$(MAKE) build swagger
	$(MAKE) build typescript
	$(MAKE) build csharp
	$(DOCKFX) --serve
endif

# Install dependencies tools
dependencies:
	$(DOTNET) tool install -g dotnet-ef
	$(DOTNET) tool install -g NSwag.ConsoleCore
	$(DOTNET) tool install -g docfx

%:
	@: