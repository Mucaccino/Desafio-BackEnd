# Desafio backend.
Seja muito bem-vindo ao desafio backend.

## Docker

Define três serviços: api, postgres e RabbitMQ:

- O serviço api usa a imagem do SDK do .NET Core 3.1, definindo as variáveis de ambiente para configuração do banco de dados PostgreSQL e do RabbitMQ.
- O serviço postgres usa a imagem do PostgreSQL e define as variáveis de ambiente para criar um usuário, senha e banco de dados.
- O serviço RabbitMQ usa a imagem do emulador do RabbitMQ para simular o serviço.
- As portas 5000 (API), 5432 (PostgreSQL) e 5672 (RabbitMQ) são expostas para acesso externo.
- Define uma rede chamada mottu-net para comunicação entre os serviços.
- Usa um volume chamado postgres-data para persistir os dados do PostgreSQL.

## API .NET Core

O Dockerfile cria duas etapas:
- (build) é usada para restaurar as dependências e compilar o código.
- (runtime) é usada para criar a imagem final para execução da aplicação.

## RabbitMQ

- Porta 5672 para conexões.
- Porta 15672 para acesso ao admin.

`RABBITMQ_CONNECTION_STRING=amqp://guest:guest@rabbitmq:5672/`

## Execução

> docker-compose up