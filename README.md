# Desafio backend.
Seja muito bem-vindo ao desafio backend.

## Arquitetura

A solução é integralmente desenvolvida em .NET 8, utilizando EF Core, RabbitMQ, PostgreSQL, MinIO e possui ambiente para execução em Docker.

### Estrutura da Solução

* Projeto `api`
    * Webapp com controllers e serviços relacionados.
    * Interface Swagger UI quando executado em Development.
* Projeto `models`
    * Classlib com modelos e classes de dominio.
* Projeto `entities`
    * Classlib que inclui `models` e trata a infra com EF Core.
* Projeto `utils`
    * Classlib com classes e métodos estáticos de auxilio.
* Projeto `workers`
    * Worker com consumer da fila de mensagens.
    * Inspeciona as mensagens e exibe no Logger.

### Setup do Projeto

> ___TODO__ makefile para setup do projeto_

Executar ambiente docker
```
> docker compose build
> docker compose up
```
Realizar update do migrations
```
> cd ./entities 
> dotnet tool install --global dotnet-ef
> dotnet ef database update
```

## Ambiente Docker

A estrutura do Docker cria o ambiente propício para o desenvolvimento ou produção da solução completa, com todos os serviços externos necessários, como o RabbitMQ, MinIO e PostgreSQL. 

O aplicativo API e WORKERS também estão adicionados ao docker-compose para possibilitar o ambiente stage completo rodando localmente, sem necessidade do um ambiente de desenvolvimento.

### Containers

* `postgresql`
    * executa uma imagem do PostgreSQL, usado como base de dados
* `rabbitmq`
    * executa a imagem do RabbitMQ, usada como messageria
* `minio`
    * executa a imagem do MinIO, usada como bucket de arquivos 
* `api`
    * executa o build do projeto `/api` em localhost:5000
    * para acessar o SwaggerUI entrar no endereço localhost:5000/swagger
* `workers`
    * executa o build do projeto `/workers`, para escuta de fila de mensagens

## Adicional

- O projeto contém um [TODO](TODO.md), que deve ser mantido.
- A solução ainda carece da implementação do projeto de testes.
- Necessário repassar o projeto incluindo Loggers no decorrer da execução.
- Usado para criar os certificados para HTTPS dentro do Docker:
`dotnet dev-certs https -ep %USERPROFILE%\.aspnet\https\aspnetapp.pfx -p mypass123`