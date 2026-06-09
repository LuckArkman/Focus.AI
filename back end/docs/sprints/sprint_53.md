# Sprint 53: Autocomplete Inteligente (Ghost Text Handler)

## Objetivo da Sprint
Esta é a feature estilo "GitHub Copilot". Enquanto o usuário digita, a IA prevê o restante da função e o exibe em cinza claro na tela.

## Tarefas de Desenvolvimento

### 1. Handler de Inline Completions
- O OmniSharp LSP não suportava nativamente *InlineCompletions* na spec 3.16, mas na 3.17+ sim.
- Criar `InlineCompletionHandler`.
- O método de *Handle* recebe o Arquivo, a Linha Atual (Line) e a Coluna Atual (Character) onde o cursor do usuário está piscando.

### 2. O Gatilho do ONNX (Integração com Sprint 26)
- Ao disparar o Autocomplete, invocar o `ModelRouterService` passando o `IntentType = Autocomplete`.
- O roteador invocará OBRIGATORIAMENTE o modelo super-rápido Qwen3 de 1.5B (carregado em VRAM/RAM). O modelo de 27B demoraria demais (ex: 2 segundos) frustrando o usuário. O objetivo aqui é resposta em < 200ms.

### 3. Controle de Cancelamento (Debounce/Cancellation)
- Como o usuário digita rápido, 10 requisições de autocomplete podem chegar por segundo. O Handler deve monitorar ferozmente o `CancellationToken` do .NET. Se uma nova requisição chegar, as 9 anteriores devem cancelar o processamento no motor ONNX imediatamente para poupar CPU/GPU.

## Testes e Validações
- Teste de Debounce: Disparar programaticamente 3 requisições ao LSP no mesmo milissegundo. Confirmar que apenas a última completou a geração no ONNX e as duas primeiras deram `TaskCanceledException` lidada de forma limpa.

## Critérios de Aceite (DoD)
1. Integração bem-sucedida entre o protocolo LSP e o Modelo de 1.5B em menos de 200 milissegundos.
2. Geração fantasma intermitente cancelada agressivamente se o cursor do usuário mudar de linha.
