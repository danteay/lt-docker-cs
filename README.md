# Master en arquitectura digital con Docker

Esta guía te ayudará a inicializar el proyecto .NET Core usando Nix, configurar un contenedor Docker para la base de datos y crear
migraciones instalando `dotnet-ef` localmente en el proyecto usando `.config`.

## Prerequisites

- [Nix](https://nixos.org/download.html) installed
- [Nix Flakes](https://nixos.wiki/wiki/Flakes) enabled
- [Docker](https://www.docker.com/get-started) installed
- [Phase.dev](https://phase.dev) account to mange secrets

## Steps 

### 1. Inicializar el proyecto .NET Core usando Nix

Todas las dependencias del proyecto se gestionarán con Nix. Para inicializar el proyecto .NET Core, ejecuta el siguiente comando:

```sh
nix develop
```

### 2. Configurar un contenedor Docker para la base de datos

Este projecto usa postgres como base de datos. Para configurar un contenedor Docker para la base de datos, ejecuta el siguiente comando:

```sh
docker-compose up postgres -d
```

### 3. Instalar `dotnet-ef` localmente

Para instalar las tools localmente en el proyecto, ejecuta el siguiente comando:

```sh
dotnet tool restore
```

### 4. Aplicar migraciones

Para aplicar las migraciones, ejecuta el siguiente comando:

```sh
dotnet dotnet-ef database update
```

### 5. Ejecutar el proyecto

Para ejecutar el pryecto localmente fuera de docker, ejecuta el siguiente comando:

```sh
dotnet run
```

Podras ver el proyecto corriendo en `http://localhost:5001`, y la documentación de la API en `http://localhost:5001/swagger`.

## Lint and format

To lint and format code this project use `csharpier`. To execute the linter and formatter, run the following command:

```sh
dotnet dotnet-csharpier .
```


