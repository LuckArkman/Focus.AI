# Sprint 29: Injeção do RAG e Montagem do Prompt Final

## Objetivo da Sprint
Juntar os mundos da Fase 2 e da Fase 3. O modelo local ONNX está rodando (Sprint 23) e nós temos o motor de busca Qdrant funcionando (Sprint 17). O objetivo aqui é colar o resultado da busca dentro do template que será executado pelo LLM de IA, aplicando as regras de limite da Sprint 28.

## Tarefas de Desenvolvimento

### 1. Estruturação do System Prompt do Focus.AI
- Escrever o System Prompt oculto que guia o agente. Ex: *"You are Focus.AI, an expert coding assistant. Only respond with valid code and short explanations. Use the context provided below to answer."*

### 2. O Template Builder
- Criar `PromptBuilderService`.
- Fluxo Fim a Fim:
  1. A `new_message` chega do SignalR.
  2. Chamamos `SemanticRetrieverService` (Sprint 17). Pega 3 trechos do Qdrant.
  3. Chamamos o `ChatSessionRepository` (Sprint 06). Pega as últimas N conversas do Mongo.
  4. Agrupamos no formato *ChatML* esperado pelo modelo Qwen3:
     ```text
     <|im_start|>system
     [System_Prompt]
     Contexts:
     {RAG_RESULTS}
     <|im_end|>
     <|im_start|>user
     {Old_Msg_1}<|im_end|>
     <|im_start|>assistant
     {Old_Reply_1}<|im_end|>
     <|im_start|>user
     {New_Msg}<|im_end|>
     <|im_start|>assistant
     ```
  5. Passa no `ContextManager` (Sprint 28) garantindo que não extrapolou.

### 3. Execução Integrada
- Esse texto complexo e montado passa pro *Router* (Sprint 26) que decide se o 27B ou 1.5B vai gerar as respostas.

## Testes e Validações
- Enviar uma pergunta complexa. Inspecionar visualmente através do Serilog o bloco de texto gigantesco `Raw_Prompt_Generated` sendo passado pelo C#. Confirmar se as Tags `<|im_start|>` e `<|im_end|>` foram anexadas com zero espaços acidentais (um espaço invisível no lugar errado ali quebra completamente a lógica de parada de um LLM).

## Critérios de Aceite (DoD)
1. A IA absorve conhecimento do banco vetorial perfeitamente no momento em que a pergunta é feita, respondendo baseada em projetos locais e não na imaginação livre dela.
