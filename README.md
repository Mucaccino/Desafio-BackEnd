# Desafio Back-end

Seja muito bem-vindo ao desafio backend. Seu objetivo é criar uma aplicação para gerenciar aluguel de motos e entregadores. 

## Solução

O projeto é integralmente desenvolvida em [.NET 8](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/apis?view=aspnetcore-8.0), utilizando EntityFramework e serviços adicionais como RabbitMQ, PostgreSQL, MinIO e Seq. Possui ambiente para execução de serviços ou completa em Docker e também o Swagger UI para interação com a API.

![Swagger UI rodando no Docker](print_swagger_ui.png?raw=true "Swagger UI rodando no Docker")

### Estrutura da Solução

```
sln/
├── Motto.ConsumerApp/
│   ├── Consumers/
│   ├── Program.cs
│   ├── Motto.ConsumerApp.csproj
├── Motto.Data/
│   ├── Entities/
│   ├── Enums/
│   ├── Migrations/
│   ├── Repositories/
│   ├── ApplicationDbContext.cs
│   ├── Motto.Data.csproj
│   ├── SeedData.cs
├── Motto.Domain/
│   ├── Events/
│   ├── Exceptions/
│   ├── Models/
│   ├── Services/
│   ├── Motto.Core.csproj
├── Motto.Tests/
│   ├── Motto.Tests.csproj
├── Motto.Utils/
│   ├── Motto.Utils.csproj
├── Motto.WebApi/
│   ├── Controllers/
│   ├── Dtos/
│   ├── Program.cs
│   ├── Motto.Api.csproj
```

### Setup da solução

#### Makefile

A solução inclui o __makefile__, para auxiliar em comandos de compulação e subida de serviços e dependências e dos próprios projetos em si. 

```
> make setup
```

Outros comandos podem ser encontrados no [__makefile__](makefile).

#### Docker-compose

Executar ambiente de dependências
```
> docker-compose up --build
```

Executar ambiente com dependências e projetos
```
> docker-compose -f docker-compose.yml -f docker-compose.projects.yml up --build
```

#### EntityFramework

Realizar update do migrations
```
> cd ./entities 
> dotnet ef database update
```

## Docker

A estrutura do Docker cria o ambiente propício para o desenvolvimento ou produção da solução, com todos os serviços externos necessários, como o RabbitMQ, MinIO, Seq e PostgreSQL.  Os aplicativos `api` e `woekers` estão adicionados ao [docker-compose.projects.yml](docker-compose.projects.yml) para permitir execução completa do ambiente.

### Containers

#### Dependências

* `postgresql`: base de dados
* `rabbitmq`: messageria
* `minio`: bucket de arquivos 
* `seq`: monitoramento de logs 

#### Aplicativos

* `api`: webapi
* `workers`: consumer da messageria

## Rodando

A API é preparada como um webapp do dotnet, possui implementação do JWT como autenticação, e possue o Swaager UI configurado no ambiente de desenvolvimento.

A solução do aplicativo possui dois tipos de usuários - admin (`Admin`) e entregador (`DeliveryDriver`) - e cada end-point criado possui sua devida autorização.

### Execução e testes

#### Makefile

O [Makefile](makefile) está configurado com as principais tarefas para a execução da solução.

#### API com Swagger UI

- Realize o __up__ dos containers e __migrations__ do EF Core (pode usar o make do makefile)
- Vá até o endereço do Swagger UI em [http://localhost:5000](http://localhost:5000)
- Execute o end-point [api/Auth/login](http://localhost:5000/swagger/index.html#/Auth/Auth_AuthenticateUser)
    - para logar como administrador use `{ username: admin, password: 123mudar }`
    - para logar como entregador use `{ username: entregador, password: 123mudar }`
- Recupere o valor da propriedade token, retornada pelo `api/Auth/login`, clique no botão `Authorize` da interface do Swagger UI e adicione o valor do token para autenticação (Ex.: Bearer 213das9bn7h21...).
- Com o Swagger UI autenticado, os end-points criado baseado nas especificações do desafio podem ser devidamente utilizadas 

#### Consumer no console

O aplicativo `workers` exibe no console (ILogger) o consumo da fila de mensagens do RabbitMQ.

#### Projeto de testes

O projeto `./tests` com testes de integração de API e testes unitários.

- `ApiIntegrationTests`: executa o programa ./api e teste conexão e login em base de dados real para testar funcionamento da API.
- `[X]ControllerTests`: executa os testes unitários relacionado ao controller usando dados do Mock. 
- ...

```
> make run-tests
```

## Adicional

### Github Actions

O projeto está configurado com um workflow de execução principal no [Github Action](https://github.com/Mucaccino/Desafio-BackEnd/actions), preparado para compilar os projetos, subir os containers, realizar o migration da base de dados e executar todos os testes.

```
> .github/workflows/main.yml
```

Link do [primeiro run completo com sucesso](https://github.com/Mucaccino/Desafio-BackEnd/actions/runs/9165020101).

### Anotações

- O projeto contém um [TODO](TODO.md), que deve ser mantido.
- Usado para criar os certificados para HTTPS dentro do Docker:
`dotnet dev-certs https -ep %USERPROFILE%\.aspnet\https\aspnetapp.pfx -p mypass123`
- https://dotnet.github.io/docfx/index.html

#### End-points

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