# Guia de integração do frontend - Bolão da Copa

Este documento define como o frontend deve integrar com a API de bolão para garantir comportamento consistente.

## Visão geral do módulo

Funcionalidades disponíveis no backend:

- criação de bolão por usuário autenticado
- entrada em bolão via código/link de convite
- cadastro e edição de palpite por jogo
- trava de edição a 5 minutos do início do jogo
- ranking automático por pontuação acumulada

## Autenticação e headers obrigatórios

### JWT

Todos os endpoints de bolão exigem usuário autenticado:

- header: `Authorization: Bearer <token>`

O token é retornado no login:

- `POST /api/auth/login`

### Frontend key (produção)

Se o backend estiver configurado com `FRONTEND_API_KEY`, o frontend deve enviar:

- header: `X-Frontend-Key: <chave>`

Recomendação: configurar esse header no client HTTP global (axios/fetch wrapper).

## Regras de pontuação

Por jogo:

- acertou vencedor/empate: `+10`
- acertou diferença de gols: `+5`
- acertou gols do mandante: `+2`
- acertou gols do visitante: `+2`
- acertou placar exato (bônus): `+10`

Máximo por partida: `29 pontos`.

## Regra de bloqueio de edição

O backend bloqueia edição de palpite quando faltam menos de 5 minutos para o jogo:

- janela de edição: até `kickoffAt - 5 minutos`
- após isso, `PUT /predictions/...` retorna `400`

A API de jogos já devolve `canEdit` por partida para facilitar UI.

## Endpoints e contratos

## 1) Cadastro e login

### `POST /api/auth/register`

Cria usuário padrão (`role = User`).

Request:

```json
{
  "username": "joao",
  "password": "123456"
}
```

Response `201`:

```json
{
  "id": "a1b2c3d4-....",
  "username": "joao",
  "role": "User"
}
```

### `POST /api/auth/login`

Request:

```json
{
  "username": "joao",
  "password": "123456"
}
```

Response `200`:

```json
{
  "token": "<jwt>",
  "expiresAt": "2026-06-10T01:00:00Z",
  "username": "joao",
  "role": "User"
}
```

## 2) Criar bolão

### `POST /api/pools`

Request:

```json
{
  "name": "Bolao da Firma",
  "tournamentId": "11111111-2222-3333-4444-555555555555"
}
```

Importante:

- `tournamentId` **não é fixo** e pode variar por ambiente/banco.
- o frontend deve buscar esse valor em `GET /api/tournament`, no campo `tournament.id`.

Response `201`:

```json
{
  "id": "d6f4....",
  "name": "Bolao da Firma",
  "inviteCode": "A1B2C3D4",
  "tournamentId": "11111111-2222-3333-4444-555555555555",
  "joinPath": "/join/A1B2C3D4"
}
```

Uso no frontend:

- exibir botão de compartilhar: `https://seu-frontend.com${joinPath}`
- guardar `poolId` para carregar jogos e ranking

## 3) Entrar no bolão por convite

### `POST /api/pools/join`

Request:

```json
{
  "inviteCode": "A1B2C3D4"
}
```

Response `200`:

```json
{
  "id": "d6f4....",
  "name": "Bolao da Firma",
  "inviteCode": "A1B2C3D4",
  "tournamentId": "11111111-2222-3333-4444-555555555555",
  "joinPath": "/join/A1B2C3D4"
}
```

## 4) Listar jogos do bolão e palpites do usuário logado

### `GET /api/pools/{poolId}/matches`

Response `200`:

```json
[
  {
    "matchId": "0a0a....",
    "matchCode": "A1",
    "kickoffAt": "2026-06-20T18:00:00Z",
    "status": "Scheduled",
    "homeTeam": "México",
    "homeTeamFlagCode": "🇲🇽",
    "awayTeam": "África do Sul",
    "awayTeamFlagCode": "🇿🇦",
    "actualHomeGoals": null,
    "actualAwayGoals": null,
    "predictedHomeGoals": 2,
    "predictedAwayGoals": 1,
    "canEdit": true
  }
]
```

Regras de UI:

- se `canEdit = false`, desabilitar botões `+` e `-` e CTA de salvar
- se `predictedHomeGoals`/`predictedAwayGoals` for `null`, mostrar estado "sem palpite"
- para jogos encerrados, exibir também placar oficial (`actualHomeGoals`, `actualAwayGoals`)

## 5) Criar/editar palpite

### `PUT /api/pools/{poolId}/predictions/{matchId}`

Request:

```json
{
  "homeGoals": 2,
  "awayGoals": 1
}
```

Response `200`:

```json
{
  "id": "9c9c....",
  "poolId": "d6f4....",
  "matchId": "0a0a....",
  "homeGoals": 2,
  "awayGoals": 1,
  "updatedAt": "2026-06-09T16:20:00Z",
  "lockAt": "2026-06-20T17:55:00Z"
}
```

Validações importantes:

- gols negativos retornam `400`
- tentativa após o bloqueio de 5 minutos retorna `400`
- jogo fora do torneio do bolão retorna `400`

## 6) Ranking do bolão

### `GET /api/pools/{poolId}/ranking`

Response `200`:

```json
[
  {
    "userId": "aaaa....",
    "username": "joao",
    "points": 68,
    "exactScores": 2
  },
  {
    "userId": "bbbb....",
    "username": "maria",
    "points": 63,
    "exactScores": 1
  }
]
```

Ordenação já vem pronta do backend:

1. maior `points`
2. maior `exactScores`
3. `username` em ordem alfabética

## Erros esperados por endpoint

- `401 Unauthorized`: token ausente/inválido
- `403 Forbidden`: usuário não pertence ao bolão
- `404 Not Found`: bolão/jogo/código de convite não existe
- `409 Conflict`: username já cadastrado no registro
- `400 Bad Request`: payload inválido, bolão/jogo incompatível, palpite fora da janela

## Fluxo de telas recomendado

1. **Autenticação**
   - registro ou login
   - salvar token (preferência: storage seguro)
2. **Home do bolão**
   - criar bolão ou informar código de convite
3. **Detalhe do bolão**
   - aba jogos (palpites)
   - aba ranking
4. **Modal de palpite**
   - contador de gols mandante/visitante
   - card com regras de pontuação
   - botão confirmar habilitado apenas quando `canEdit = true`

## Contratos TypeScript sugeridos

```ts
export type PoolResponse = {
  id: string;
  name: string;
  inviteCode: string;
  tournamentId: string;
  joinPath: string;
};

export type PoolMatchResponse = {
  matchId: string;
  matchCode: string;
  kickoffAt: string;
  status: string;
  homeTeam: string;
  homeTeamFlagCode: string;
  awayTeam: string;
  awayTeamFlagCode: string;
  actualHomeGoals: number | null;
  actualAwayGoals: number | null;
  predictedHomeGoals: number | null;
  predictedAwayGoals: number | null;
  canEdit: boolean;
};

export type PoolRankingItem = {
  userId: string;
  username: string;
  points: number;
  exactScores: number;
};
```

## Checklist de implementação frontend

- [ ] incluir `Authorization` em chamadas autenticadas
- [ ] incluir `X-Frontend-Key` quando necessário
- [ ] tratar `canEdit` para bloquear edição na UI
- [ ] exibir mensagens amigáveis para `400/401/403/404/409`
- [ ] atualizar lista de jogos e ranking após salvar palpite
- [ ] implementar deep link de convite usando `joinPath`
