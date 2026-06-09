# Sprint 17: Desenvolvimento da Busca Semântica (RAG Retrieval)

## Objetivo da Sprint
Permitir que a aplicação, ao receber a pergunta do usuário no chat, faça uma busca no Qdrant utilizando Embeddings, extraia as conversas e códigos passados mais relevantes e os empacote em um formato útil que logo em seguida será injetado no prompt secreto do LLM Qwen3.

## Tarefas de Desenvolvimento

### 1. Implementação do RAG Retriever (Application Layer)
- Criar a classe `SemanticRetrieverService`.
- Expor um método público assíncrono chamado `RetrieveRelevantContextAsync(string queryText, Guid userId, string projectId)`.

### 2. O Pipeline de Recuperação
- O método deve seguir a seguinte esteira de montagem:
  1. Converter a `queryText` em vetor chamando o `LocalEmbeddingGenerator`.
  2. Enviar esse vetor e o limite (Top N, ex: Top 5 resultados mais relevantes) para o serviço do Qdrant.
  3. O Qdrant DEVE rodar com os filtros de `userId` e `projectId`.
  4. Mapear o retorno gRPC de volta para uma lista de strings puras extraídas do payload JSON dos vetores recuperados.

### 3. Montagem do Prompt Base
- Criar uma abstração do "System Prompt" padrão que encapsulará os resultados do Retriever.
- Formatar o Output: Juntar as N strings retornadas pelo Qdrant e prefixá-las com tags úteis pro LLM, como:
  ```text
  [Contexto Histórico Extraído da Memória]:
  Fragmento 1: {...}
  Fragmento 2: {...}
  ```

## Testes e Validações
- **Teste Mock de Retrieval:** Mockar um ambiente onde há 50 vetores (simulação) no Qdrant de diferentes usuários. Tentar buscar utilizando a frase "Qual o setup do banco de dados que eu escolhi ontem?" e validar se o Qdrant retornou apenas resultados cujo `userId` bata, e priorizou resultados falando sobre "Postgres, Docker".

## Critérios de Aceite (DoD)
1. O fluxo fim a fim desde o momento em que o usuário digita a pergunta até a extração dos textos do banco de dados vetorial funciona com alta velocidade (<100ms em média).
2. O sistema é robusto o suficiente para devolver string vazia se o score de similaridade (Cosine score retornado pelo Qdrant) for baixo demais, evitando "alucinações" com contexto irrelevante.
