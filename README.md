# Desafio Back-end

Seja muito bem-vindo ao desafio backend. Seu objetivo é criar uma aplicação para gerenciar aluguel de motos e entregadores. 

## Solução do Projeto

O projeto é integralmente desenvolvido em [.NET 8](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/apis?view=aspnetcore-8.0), utilizando EntityFramework e serviços adicionais como RabbitMQ, PostgreSQL, MinIO e Seq. Possui ambiente para execução de serviços ou completa em Docker e também o Swagger UI para interação com a API.

![Swagger UI rodando no Docker](images/print_swagger_ui.png?raw=true "Swagger UI rodando no Docker")

## Estrutura da Solução

### `Motto.Api`

**Descrição**: WebAPI com controllers. 
**Função**: Controlar e rotear requisições HTTP, delegando a lógica de negócio para os serviços localizados em Motto.Domain.

### `Motto.Domain`

**Descrição**: Contém serviços de negócio e lógica de aplicação.
**Função**: Implementar a lógica de negócio e coordenar as operações entre o Motto.WebApi e o Motto.Data.

### `Motto.Utils`

**Descrição**: Métodos estáticos de auxílio.
**Função**: Fornecer utilitários genéricos que podem ser usados por outros projetos.

### `Motto.ConsumerApp`

**Descrição**: Aplicativo de console com consumer da mensageria RabbitMQ.
**Função**: Escutar mensagens do RabbitMQ e processá-las conforme necessário.

### `Motto.Data`

**Descrição**: Contém as entidades, repositórios e configurações do Entity Framework.
**Função**: Gerenciar o acesso aos dados e a persistência, incluindo todas as entidades do modelo de dados e suas configurações de mapeamento.

### Benefícios da Nova Estrutura:

- **Separação de Responsabilidades**: A separação clara entre lógica de negócio (Motto.Domain) e acesso a dados (Motto.Data) melhora a coesão e a modularidade.
- **Manutenibilidade**: Facilita a manutenção e a evolução do código, já que cada camada tem uma responsabilidade bem definida.
- **Testabilidade**: A separação permite que a lógica de negócio e o acesso a dados sejam testados de forma independente.
- **Reutilização**: Os serviços de negócio e os repositórios podem ser reutilizados em diferentes partes da aplicação.

### Nova Estrutura de Projeto:

```
Desafio_Back-End/
├── Motto.ConsumerApp/
│   ├── Consumers/
├── Motto.Data
│   ├── Entities/
│   ├── Migrations/
│   ├── Repositories/
│   ├── DbContext.cs
├── Motto.Domain/
│   ├── Events/
│   ├── Models/
│   ├── Services/
├── Motto.Tests/
├── Motto.Utils/
├── Motto.WebApi/
│   ├── Controllers/
│   ├── Dtos/
```

## Configurações da Solução

### Make

A solução inclui o **makefile** para auxiliar com comandos de compilação e subida de serviços e dependências e dos próprios projetos em si. 

```
# seed inicial da base de dados
> make setup

# up dos serviços dependentes
> make up-services
```

Outros comandos podem ser encontrados no [makefile](makefile).

### Docker

A solução conta com docker-compose para realizar a montagem do ambiente de serviços e dependências no Docker.


```
> docker-compose up --build
```

Adicionalmente possui o `docker-compose.projects.yml` que realiza também a montagem dos projetos no Docker (e deve ser executado junto ao `docker-compose.yml`).

```
> docker-compose -f docker-compose.yml -f docker-compose.projects.yml up --build
```

#### Dependências

* `postgresql`: utilizado como base de dados
* `rabbitmq`: utilizado como messageria
* `minio`: utilizado como bucket de arquivos 
* `seq`: utilizado para monitoramento de logs 

### EntityFramework

A estrutura de entidades e interação com base de dados é realizada com Entity Framework no projeto `Motto.Data`.

```
# update do migrations
> dotnet ef database update -p Motto.Data 
```

## Executando a Solução

O `Motto.WebApi` é uma **webapi** do dotnet e possui implementação manual do JWT para autenticação e o Swagger UI configurado no ambiente de desenvolvimento, bem como o ReDocs.

O aplicativo possui dois tipos de usuários - admin (`Admin`) e entregador (`DeliveryDriver`) - e cada end-point criado possui sua devida autorização.

^O [makefile](makefile) está configurado com as principais tarefas para a execução da solução.^

### Executanto a WebApi

- Realize o __up__ dos containers e o __migrations__ do EF Core
    ```
    > make up-services
    > make setup
    ```
- Realize o __up__ dos projetos [ou] execute-os manualmente
    ```
    > make up-projects
    ``` 
- Vá até endereço do Swagger UI em [http://localhost:5000](http://localhost:5000)
- Realize o login pelo end-point [api/Auth/login](http://localhost:5000/swagger/index.html#/Auth/Auth_AuthenticateUser)
    - `{ username: "admin", password: "123mudar" }`
    - `{ username: "entregador", password: "123mudar" }`
- Recupere o valor da propriedade token, clique no botão `Authorize` do Swagger UI e adicione o valor do token para autenticação (Ex.: Bearer 213das9bn7h21...).
- Com o Swagger UI autenticado, os end-points criados baseado nas especificações do desafio podem ser devidamente utilizadas 

### Executando o ConsumerApp

O aplicativo `Motto.ConsumerApp` exibe no console (e no **ILogger**) o consumo da fila de mensagens do RabbitMQ.

### Executando os Testes

O projeto `Motto.Tests` possui testes de integração com WebApi e testes unitários.

- `ApiIntegrationTests`: executa o programa ./api e teste conexão e login em base de dados real para testar funcionamento da API.
- `____ControllerTests`: executa os testes unitários relacionados aos controllers - usando dados do Mock. 

```
# executar os testes pelo make
> make run-tests
```

## Informações Adicionais

### Github Actions

O projeto está configurado com um workflow de execução principal no [Github Action](https://github.com/Mucaccino/Desafio-BackEnd/actions), preparado para compilar os projetos, subir os containers, realizar o migration da base de dados e executar todos os testes, validando a solução por inteiro.

```
> .github/workflows/main.yml
```

### Notas

- Usado para criar os certificados para HTTPS dentro do Docker:
`dotnet dev-certs https -ep %USERPROFILE%\.aspnet\https\aspnetapp.pfx -p mypass123`
- Utilizar futuramente para documentação: https://dotnet.github.io/docfx/index.html