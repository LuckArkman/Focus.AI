# Sprint 07: Integração com Qdrant (Vetores de Contexto da IA)

## Objetivo da Sprint
O Qdrant servirá como a "Memória de Longo Prazo" do agente. Esta sprint visa integrar o cliente gRPC oficial do Qdrant para criar as *Collections*, gerar e persistir os vetores de contexto das conversas e implementar os filtros de segurança Multi-Tenant (baseados no *User ID*).

## Tarefas de Desenvolvimento

### 1. Configuração do Cliente Qdrant (gRPC)
- Adicionar o pacote NuGet `Qdrant.Client` ao projeto `Infrastructure`.
- Registrar a conexão Singleton no `Program.cs` (`new QdrantClient("localhost", 6334)` - lembrando que a 6334 é a porta gRPC, não a REST, garantindo máxima velocidade em C#).

### 2. Criação das Collections Dinâmicas
- Desenvolver um serviço de *bootstrap* (que roda no startup da API) chamado `QdrantInitializer`.
- Se a coleção `UserInteractions` não existir, o sistema deve criá-la.
- Definir as métricas da coleção:
  - Tamanho do vetor: **Definir conforme o modelo de embedding local que será usado** (ex: 384 dimensões para `all-MiniLM-L6-v2`, ou 1536 se fosse o padrão Qwen).
  - Distância Métrica: `Cosine` (Similaridade de Cosseno).

### 3. Implementação do Repositório Vetorial
- Criar a interface `IVectorMemoryRepository` na camada Application.
- Implementar métodos:
  - `UpsertInteractionVectorAsync(Guid userId, string sessionId, string rawText, float[] vector)`
  - `SearchSimilarContextAsync(Guid userId, float[] queryVector, int limit = 5)`

### 4. Filtros de Payload Estritos (Multi-Tenancy)
- O Qdrant usa o conceito de *Payload* (JSON acoplado ao vetor).
- No método de busca (`SearchSimilarContextAsync`), codificar uma restrição obrigatória e inquebrável: O `Condition.MatchKeyword` para a chave `user_id` deve **obrigatoriamente** corresponder ao ID do usuário autenticado no token JWT. Se isso falhar, o sistema misturará o código fonte de diferentes desenvolvedores.

## Testes e Validações
- **Testes Unitários:** O filtro de payload deve ser blindado. Garantir através de testes unitários com Mock que a query enviada para o Qdrant SEMPRE inclua a cláusula WHERE User = CurrentUser.
- **Integração Básica:** Subir um container Qdrant, inserir 3 vetores de teste (mockados com array float aleatório), e tentar fazer uma busca utilizando filtro de payload, recebendo a resposta certa.

## Critérios de Aceite (DoD)
1. Conexão gRPC nativa com o Qdrant estabelecida.
2. Ao recuperar memórias vetoriais, os dados são filtrados *hard-coded* pelo ID do usuário logado.
3. Coleção padrão de interações é criada caso o banco do Docker seja resetado (volumes excluídos).
