# Workflow

O projeto está configurado com pipelines para o [Github Actions](https://github.com/Mucaccino/Desafio-BackEnd/actions).

## CLI

Workflow de execução principal, preparado para compilar os projetos, subir os containers, realizar o migration da base de dados e executar todos os testes, validando a solução por inteiro.

```
> .github/workflows/main.yml
```

## Pages

Workdlow de criação de sites, preparado para compilar, gerar e publicar o site da documentação no Github Pages.

```
> .github/workflows/pages.yml
```