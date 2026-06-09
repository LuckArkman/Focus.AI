# Sprint 06: Integração da Camada de Persistência com MongoDB

## Objetivo da Sprint
Preparar o banco de dados NoSQL orientado a documentos (MongoDB) para armazenar os logs densos das sessões do usuário com a inteligência artificial. Como o modelo troca mensagens grandes (JSON, código, respostas de tools), usar MongoDB é muito mais performático e flexível do que o PostgreSQL para este fim.

## Tarefas de Desenvolvimento

### 1. Configuração do Driver do MongoDB
- Instalar o pacote NuGet `MongoDB.Driver` no projeto `Infrastructure`.
- Criar a classe de configuração `MongoDbSettings` mapeando as propriedades: `ConnectionString`, `DatabaseName` e os nomes das *Collections* (ex: `ChatSessionsCollection`).
- Injetar no `Program.cs` as configurações usando o padrão *Options* (`services.Configure<MongoDbSettings>(...)`).

### 2. Modelagem Orientada a Documentos (Event Sourcing Textual)
- Criar a entidade de domínio `ChatSession` e `ChatMessage`.
  - `ChatSession`: Contém `Id` (ObjectId), `UserId` (Guid - Ligação com o Postgres), `ProjectId`, `CreatedAt`.
  - `ChatMessage`: Contém `Id`, `SessionId`, `Role` (User, Assistant, System), `Content` (texto ou JSON do código gerado), `TokensUsed` e `Timestamp`.

### 3. Implementação do Contexto MongoDB
- Diferente do Entity Framework, o MongoDB acessa coleções diretamente. Criar a classe `MongoDbContext` que inicializa o `MongoClient` usando as configurações injetadas e expõe as coleções `IMongoCollection<ChatSession>`.

### 4. Criação do Repositório NoSQL
- Criar `IChatSessionRepository` no Domain.
- Implementar `ChatSessionRepository` no Infrastructure.
- Métodos requeridos: 
  - `CreateSessionAsync`
  - `AddMessageToSessionAsync`
  - `GetSessionHistoryAsync` (Recupera as últimas N mensagens de um chat para alimentar o array de mensagens inicial do LLM).

## Testes e Validações
- **Teste de Serialização:** Garantir que o `Guid` do PostgreSQL seja salvo no MongoDB como string ou UUID binário legível para que consultas cruzadas funcionem perfeitamente.
- **Teste de Performance de Inserção:** Validar se o método `AddMessageToSessionAsync` não sobrecarrega a API caso chamado várias vezes por segundo (ex: em respostas de stream particionadas do ONNX).

## Critérios de Aceite (DoD)
1. O repositório consegue gravar sessões no MongoDB local instanciado no Docker.
2. O histórico de uma conversa pode ser recuperado ordenado pela data (`Timestamp`) para ser enviado como contexto para o modelo.
3. Não pode existir dependência forte estrutural (Chaves Estrangeiras) do MongoDB para o PostgreSQL no código.
