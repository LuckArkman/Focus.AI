# Sprint 25: Geração de Tokens em Streaming (IAsyncEnumerable)

## Objetivo da Sprint
IAs como o Qwen não cospem um texto inteiro de uma vez; elas geram palavra por palavra (token a token). Para dar a sensação de velocidade na extensão do VS Code, precisamos implementar o algoritmo de "Text Generation Stream", conectando os *yields* da IA diretamente no SignalR construído na Sprint 09.

## Tarefas de Desenvolvimento

### 1. Loop de Geração em C# (Greedy Search / Sampling)
- No `OnnxChatClient`, implementar o método `CompleteStreamingAsync(ChatOptions options)`.
- O método deve rodar um laço `while` até gerar o token `<|endoftext|>` ou estourar o `max_tokens`.
- Em cada loop:
  1. O modelo ONNX roda os tensores de entrada.
  2. O vetor de saída é computado usando Top-K / Top-P (Temperature Sampling implementado manualmente em C# manipulando o Span numérico dos logits).
  3. O Token ID gerado é enviado para o *Tokenizer* converter para pedaço de string.
  4. O pedaço de string (ex: " function") é retornado via `yield return new ChatCompletionChunk { Text = " function" }`.
  5. O Token ID é re-anexado aos tensores de entrada para prever a próxima palavra (Auto-regressivo).

### 2. Conexão do Streaming com SignalR
- O `ChatAgentHub` (Sprint 09) agora utilizará `IAsyncEnumerable<ChatCompletionChunk>` que é nativamente compreendido pelo SignalR do .NET 8 como um *Server-to-Client Streaming*.
- O frontend no VS Code consumirá esse pipe assíncrono e repintará a tela letra a letra.

## Testes e Validações
- Usar o Console App de Teste do SignalR (Sprint 09). Chamar a geração do stream de um texto longo ("Conte-me uma história enorme"). Garantir que o C# recebe fragmentos individuais a cada décimo de segundo em vez de travar por 10 segundos esperando tudo terminar.

## Critérios de Aceite (DoD)
1. Interface `IAsyncEnumerable` utilizada para "yieldar" pedaços de texto instantaneamente.
2. O usuário na extensão do VS Code vê o código sendo "digitado" pela IA, reduzindo o *Time to First Token* (TTFT) para milissegundos.
