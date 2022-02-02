# Url.Short-Api

An Url shortener rest api written in ASP.Net core

## Build steps

### Requirements

- [Dotnet](https://dotnet.microsoft.com/download)
- [Postgres](https://www.postgresql.org/)

>
> Create the database `urlshort` in postgres and set _**UserId**_ and _**Password**_ environment variables for the database credentials.
>
> Set _**AppSecret**_ environment variables for the integration with the frontend and to acquire the jwt token or the admin routes.

### Setup

```shell
git clone https://github.com/Ola-jed/Url.Short-Api.git
cd Url.Short-Api/Url.Short-Api
# Set the env vars before
# export X=xxx or dotnet user-secrets set "Key" "Value"
dotnet ef database update
dotnet run
```

## TODO

- [ ] Host on [Azure](https://azure.microsoft.com)
- [ ] Write unit tests