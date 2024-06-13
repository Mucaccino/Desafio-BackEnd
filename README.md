# Desafio Back-end

Bem-vindo ao desafio backend. Seu objetivo é criar uma aplicação para gerenciar aluguel de motos e entregadores.

## O que o projeto faz

Este projeto é uma aplicação completa desenvolvida em .NET 8 para gerenciar o aluguel de motos e entregadores. Ele inclui uma API, uma aplicação de console para processamento de mensagens, e integrações com diversos serviços como [RabbitMQ](https://www.rabbitmq.com/), [PostgreSQL](https://www.postgresql.org/), [MinIO](https://min.io/) e [Seq](https://datalust.co/seq). A solução possui configuração para execução em [Docker](https://www.docker.com/) e contém o [Swagger UI](https://swagger.io/tools/swagger-ui/) para interação com a API.

## Como começar a usar o projeto

### Pré-requisitos

- [.NET 8](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/apis?view=aspnetcore-8.0)
- [Docker](https://www.docker.com/)
- [Make](https://gnuwin32.sourceforge.net/packages/make.htm)

### Passos para iniciar a api

1. Clone o repositório:
    ```sh
    git clone https://github.com/Mucaccino/Desafio-BackEnd.git
    cd Desafio-BackEnd
    ```

2. Configure e inicie os serviços dependentes:
    ```sh
    make setup
    make docker services
    ```

3. Construa e inicie os projetos:
    ```sh
    make docker projects
    ```
    > ou execute os projetos manualmente no VS Code ou Visual Studio.

4. Vá para o Swagger UI em http://localhost:5000/swagger

5. Faça login pelo endpoint api/Auth/login com as credenciais:
    ```json
    { "username": "admin", "password": "123mudar" }
    { "username": "entregador", "password": "123mudar" }
    ```

6. Autorize o Swagger UI com o token recebido.

### Executando o consumidor de mensagens

O aplicativo Motto.ConsumerApp consome mensagens do RabbitMQ. 
```sh
make run consumer
```

### Executando os testes

Para rodar os testes de integração e unitários:
```sh
make tests
```

## Onde obter ajuda

Se precisar de ajuda, você pode abrir uma issue no repositório do GitHub ou consultar a [documentação adicional](https://mucaccino.github.io/Desafio-BackEnd/). 

## Quem mantém esse projeto

Este projeto é mantido por Murillo L do Carmo (murillodocarmo@gmail.com) como caso de uso para apresentação e portifólio.

## Licença
Este projeto está licenciado sob a licença MIT - veja o arquivo LICENSE para detalhes.