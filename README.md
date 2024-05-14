# Desafio backend.

Seja muito bem-vindo ao desafio backend. Seu objetivo é criar uma aplicação para gerenciar aluguel de motos e entregadores. Quando um entregador estiver registrado e com uma locação ativa poderá também efetuar entregas de pedidos disponíveis na plataforma.

## Arquitetura

A solução é integralmente desenvolvida em .NET 8, utilizando EF Core, RabbitMQ, PostgreSQL, MinIO e possui ambiente para execução em Docker.

### Estrutura da Solução

* Projeto `api`
    * Webapp com controllers e serviços relacionados.
    * Interface Swagger UI quando executado em Development.
* Projeto `models`
    * Classlib com modelos e classes de dominio.
* Projeto `entities`
    * Classlib de infraestrutura que configura os `models` com EF Core.
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

## Execução e API

A API é preparada como um webapp do dotnet, possui implementação do JWT como autenticação, e possue o Swaager UI configurado no ambiente de desenvolvimento.

A solução do aplicativo possui dois tipos de usuários - admin (`Admin`) e entregador (`DeliveryDriver`) - e cada end-point criado possui sua devida autorização.

### Controllers e End-points

- `Motorcycle`
    - Responsável pelo cadastro, listagem e consulta de motos
- `Rental`
    - Responsável pela criação, listagem e consulta de aluguel, entrega e consulta do valor total
- `RentalPlan`
    - Responsável por listar os planos disponíveis
- `Auth`
    - Reponsável pelo login e verificação da autenticacao
- `LicenseImage`
    - Responsável para receber e recuparar imagem vinculada ao entregador
- `User`
    - Responsável pelo cadastro de usuários

### Para executar e testar

#### API com Swagger UI

- Realize o __up__ dos containers e __migrations__ do EF Core
- Vá até o endereço do Swagger UI em [http://localhost:5000](http://localhost:5000)
- Execute o end-point [api/Auth/login](http://localhost:5000/swagger/index.html#/Auth/Auth_AuthenticateUser)
    - para logar como administrador use `{ username: admin, password: 123mudar }`
    - para logar como entregador use `{ username: entregador, password: 123mudar }`
- Recupere o valor da propriedade token, retornada pelo `api/Auth/login`, clique no botão `Authorize` da interface do Swagger UI e adicione o valor do token para autenticação (Ex.: Bearer 213das9bn7h21...).
- Pode verificar as autorizações de cada um dos tipos de usuários em `api/Auth/verify/admin` ou `api/Auth/verify/deliveryDriver`
- Com o Swagger UI autenticado, os end-points criado baseado nas especificações do desafio podem ser devidamente utilizadas 

## Adicional

- O projeto contém um [TODO](TODO.md), que deve ser mantido.
- Passo a passo para execução do projeto.
- A solução ainda carece da implementação do projeto de testes.
- Necessário repassar o projeto incluindo Loggers no decorrer da execução.
- Usado para criar os certificados para HTTPS dentro do Docker:
`dotnet dev-certs https -ep %USERPROFILE%\.aspnet\https\aspnetapp.pfx -p mypass123`