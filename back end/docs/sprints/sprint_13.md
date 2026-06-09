# Sprint 13: Sincronização de Histórico de Conversas (MongoDB + CQRS)

## Objetivo da Sprint
O painel lateral (Web ou Extensão do VS Code) precisa carregar conversas anteriores de um mesmo projeto. Esta sprint foca em ligar a camada web aos repositórios do MongoDB construídos na Fase 1, usando MediatR.

## Tarefas de Desenvolvimento

### 1. Casos de Uso do MongoDB (Commands)
- Criar `CreateChatSessionCommand`: Inicializa uma nova *Thread* na collection `ChatSessions` com o `ProjectId`.
- Criar `AddMessageCommand`: Insere na collection `ChatMessages` o log de quem enviou (User) e o payload do texto. Esse command deverá ser assíncrono para não travar a UI.

### 2. Casos de Uso de Recuperação (Queries)
- Criar `GetChatSessionsByProjectQuery`: Retorna uma lista de todas as conversas ocorridas em um `ProjectId` específico.
- Criar `GetMessagesBySessionQuery`: Retorna o JSON completo daquela conversa, vital para o front-end repintar a tela com o histórico.

### 3. Endpoints de Histórico
- Adicionar rotas `/api/projects/{projectId}/sessions` e `/api/sessions/{sessionId}/messages`.
- Garantir paginação ou limite de busca (ex: pegar as últimas 50 mensagens para evitar overhead na rede caso a conversa seja gigantesca).

## Testes e Validações
- **Teste de Carga de Leitura:** Teste de integração inserindo 100 mensagens numa sessão via Repository, chamando a Query e verificando se o retorno ocorre em menos de 100ms (vantagem da modelagem em documentos sem Join no Mongo).

## Critérios de Aceite (DoD)
1. Ao carregar um projeto no VS Code, a extensão conseguirá ler e carregar o ID da última conversa não finalizada puxando os dados MongoDB via API.
2. Paginação embutida nas mensagens de chat (MongoDB `Skip` e `Limit` implementados de forma otimizada).
