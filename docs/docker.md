# Docker

A solução conta com docker-compose para realizar a montagem do ambiente de serviços e dependências no Docker.

## Compose Principal

```
> docker-compose up --build
```

### Dependências

* `postgresql`: utilizado como base de dados
* `rabbitmq`: utilizado como messageria
* `minio`: utilizado como bucket de arquivos 
* `seq`: utilizado para monitoramento de logs 

## Compose Projects

Possui o `docker-compose.projects.yml` que realiza também a montagem dos projetos no Docker (e deve ser executado junto ao `docker-compose.yml`).

```
> docker-compose -f docker-compose.yml -f docker-compose.projects.yml up --build
```

### Projetos

* `webapi`: execução do Motto.WebApi
* `consumner`: execução do Motto.ConsumerApp