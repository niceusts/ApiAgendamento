
# Frontend - Sistema de Agendamento Médico

Este é o frontend do sistema de agendamento médico, desenvolvido com [Vue 3](https://vuejs.org/), [Vite](https://vitejs.dev/), [TypeScript](https://www.typescriptlang.org/), [Pinia](https://pinia.vuejs.org/) e [Vue Router](https://router.vuejs.org/).

## Funcionalidades

- Tela de login com autenticação JWT
- Dashboard protegida por autenticação
- Integração com API backend via Axios
- Gerenciamento de estado com Pinia
- Estrutura pronta para expansão de funcionalidades

## Pré-requisitos

- Node.js 20.19+ ou 22.12+
- npm

## Instalação

No diretório `frontend-app`, execute:

```powershell
npm install
```

## Scripts Disponíveis

- `npm run dev` — Inicia o servidor de desenvolvimento (http://localhost:5173)
- `npm run build` — Gera a build de produção
- `npm run preview` — Visualiza a build de produção localmente
- `npm run lint` — Executa o linter e corrige problemas automaticamente
- `npm run format` — Formata o código com Prettier

## Como rodar o projeto

1. Certifique-se de que o backend está rodando em `https://localhost:7056/api`
2. Inicie o frontend:

```powershell
npm run dev
```

3. Acesse [http://localhost:5173](http://localhost:5173) no navegador.

## Estrutura de Pastas

- `src/components` — Componentes reutilizáveis (ex: LoginForm)
- `src/views` — Páginas principais (Login, Dashboard)
- `src/router` — Configuração das rotas e proteção por autenticação
- `src/services` — Serviços de integração com a API (ex: Axios)
- `src/stores` — Gerenciamento de estado global (Pinia)

## Autenticação

- O login é feito via `/auth/login` na API.
- O token JWT é salvo no `localStorage` e usado para proteger rotas.
- Ao deslogar, o token é removido e o usuário é redirecionado para o login.

## Configuração da API

O endpoint padrão da API está definido em `src/services/api.ts`:

```ts
baseURL: 'https://localhost:7056/api'
```
