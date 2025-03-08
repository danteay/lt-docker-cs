# Deployment

## Requerimientos

- [Terraform](https://www.terraform.io/downloads.html)
- [AWS CLI](https://aws.amazon.com/cli/)
- [AWS Account](https://aws.amazon.com/)
- [Phase.dev](https://phase.dev) account to mange secrets
- [Docker](https://www.docker.com/get-started) installed
- [Commitizen](https://commitizen-tools.github.io/commitizen/getting_started/)

### Recomendaciones

Para facilitar la configuracion del entorno se recomienda instalar las herramientas de trabajo en un entorno Nix. Para
instalar Nix en tu sistema operativo sigue las instrucciones en [Nix](https://nixos.org/download.html).

Posteriormente, para instalar las herramientas necesarias para el proyecto, ejecuta el siguiente comando:

```sh
nix develop --impure
```

## Pasos

### Creacion de usuario cicd_user en AWS

Es necesario un usuario con permisos suficientes para crear recursos usando Terraform, asi como para poder usar sus
credenciales en Github Actions. Para crear un usuario con los permisos necesarios es necesario entrar a la consola de AWS
y seguir los siguientes pasos:

- Ir a la consola de AWS y entrar a `IAM`
- Ir a `Policies` y crear una nueva con el boton `Create policy`
- Seleccionar `JSON` y copiar el contenido del archivo `cicd_user_policy.json` de la raiz del proyecto
- Como Nombre de la politica poner `CICDDeployment`
- Crear la politica con el boton `Create policy`
- Ir a `Users` y crear un nuevo usuario con el boton `Create user`
- Como nombre poner `cicd_user`
- Seleccionar `Programmatic access`
- En los permisos seleccionar `Attach policies directly` y buscar la politica `CICDDeployment`
- Continuar el flujo hasta crear el usuario
- Dentro de `User` ingresar el usuario creado y en la pestaña `Security credentials` y en la seccions `Access key` pulsar `Create access key`
- Para `use case` selecciona `Other` y presionar `Siguiente`
- Agrega un tag opcional al access key y presiona `Create access key`
- Guarda el `Access key ID` y el `Secret access key` en un lugar seguro o descarga el CSV con las credenciales

### Configuración de AWS CLI

Para configurar AWS CLI, ejecuta el siguiente comando:

```sh
aws configure

# Output
# AWS Access Key ID: <YOUR_AWS_ACCESS_KEY_ID>
# AWS Secret Access Key: <YOUR_AWS_SECRET_ACCESS_KEY>
# Default region name: <YOUR_AWS_REGION> (recommended: us-east-1)
# Default output format: json
```

### Creacion de Service linked Role para ECS

Para poder crear un cluster de ECS es necesario crear un Service linked role para ECS. Para crear el role, ejecuta el 
siguiente comando:

```sh
aws iam create-service-linked-role --aws-service-name ecs.amazonaws.com
```

### Creacion de Service Linked Role para Elastic Load Balancer

Para poder crear un Load Balancer en ECS es necesario crear un Service linked role para Elastic Load Balancer. Para 
crear el role, ejecuta el siguiente comando:

```sh
aws iam create-service-linked-role --aws-service-name elasticloadbalancing.amazonaws.com
```

### Create DynamoDB table for remote state locking

Para poder usar el remote state de Terraform es necesario crear una tabla de DynamoDB para el locking del estado. Para
crear la tabla, ejecuta el siguiente comando:

```sh
aws dynamodb create-table \
    --region <tu-region> \
    --table-name lt-docker-cs-terraform-locks \
    --attribute-definitions AttributeName=LockID,AttributeType=S \
    --key-schema AttributeName=LockID,KeyType=HASH \
    --provisioned-throughput ReadCapacityUnits=5,WriteCapacityUnits=5
````

### Crear Bucket de S3 para el remote state

Para poder usar el remote state de Terraform es necesario crear un bucket de S3 para almacenar el estado remoto. Para
crear el bucket, ejecuta el siguiente comando:

```sh
aws s3api create-bucket --bucket lt-docker-cs-terraform-state --region <tu-region>
```

### Creacion de infraestructura

Para crear la infraestructura necesaria para el proyecto, ejecuta el siguiente comando:

```sh
env AWS_REGION=<tu-region> make tf_apply
```

Esto creara o seleccionara un workspace en terraform llamado `prod` en el cual se crearan los recursos necesarios para el
proyecto dentro de AWS.

Para finalizar descomentar del archivo `terraform/main.tf` la seccion correspondiente al modulo `lt-docker-cs-service`
para habilitar el despliegue del servicio de ECS usando el pipeline de Github Actions.

### Configuración de Github Actions

#### Creacion de Access Token para uso en el Pipeline

Para poder usar Github Actions es necesario crear un access token con permisos suficientes para poder ejecutar el pipeline.

- Ir a la consola de Github y entrar a `Settings`
- Ir a `Developer settings` y seleccionar `Personal access tokens`
- Seleccionar la seccion `Fine-grained tokens` y presionar `Generate new token`
- Dar un nombre al token y Seleccionar dentro de la seccion `Repository access` la opcion `Only select repositories` y 
  seleccionar el repositorio del proyecto.
- Dentro de la seccion `Repository permissions` seleccionar lo siguiente:
  - `Actions` -> `Read & write`
  - `Action Variables` -> `Read & write`
  - `Code` -> `Read & write`
  - `Deployments` -> `Read & write`
  - `Environment` -> `Read & write`
  - `Pull requests` -> `Read & write`
  - `Repository custom properties` -> `Read`
  - `Secrets` -> `Read`
  - `Metadata` -> `Read`
- Presionar `Generate token` y guardar el token en un lugar seguro

#### Configuracion de Secrets y Variables

Para configurar Github actions en necesario crear los siguientes secrets y variables en el repositorio:

##### Secrets

- `AWS_ACCESS_KEY_ID`: Access key ID del usuario `cicd_user` creado en AWS
- `AWS_SECRET_ACCESS_KEY`: Secret access key del usuario `cicd_user` creado en AWS
- `AWS_REGION`: Region de AWS donde se crearan los recursos
- `AWS_ACCOUNT_ID`: ID de la cuenta de AWS
- `PHASE_API_KEY`: API key de Phase.dev para manejar los secrets de produccion
- `ACCESS_TOKEN`: Access token creado en Github para el pipeline

##### Variables

- `POSTGRES_SEC_ID`: ID del secret de postgres en Phase.dev
- `GIT_NAME`: Nombre del usuario de git para los commits (debe ser admin del repositorio)
- `GIT_EMAIL`: Email del usuario de git para los commits (debe ser admin del repositorio)

#### Rule sets del repositorio

Si su repositorio cuenta con algún ruleset que impide ejecutar acciones sobre el branch principal, es necesario deshabilitar
ese rule set para el usuario específico configurado en el pipeline. Esto puede hacerse si el usuario es administrador
del repositorio. Para deshabilitar el rule set sigue los siguientes pasos:

- Ir a la consola de Github y entrar al repositorio
- Ir a `Settings` y seleccionar `Rules` > `Rulesets`
- Seleccionar la rule set que se desea modificar
- En la seccion `Bypass list` agregar el elemento `Repository admin`

### Eliminacion de recursos

Para eliminar los recursos creados en AWS, ejecuta los siguientes pasos:

- Entrar a la consola de AWS y en la seccion de ECR seleccionar el repositorio creado, posteriormente eliminar las imagenes
  almacenadas en el repositorio.
- Ejecutar el siguiente comando para eliminar los recursos creados en AWS:

  ```sh
  make tf_destroy
  ```
  
- Eliminar los service linked roles creados en los pasos anteriores ejecutando los siguientes comandos:

  ```sh
  aws iam delete-service-linked-role --role-name AWSServiceRoleForECS
  aws iam delete-service-linked-role --role-name AWSServiceRoleForElasticLoadBalancing
  ```


