# Sprint 26: Roteamento Inteligente (Model Routing 1.5B vs 27B)

## Objetivo da Sprint
Implementar a inteligência arquitetural descrita no início do projeto. Nem toda pergunta precisa acordar o gigantesco Qwen3 27B. Vamos construir um classificador que intercepta o pedido do VS Code e o envia para o Qwen3 1.5B caso seja uma tarefa boba (ex: preenchimento de comentário) ou direcione pro 27B se for algo denso (ex: "Refatore esta classe").

## Tarefas de Desenvolvimento

### 1. Classificador Heurístico e Semântico
- Criar a classe `ModelRouterService` no Application.
- A função do VS Code (Extensão) deve informar no JSON um campo `IntentType` (Autocomplete, Chat, Refactor, Agent_Module3).
- **Hard-Routing:** Se `IntentType == Autocomplete`, o backend roteia *imediatamente* pro cliente ONNX de 1.5B em RAM.
- **Soft-Routing:** Se `IntentType == Chat`, passar a frase para o modelo 1.5B internamente no C# fazer uma classificação rápida: *"A tarefa requer análise sistêmica ou é trivial?"*. Se trivial, ele mesmo responde. Se profunda, ele repassa para a sessão do 27B (CUDA).

### 2. Multi-Factory no .NET
- O `IServiceCollection` deve ser capaz de prover `IChatClient("Small")` e `IChatClient("Large")` usando a interface nova de Factory do .NET 8 (Keyed Services).
- Exemplo: `services.AddKeyedScoped<IChatClient, OnnxChatClient>("small_qwen")`.

## Testes e Validações
- **Teste Unitário com Keyed Services:** Garantir que injetar o `[FromKeyedServices("small_qwen")]` não instancie o arquivo de 20GB.
- Mapear logs de "Router Decision: Routed to 1.5B Model".

## Critérios de Aceite (DoD)
1. Roteador implementado economizando agressivamente o consumo de GPU.
2. O Autocomplete da extensão tem tempo de resposta inferior a 50ms pois só bate no 1.5B.
