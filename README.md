# ApiAgendamento

Sistema de Agendamento Médico — Backend (.NET 8 + PostgreSQL + DDD + Clean Architecture)

## Visão Geral

Este projeto é uma API REST para agendamento de consultas médicas, seguindo os princípios de DDD (Domain-Driven Design) e Clean Architecture.  
Permite cadastro de médicos e pacientes, gerenciamento de horários disponíveis, criação de agendamentos, autenticação segura (JWT) e persistência em banco relacional (PostgreSQL).

## Arquitetura

- **Domain**: Entidades, regras de negócio e interfaces de repositório.
- **Application**: Serviços de aplicação (casos de uso).
- **Infrastructure**: Persistência (EF Core + PostgreSQL), repositórios concretos.
- **Api**: Controllers, autenticação, endpoints REST.

## Segurança

- Autenticação via JWT.
- Senhas armazenadas com hash seguro (BCrypt).
- Variáveis sensíveis (banco, JWT) em arquivo `.env` (NÃO versionar).

## Como rodar o projeto

### 1. Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL](https://www.postgresql.org/download/)
- [DotNet CLI](https://learn.microsoft.com/pt-br/dotnet/core/tools/)
- (Opcional) [Visual Studio 2022+](https://visualstudio.microsoft.com/pt-br/)

### 2. Clone o repositório

```
git clone https://github.com/seu-usuario/seu-repo.git
cd seu-repo
```

### 3. Crie o arquivo `.env` na raiz

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

## Endpoints principais

- **Autenticação:**  
  - `POST /api/auth/register` — Cadastro de usuário (médico ou paciente)
  - `POST /api/auth/login` — Login (retorna JWT)
- **Médico:**  
  - `GET /api/medico/{medicoId}/horarios` — Listar horários do médico
  - `POST /api/medico/{medicoId}/horarios` — Adicionar horário disponível
- **Paciente:**  
  - `GET /api/paciente/meus-agendamentos` — Listar agendamentos do paciente autenticado
- **Agendamento:**  
  - `POST /api/agendamento` — Criar agendamento (autenticado)

> Todos os endpoints (exceto login e registro) exigem JWT no header:  
> `Authorization: Bearer <seu_token>`

## Testes

- Estrutura para testes unitários e de integração em `/tests` (em produção).

## Tecnologias

- .NET 8
- Entity Framework Core
- PostgreSQL
- JWT Authentication
- BCrypt.Net (hash de senha)

- DotNetEnv (variáveis de ambiente)
