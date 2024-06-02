# Entity Framework

O projeto utiliza Entity Framework para a estrutura de entidades e a conexão com o PostgreSQL, centralizada no projeto `Motto.Data`.

## Atualizando as Migrations

Para atualizar o banco de dados com as migrations mais recentes, execute o seguinte comando:

```sh
dotnet ef database update -p Motto.Data
```

### Executar pelo make
```
> make setup
```