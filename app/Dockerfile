# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /src

# Copy the project files
COPY app.csproj .
RUN dotnet restore

# Copy the rest of the source code
COPY . .

# Build the application
RUN dotnet publish -c Release -o ./publish

# Stage 2: Create the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS runtime
WORKDIR /app

ARG stage="dev"
ARG port="5000"

# Copy the published application from the build stage
COPY --from=build /src/publish .

# Remove the default config folder to not load all stages
RUN rm -rf config

# Copy the config file for the current stage
COPY --from=build "/src/config/${stage}.yml" "./config/${stage}.yml"

ENV SERVICE_NAME="lt-docker-netcore"
ENV STAGE="${stage}"
ENV PORT="${port}"

# Secrets manager credentials
ENV PHASE_API_KEY=""
ENV PHASE_APP_ID=""

# Expose the port your application will run on
EXPOSE ${port}

# Set the entry point for the application
ENTRYPOINT ["dotnet", "app.dll"]