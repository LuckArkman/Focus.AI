# Sprint 15: Integração entre Embeddings, Chat e Qdrant (Memória Contínua)

## Objetivo da Sprint
O usuário enviou uma mensagem para a IA. O texto foi salvo no MongoDB. Agora, precisamos automatizar o envio dessa memória para o Qdrant, utilizando o nosso gerador de embeddings local (construído na Sprint 14).

## Tarefas de Desenvolvimento

### 1. Observer ou Background Task (Event-Driven)
- Uma boa prática é não travar o loop de Request-Response de envio de mensagem enquanto vetoriza as coisas.
- Mapear a camada de Domain Events do MediatR. Ao persistir a mensagem no MongoDB, disparar um evento na memória `MessageAddedEvent`.

### 2. O Handler em Background do RAG
- Criar a classe `PersistVectorMemoryHandler` que escuta `MessageAddedEvent`.
- Ao capturar o evento, o fluxo deve:
  1. Pegar a string do chat.
  2. Mandar pro `LocalEmbeddingGenerator` (Sprint 14), aguardando o `float[]`.
  3. Instanciar a classe `IVectorMemoryRepository` (criada na Sprint 07).
  4. Disparar a chamada gRPC que joga esse vetor no Qdrant, com o *Payload* contendo o `SessionId`, `UserId` e o `Text`.

## Testes e Validações
- **Teste Unitário com Mock duplo:** Validar se, dado o acionamento do evento, as funções do gerador ONNX e do serviço do Qdrant foram chamadas exatamente 1 vez cada.
- **Teste Isolado:** Bater na porta Web do Qdrant UI local (normalmente `http://localhost:6333/dashboard`) após salvar algumas mensagens via API HTTP, e observar visualmente no painel se os novos pontos vetorizados estão surgindo com os metadados JSON corretos.

## Critérios de Aceite (DoD)
1. A gravação do vetor de uma conversa ocorre *under-the-hood* (em background) via padrão *Publisher/Subscriber* do MediatR e não causa aumento de lentidão pro usuário no HTTP.
2. O Qdrant recebe perfeitamente a coleção vetorial pareada ao formato exigido (mesmo número de dimensões criadas na Collection).
