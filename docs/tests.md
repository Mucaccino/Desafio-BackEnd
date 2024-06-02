# Testes

O projeto `Motto.Tests` possui testes de integração com WebApi e testes unitários.

- `ApiIntegrationTests`: executa o programa ./api e teste conexão e login em base de dados real para testar funcionamento da API.
- `____ControllerTests`: executa os testes unitários relacionados aos controllers - usando dados do Mock. 

## Executar testes

# [make](#tab/make)

```sh
make tests
```

# [dotnet-cli](#tab/dotnet)

```sh
dotnet test ./Motto.Tests/Motto.Tests.csproj
```

# [Visual Studio](#tab/vs)

> Executar testes pela interface do gerenciador de testes.