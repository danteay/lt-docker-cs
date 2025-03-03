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
- Seleccionar `JSON` y pegar el siguiente JSON:

  ```json
  {
      "Version": "2012-10-17",
      "Statement": [
          {
              "Effect": "Allow",
              "Action": [
                  "ecr:DescribeRepositoryCreationTemplates",
                  "ecr:UpdateRepositoryCreationTemplate",
                  "ecr:BatchDeleteImage",
                  "ecr:BatchGetRepositoryScanningConfiguration",
                  "ecr:DeleteRepository",
                  "ecr:TagResource",
                  "ecr:BatchCheckLayerAvailability",
                  "ecr:GetLifecyclePolicy",
                  "ecr:DescribeImageScanFindings",
                  "apigateway:*",
                  "ecr:CreateRepository",
                  "ecr:GetDownloadUrlForLayer",
                  "ecr:PutImageScanningConfiguration",
                  "ecr:DescribePullThroughCacheRules",
                  "ecr:GetAuthorizationToken",
                  "ecr:DeleteLifecyclePolicy",
                  "ecr:PutImage",
                  "ecr:CreateRepositoryCreationTemplate",
                  "ecr:ValidatePullThroughCacheRule",
                  "ecr:GetAccountSetting",
                  "ecr:UntagResource",
                  "ecs:*",
                  "ecr:BatchGetImage",
                  "ecr:DescribeImages",
                  "ecr:StartLifecyclePolicyPreview",
                  "ecr:PutAccountSetting",
                  "ecr:UpdatePullThroughCacheRule",
                  "ecr:InitiateLayerUpload",
                  "ecr:PutImageTagMutability",
                  "ecr:StartImageScan",
                  "ecr:DescribeImageReplicationStatus",
                  "ecr:ListTagsForResource",
                  "ecr:UploadLayerPart",
                  "ecr:CreatePullThroughCacheRule",
                  "ecr:ListImages",
                  "ecr:PutRegistryPolicy",
                  "ecr:GetRegistryScanningConfiguration",
                  "ecr:CompleteLayerUpload",
                  "ecr:DescribeRepositories",
                  "ecr:DeleteRepositoryPolicy",
                  "ecr:ReplicateImage",
                  "vpc-lattice:*",
                  "ecr:GetRegistryPolicy",
                  "ecr:PutLifecyclePolicy",
                  "ecr:GetLifecyclePolicyPreview",
                  "ecr:DescribeRegistry",
                  "ecr:PutRegistryScanningConfiguration",
                  "ecr:DeletePullThroughCacheRule",
                  "ecr:BatchImportUpstreamImage",
                  "ecr:SetRepositoryPolicy",
                  "vpce:*",
                  "ecr:DeleteRepositoryCreationTemplate",
                  "ecr:DeleteRegistryPolicy",
                  "ecr:GetRepositoryPolicy",
                  "ecr:PutReplicationConfiguration"
              ],
              "Resource": "*"
          }
      ]
  }
  ```

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

### Creacion de infraestructura

Para crear la infraestructura necesaria para el proyecto, ejecuta el siguiente comando:

```sh
make tf_apply
```

Esto creara o seleccionara un workspace en terraform llamado `prod` en el cual se crearan los recursos necesarios para el
proyecto dentro de AWS.

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
- `PHASE_API_KEY`: API key de Phase.dev para manejar los secrets
- `ACCESS_TOKEN`: Access token creado en Github para el pipeline

##### Variables

- `LOGSTASH_SEC_ID`: ID del secret de logstash en Phase.dev
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

