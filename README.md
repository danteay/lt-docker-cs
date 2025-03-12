# Master en arquitectura digital con Docker

Esta guía te ayudará a inicializar el proyecto .NET Core usando Nix, configurar un contenedor Docker para la base de 
datos y crear migraciones instalando `dotnet-ef` localmente en el proyecto usando `.config`.

## Prerequisites

- [Nix](https://nixos.org/download.html) Instalado
- [Nix Flakes](https://nixos.wiki/wiki/Flakes) habilitado
- [Docker](https://www.docker.com/get-started) Instalado
- [Phase.dev](https://phase.dev) cuenta para manejar secretos
- [Cuenta de AWS](https://aws.amazon.com/) para desplegar el proyecto

## Steps 

### Configuracion de Secrets

Para ejecutar el proyecto tanto en modo desarrollo (dentro de docker) como en produccion es necesario configurar 2
secretos dentro de Phase.dev. Para configurar los secretos sigue los siguientes pasos:

- Ir a la consola de Phase.dev y entrar a la seccion `Apps`
- Crear 2 applicaciones con los siguientes nombres y valores para el ambiente de Development:
  - postgres:
    - `POSTGRES_USER`: `root`
    - `POSTGRES_PASSWORD`: `root`
    - `POSTGRES_HOST`: `postgres`
    - `POSTGRES_PORT`: `5432`
    - `POSTGRES_DB`: `postgres`
  - elastic:
    - `ELASTIC_HOST`: `http://elasticsearch:9200`
    - `ELASTIC_USER`: `elastic`
    - `ELASTIC_PASSWORD`: `elastic`
- Guarda los IDs de las aplicaciones creadas para usarlos posteriormente en el despliegue del proyecto y la ejecucion 
  del ambiente de desarrollo
- Entra a cada una de las aplicaciones y en la seccion `Settings` habilita la opcion `Server-side encryption (SSE)`
  para permitir su consumo mediante API
- En la seccion `Access Control > Service Accounts` crea un nuevo Service Account tanto para el ambiente de desarrollo
  como para el de produccion y asigna los permisos necesarios para consumir los secretos de las aplicaciones creadas.
  **El ambiente de produccion no requerira acceso al secret de elastic dado que solo es necesario en desarrollo**
- Entra a cada service account y crea un `Access Token` para cada uno el cual servira para configurar la variable 
  `PHASE_API_KEY` tanto en el los secrets del pipeline de Github Actions como en el ambiente de desarrollo

### Ejecucion del ambiente de desarrollo

#### Inicializar el proyecto .NET Core usando Nix

Todas las dependencias del proyecto se gestionarán con Nix. Para inicializar el proyecto .NET Core, ejecuta el siguiente comando:

```sh
nix develop --impure
```

#### Configurar las variables de entorno

Para que el proyecto funcione correctamente es necesario configurar variables de entorno relacionadas con los secretos

Para esto crea un archivo `.envrc` en la raiz del proyecto con el siguiente contenido:

```sh
export PHASE_API_KEY="<your-phase-dev-api-key-for-development>"
export POSTGRES_SEC_ID="<postgres-app-id>"
export ELASTIC_SEC_ID="<elastic-app-id>"
```

Este archivo sera cargado automaticamente al entrar al directo en la consola. Si direnv marca un error al intentar cargar
el archivo `.envrc` ejecuta el siguiente comando:

```sh
direnv allow .
```

#### Configurar un contenedor Docker para la base de datos

Este projecto usa postgres como base de datos. Para configurar un contenedor Docker para la base de datos, ejecuta el siguiente comando:

```sh
docker-compose up postgres -d
```

#### Instalar `dotnet-ef` localmente

Para instalar las tools localmente en el proyecto, ejecuta el siguiente comando:

```sh
dotnet tool restore
```

#### Aplicar migraciones

Para aplicar las migraciones, ejecuta el siguiente comando:

```sh
dotnet dotnet-ef database update
```

#### Ejecutar el proyecto

Para ejecutar el pryecto localmente fuera de docker, ejecuta el siguiente comando:

```sh
make run_dev
```

Podras ver el proyecto corriendo en `http://localhost:5001`, y la documentación de la API en `http://localhost:5001/swagger`.

## Lint and format

To lint and format code this project use `csharpier`. To execute the linter and formatter, run the following command on 
the root of the project:

```sh
make net_format
```

## Deployment

Sigue la guía de [Deployment](deployment.md) para desplegar el proyecto en AWS.


