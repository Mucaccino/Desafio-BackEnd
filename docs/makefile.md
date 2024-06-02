# Makefile

A solução inclui o **makefile** para auxiliar com comandos de compilação e subida de serviços e dependências e dos próprios projetos em si. 

## Comandos principais

Setup e seed inicial de base de dados:

```sh
make setup
```

Subir containers de dependências:

```sh
make services
```

Subir containers de projetos:

```sh
make projects
```

Outros comandos podem ser encontrados no [makefile](../makefile).

## Como instalar o make

# [Linux](#tab/linux)

```sh
sudo apt install make
```

# [Windows](#tab/windows)

```sh
choco install make
```

**ou** baixe o arquivo [source](https://gnuwin32.sourceforge.net/packages/make.htm) para instalação.
