# Giro da Copa

Aplicação para acompanhar jogos, classificação e chave da Copa do Mundo — frontend React (Vite) e API ASP.NET Core com PostgreSQL.

## Estrutura

```
giro-da-copa/
├── frontEnd/          # React + Vite (deploy: Netlify)
├── src/
│   ├── GiroDaCopa.Api/           # API REST
│   ├── GiroDaCopa.Application/   # Casos de uso (MediatR)
│   ├── GiroDaCopa.Domain/
│   ├── GiroDaCopa.Infrastructure/
│   └── GiroDaCopa.Persistence/   # EF Core + PostgreSQL
├── Dockerfile         # Imagem da API para Render
├── docker-compose.yml # API + Postgres local
├── render.yaml        # Blueprint Render (API + DB)
└── netlify.toml       # Deploy do frontend (quando presente)
```

## Pré-requisitos

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [Node.js 22+](https://nodejs.org/)
- PostgreSQL 15+

## Configuração local

### 1. Segredos do backend

**Nunca commite senhas, chaves JWT ou connection strings reais.**

O `appsettings.json` versionado contém apenas a estrutura, sem valores sensíveis.

#### Opção A — arquivo local (recomendado para começar)

```bash
cp src/GiroDaCopa.Api/appsettings.Development.json.example \
   src/GiroDaCopa.Api/appsettings.Development.json
```

Edite `appsettings.Development.json` com suas credenciais. Esse arquivo está no `.gitignore`.

#### Opção B — User Secrets (recomendado para desenvolvimento)

```bash
cd src/GiroDaCopa.Api

dotnet user-secrets set "ConnectionStrings:Default" \
  "Host=localhost;Port=5432;Database=worldcup;Username=postgres;Password=SUA_SENHA"

dotnet user-secrets set "Jwt:Secret" "SUA_CHAVE_JWT_COM_PELO_MENOS_32_CARACTERES"
dotnet user-secrets set "Admin:Password" "SUA_SENHA_ADMIN"
```

Referência completa de chaves: `src/GiroDaCopa.Api/appsettings.example.json`

### 2. Banco de dados

```bash
# Crie o banco worldcup no PostgreSQL, depois suba a API (migrations rodam no startup)
cd src/GiroDaCopa.Api
dotnet run
```

API local: `http://localhost:5013`  
Swagger (somente Development): `http://localhost:5013/swagger`

### 3. Frontend

```bash
cd frontEnd
cp .env.example .env.local   # opcional — .env.development já cobre o dev
npm install
npm run dev
```

App: `http://localhost:3000`

Variáveis do frontend (ver `frontEnd/.env.example`):

| Variável | Descrição |
|---|---|
| `VITE_API_BASE_URL` | URL da API (`/api` em dev com proxy Vite) |
| `VITE_API_PROXY_TARGET` | Backend local para o proxy (`http://localhost:5013`) |

Arquivos `.env`, `.env.local` e `.env.*.local` **não são versionados**.

## Deploy

### Frontend — Netlify

1. Conecte o repositório no Netlify
2. Deixe o `netlify.toml` controlar o build (remova overrides na UI)
3. Em **Site configuration → Environment variables**, defina:

| Variável | Exemplo |
|---|---|
| `VITE_API_BASE_URL` | `https://sua-api.com/api` |

Build: `npm run build` → publica `frontEnd/dist`

### Backend — Render (Docker)

O repositório inclui `Dockerfile`, `render.yaml` e `docker-compose.yml`.

#### Opção A — Blueprint (recomendado)

1. No [Render Dashboard](https://dashboard.render.com) → **New** → **Blueprint**
2. Conecte o repositório — o `render.yaml` cria:
   - **Web Service** `girodacopa-api` (Docker)
   - **PostgreSQL** `girodacopa-db`
3. Após o deploy, em **Environment** do web service, defina:
   - `FRONTEND_URL` = URL do Netlify (ex.: `https://seu-site.netlify.app`)
4. No Netlify, defina:
   - `VITE_API_BASE_URL` = `https://girodacopa-api.onrender.com/api` (sua URL Render + `/api`)

#### Opção B — Web Service manual

| Campo | Valor |
|---|---|
| Runtime | Docker |
| Dockerfile | `./Dockerfile` |
| Health Check Path | `/health` |

#### Variáveis de ambiente (Render)

| Variável | Obrigatória | Descrição |
|---|---|---|
| `DATABASE_URL` | Sim | Injetada automaticamente pelo Postgres Render |
| `Jwt__Secret` | Sim | Chave JWT (mín. 32 caracteres) |
| `Jwt__Issuer` | Sim | `GiroDaCopa` |
| `Jwt__Audience` | Sim | `GiroDaCopa` |
| `Admin__Password` | Sim | Senha do admin (seed na 1ª execução) |
| `Admin__Username` | Não | Padrão: `admin` |
| `FRONTEND_URL` | Recomendado | URL do frontend para CORS |
| `ASPNETCORE_ENVIRONMENT` | Sim | `Production` |

Migrations e seed rodam automaticamente no startup.

#### Testar com Docker localmente

```bash
docker compose up --build
```

API: `http://localhost:5013` · Swagger: `http://localhost:5013/swagger`

### Backend — outros hosts

Configure via variáveis de ambiente:

| Variável de ambiente | Configuração equivalente |
|---|---|
| `ConnectionStrings__Default` ou `DATABASE_URL` | PostgreSQL |
| `Jwt__Secret` | Chave JWT (mín. 32 caracteres) |
| `Jwt__Issuer` | `GiroDaCopa` |
| `Jwt__Audience` | `GiroDaCopa` |
| `Admin__Username` | Usuário admin inicial |
| `Admin__Password` | Senha admin inicial (seed na 1ª execução) |
| `PORT` | Porta HTTP (Render define automaticamente) |

Use senhas fortes e **rotacione** qualquer credencial que tenha sido exposta no histórico do Git.

## Segurança

- Segredos do backend: User Secrets (dev) ou variáveis de ambiente (produção)
- Segredos do frontend: painel Netlify ou `.env.local` local
- Não versionar: `bin/`, `obj/`, `.env*`, `appsettings.Development.json`
- Templates seguros versionados: `appsettings.*.example.json`, `frontEnd/.env.example`
- Painel admin: `/admin` — protegido por JWT

Se `appsettings.json`, `bin/` ou `appsettings.Development.json` já foram commitados com segredos, **rotacione** a senha do admin, a chave JWT e a senha do banco. Depois remova do índice do Git:

```bash
git rm -r --cached src/**/bin src/**/obj 2>/dev/null || true
git rm --cached src/GiroDaCopa.Api/appsettings.Development.json 2>/dev/null || true
```

## Scripts úteis

```bash
# Docker — API + PostgreSQL
docker compose up --build

# Backend local (sem Docker)
cd src/GiroDaCopa.Api && dotnet run

# Frontend (quando a pasta frontEnd existir)
cd frontEnd && npm run dev
cd frontEnd && npm run optimize:assets
```

## Licença

Ver arquivos de licença nos respectivos módulos do projeto.
