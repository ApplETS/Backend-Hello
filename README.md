# Backend PS

## Installation Docker
Start by pulling the image from docker hub
```bash
docker pull ghcr.io/applets/backend-hello:<VERSION>
```
multiple versions are available, you can check them [here](https://github.com/ApplETS/Backend-Hello/pkgs/container/backend-hello/versions)

The default one is `latest` but for cutting edge updates you can use `main` branch name.

## Create the DB

ℹ️ If your database is already created and running, you can skip this step and directly go to [Run the image](#run-the-image).

Access this [repository](https://github.com/ApplETS/Hello-Database-Setup) to get started!

## Run the image
You'll need to setup the environment variables in the `.env` file

Simply copy and paste the `.env.template` file, rename it to `.env` and fill it with the correct value.

Then, run the image on your local machine
```bash
docker run --env-file .env --restart always -d -p 8080:8080 --name ps-api applets/backend-hello:<VERSION>
```

You can navigate to `http://localhost:8080/swagger` to check if the API is running correctly!

