# EF Core

O projeto utiliza Entity Framework para a estrutura de entidades e a conexão com o PostgreSQL, centralizada no projeto `Motto.Data`.

## Executando as Migrations

Para atualizar o banco de dados com as migrations mais recentes, execute o seguinte comando:

# [make](#tab/make)

```sh
make setup
```

# [dotnet-cli](#tab/dotnet)

```sh
dotnet ef database update -p Motto.Data
```