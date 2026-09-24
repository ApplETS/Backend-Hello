# Backend Hello
This API allows to interface the database and handles business functions

## Installation
Different ways can be used to spin up a backend Hello API.

### Create the DB
First of, you need to create a postgreSQL DB if it's not already done.
You can check by running this command.
```bash
docker ps -a
```
It should look like this:
```bash
ONTAINER ID   IMAGE                COMMAND                  CREATED          STATUS          PORTS                                                                                                NAMES
592987e98cc8   backend-hello-core   "dotnet api.core.dll"    32 minutes ago   Up 32 minutes   8082/tcp, 0.0.0.0:8081->8080/tcp, [::]:8081->8080/tcp, 0.0.0.0:8082->8081/tcp, [::]:8082->8081/tcp   backend-hello-core-1
6d67c189ee84   postgres             "docker-entrypoint.s…"   5 days ago       Up 13 hours     0.0.0.0:5432->5432/tcp, [::]:5432->5432/tcp                                                          hello-database-setup-db-1
a20bbf3cfe10   adminer              "entrypoint.sh docke…"   5 days ago       Up 13 hours     0.0.0.0:8090->8080/tcp, [::]:8090->8080/tcp                                                          hello-database-setup-adminer-1
3c94feaf799e   redis:7.2.4          "docker-entrypoint.s…"   5 days ago       Up 13 hours     0.0.0.0:6379->6379/tcp, [::]:6379->6379/tcp                                                          hello-database-setup-redis-1
5c887780659b   nginx:1.25.3         "/docker-entrypoint.…"   5 days ago       Up 13 hours     0.0.0.0:6464->80/tcp, [::]:6464->80/tcp                                                              hello-database-setup-cdn-1
```

> ⚠️ If your database is already created and running, you can skip this step and directly go to [Run the image](#run-the-image).

Access this [repository](https://github.com/ApplETS/Hello-Database-Setup) to get started!

### Installation introduction

There are multiple ways you can install the API:
- Using the docker compose file:  [Docker Compose](#-docker-compose)
  - Depending on the running parameter you can modify the existing code.
- Using a docker image:  [Docker Installation](#-docker-installation) (Deprecated)
  - This method doesn't allows you to modify easily the existing code and is more suited for a local server when there is only front-end development to be done.
- Using the classic approach with Visual Studio: [Local Installation](#-local-installation)

### 🐳 Docker Compose
After cloning this repository, open it via a terminal or an IDE.  
```bash
cd path/to/repo
# With an SSH Key
git clone git@github.com:ApplETS/Backend-Hello.git
# Without an ssh key
git clone https://github.com/ApplETS/Backend-Hello.git
```
You'll need to setup the environment variables in the `.env` file.  
Simply copy and paste the `.env.template` file, rename it to `.env` and fill it with the correct values.  

Or, in the same directory as this README, run this command and fill it with the correct values:
```bash
cp core/.env.template .env
```
In the same directory as this README, run this the docker compose command:
```bash
docker compose up -d 
```

After running the docker compose file, the api should be accessible via:   

> ⚠️ Don't forget to rebuild the app after modifying the source code, use the command docker compose up -d --build

### 🐳 Docker Installation
Start by pulling the image from docker hub
```bash
docker pull ghcr.io/applets/backend-hello:<VERSION>
```
multiple versions are available, you can check them [here](https://github.com/ApplETS/Backend-Hello/pkgs/container/backend-hello/versions)

The default one is `latest` but for cutting edge updates you can use `main` branch name.

#### Running the docker image
You'll need to setup the environment variables in the `.env` file

Simply copy and paste the `.env.template` file, rename it to `.env` and fill it with the correct value.

Then, run the image on your local machine
```bash
docker run --env-file .env --restart always -d -p 8080:8080 -v <PATH_TO_VOLUME_FOLDER>:/app/volume:rw --name ps-api ghcr.io/applets/backend-hello:<VERSION>
```

You can navigate to `http://localhost:8080/swagger` to check if the API is running correctly!

### 👷 Local Installation
You will need a couple of things to get started developing and maintaining Hello-Backend.
- Docker (suppose to be already installed at this point)
- [Git](https://git-scm.com/downloads)
- Visual Studio 2022 or above version.
  - Make sure to check the ASP.NET and web development component
<img width="927" alt="image" src="https://github.com/ApplETS/Backend-Hello/assets/25663435/43d8ce41-2990-445b-9621-8f1418d33f0f">

- .NET8 (which is supposed to come package with VS2022, but in case it's not.)

#### Setup
1. Open Bash, navigate to somewhere you like:
```bash
cd path/to/repo
```
2. Clone the repo using an [ssh key](https://docs.github.com/en/authentication/connecting-to-github-with-ssh/generating-a-new-ssh-key-and-adding-it-to-the-ssh-agent) or without:
```bash
# With an SSH Key
git clone git@github.com:ApplETS/Backend-Hello.git
# Without an ssh key
git clone https://github.com/ApplETS/Backend-Hello.git
```
3. Open the .sln in Visual Studio 2022
4. You'll now need to create your `.env` file from the `.env.template`. You can ask help to fill the empty values from a project maintainer.


#### Running the app using Visual Studio
To run the app make sure to check Docker to run it using docker:

<img width="200" alt="image" src="https://github.com/ApplETS/Backend-Hello/assets/25663435/a1dc7a1f-5e53-4361-9e65-95f284ed29fb">

Then click on ![image](https://github.com/ApplETS/Backend-Hello/assets/25663435/de874f36-47e0-4e9f-8c8f-9e7e21a67d6c) to run the API.

You can navigate to http://localhost:8080 where you're app is running.
