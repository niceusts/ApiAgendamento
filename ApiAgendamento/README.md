
# Backend - ApiAgendamento

Backend em ASP.NET Core 8 para o sistema de agendamento médico.

## Pré-requisitos
- .NET 8 SDK
- PostgreSQL

## Como rodar o projeto

1. **Clone o repositório e acesse a pasta:**
  ```powershell
  git clone <url-do-repo>
  cd ApiAgendamento
  ```

2. **Restaure os pacotes:**
  ```powershell
  dotnet restore
  ```

3. **Configure o banco de dados e variáveis de ambiente:**
  - Crie um arquivo `.env` na pasta `ApiAgendamento/` com o conteúdo:
    ```
    DB_CONNECTION=Host=localhost;Port=5432;Database=ApiAgendamentoDb;Username=postgres;Password=sua_senha
    JWT_KEY=sua-chave-super-secreta-para-jwt-1234567890
    JWT_ISSUER=ApiAgendamento
    JWT_AUDIENCE=ApiAgendamentoUser
    ```
  - Não suba o `.env` para o GitHub!

4. **Crie o banco e as tabelas:**
  - Certifique-se que o PostgreSQL está rodando.
  - Execute:
    ```powershell
    dotnet ef database update --project ApiAgendamento/ApiAgendamento.csproj
    ```

5. **Rode a aplicação:**
  ```powershell
  dotnet run --launch-profile https --project ApiAgendamento/ApiAgendamento.csproj
  ```
  - Acesse a API e o Swagger em: https://localhost:7056/swagger

- Estrutura para testes unitários e de integração em `/tests` (em produção).

## Tecnologias

- .NET 8
- Entity Framework Core
- PostgreSQL
- JWT Authentication
- BCrypt.Net (hash de senha)

- DotNetEnv (variáveis de ambiente)
