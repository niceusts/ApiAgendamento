# ApiAgendamento

Sistema de Agendamento M�dico � Backend (.NET 8 + PostgreSQL + DDD + Clean Architecture)

## Vis�o Geral

Este projeto � uma API REST para agendamento de consultas m�dicas, seguindo os princ�pios de DDD (Domain-Driven Design) e Clean Architecture.  
Permite cadastro de m�dicos e pacientes, gerenciamento de hor�rios dispon�veis, cria��o de agendamentos, autentica��o segura (JWT) e persist�ncia em banco relacional (PostgreSQL).

## Arquitetura

- **Domain**: Entidades, regras de neg�cio e interfaces de reposit�rio.
- **Application**: Servi�os de aplica��o (casos de uso).
- **Infrastructure**: Persist�ncia (EF Core + PostgreSQL), reposit�rios concretos.
- **Api**: Controllers, autentica��o, endpoints REST.

## Seguran�a

- Autentica��o via JWT.
- Senhas armazenadas com hash seguro (BCrypt).
- Vari�veis sens�veis (banco, JWT) em arquivo `.env` (N�O versionar).

## Como rodar o projeto

### 1. Pr�-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL](https://www.postgresql.org/download/)
- [DotNet CLI](https://learn.microsoft.com/pt-br/dotnet/core/tools/)
- (Opcional) [Visual Studio 2022+](https://visualstudio.microsoft.com/pt-br/)

### 2. Clone o reposit�rio

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

> **Importante:** N�o suba o `.env` para o GitHub!

### 4. Restaure os pacotes

```
dotnet restore
```

### 5. Crie o banco e as tabelas

```
dotnet ef database update --project ApiAgendamento/ApiAgendamento.csproj
```

### 6. Rode a aplica��o

```
dotnet run --project ApiAgendamento/ApiAgendamento.csproj
```

A API estar� dispon�vel em `https://localhost:5001` ou `http://localhost:5000`.

## Endpoints principais

- **Autentica��o:**  
  - `POST /api/auth/register` � Cadastro de usu�rio (m�dico ou paciente)
  - `POST /api/auth/login` � Login (retorna JWT)
- **M�dico:**  
  - `GET /api/medico/{medicoId}/horarios` � Listar hor�rios do m�dico
  - `POST /api/medico/{medicoId}/horarios` � Adicionar hor�rio dispon�vel
- **Paciente:**  
  - `GET /api/paciente/meus-agendamentos` � Listar agendamentos do paciente autenticado
- **Agendamento:**  
  - `POST /api/agendamento` � Criar agendamento (autenticado)

> Todos os endpoints (exceto login e registro) exigem JWT no header:  
> `Authorization: Bearer <seu_token>`

## Testes

- Estrutura para testes unit�rios e de integra��o em `/tests` (em produ��o).

## Tecnologias

- .NET 8
- Entity Framework Core
- PostgreSQL
- JWT Authentication
- BCrypt.Net (hash de senha)
- DotNetEnv (vari�veis de ambiente)