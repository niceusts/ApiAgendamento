# Backend - ApiAgendamento

Backend em ASP.NET Core 8 para o sistema de agendamento médico.

## Pré-requisitos
- .NET 8 SDK
- PostgreSQL

## Configuração
1. Configure o banco de dados PostgreSQL e ajuste a string de conexão no arquivo `.env`.
2. Configure as variáveis de ambiente JWT (`JWT_KEY`, `JWT_ISSUER`, `JWT_AUDIENCE`).
3. (Opcional) Rode as migrations:
  ```powershell
  dotnet ef database update
  ```

## Como rodar
```powershell
dotnet run --launch-profile https
```
Acesse a API e o Swagger em: https://localhost:7056/swagger


```
DB_CONNECTION=Host=localhost;Port=5432;Database=ApiAgendamentoDb;Username=postgres;Password=sua_senha
JWT_KEY=sua-chave-super-secreta-para-jwt-1234567890
JWT_ISSUER=ApiAgendamento
JWT_AUDIENCE=ApiAgendamentoUser
```

> **Importante:** Não suba o `.env` para o GitHub!

### 4. Restaure os pacotes

```
dotnet restore
```

### 5. Crie o banco e as tabelas

rode a variável de ambiente no terminal
```
 Host=localhost;Port=5432;Database=ApiAgendamentoDb;Username=postgres;Password=sua_senha

```
crie o banco: 
```
dotnet ef database update --project ApiAgendamento/ApiAgendamento.csproj
```

### 6. Rode a aplicação

```
dotnet run --project ApiAgendamento/ApiAgendamento.csproj
```

A API estará disponível em `https://localhost:5001` ou `http://localhost:5000`.

## Testes

- Estrutura para testes unitários e de integração em `/tests` (em produção).

## Tecnologias

- .NET 8
- Entity Framework Core
- PostgreSQL
- JWT Authentication
- BCrypt.Net (hash de senha)

- DotNetEnv (variáveis de ambiente)
