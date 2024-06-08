# Variables
DOCKER_COMPOSE = docker-compose
DOTNET = dotnet
DOCKFX = docfx
NSWAG = nswag
TESTS_PROJECT = Motto.Tests
EF_PROJECT = Motto.Data
Command := $(firstword $(MAKECMDGOALS))
Arguments := $(wordlist 2,$(words $(MAKECMDGOALS)),$(MAKECMDGOALS))

# Targets
.PHONY: all build setup services projects docs

# Install dependencies, up services and setup database
setup: dependencies services update

# Update database
update:
	$(DOTNET) ef database update --project $(EF_PROJECT)

# Up only services
services:
	$(DOCKER_COMPOSE) up -d --build

# Up projects without services
projects:
	$(DOCKER_COMPOSE) -f docker-compose.yml -f docker-compose.projects.yml up -d --build

# Up services and projects
complete: services projects

# Down containers
down:
	$(DOCKER_COMPOSE) -f docker-compose.yml -f docker-compose.projects.yml down

# Run all tests
tests: services
	$(DOTNET) test ./$(TESTS_PROJECT)/$(TESTS_PROJECT).csproj

# Run dotnet clean
clean: 
	$(DOTNET) clean

# Run docfx
docs:
	$(NSWAG) aspnetcore2openapi /project:Motto.WebApi/Motto.WebApi.csproj /nobuild:false /output:restapi/swagger.json
	$(NSWAG) openapi2tsclient /input:restapi/swagger.json /template:Axios /clientBaseClass:ClientBase /extensionCode:ClientBase.ts /useTransformOptionsMethod:true /output:restapi/clients.ts 
	$(NSWAG) openapi2csclient /input:restapi/swagger.json /classname:MottoServiceClient /namespace:Motto /output:restapi/clients.cs
# make docs serve
ifeq (serve, $(filter serve,$(MAKECMDGOALS)))
	$(DOCKFX) --serve
else
	$(DOCKFX) 
endif

# Install dependencies tools
dependencies:
	$(DOTNET) tool install -g dotnet-ef
	$(DOTNET) tool install -g NSwag.ConsoleCore
	$(DOTNET) tool install -g docfx

%:
	@: