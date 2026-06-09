# Sprint 57: Roteamento Híbrido (REST, SignalR e LSP)

## Objetivo da Sprint
Unir todos os pontos das Fases 1, 3 e 6 num único orquestrador. A extensão VS Code usará as três portas de comunicação do .NET 8, e precisamos garantir que essas instâncias conversem perfeitamente na mesma Injeção de Dependência, sem travar o processador do usuário.

## Tarefas de Desenvolvimento

### 1. Refatoração do `Program.cs` (.NET 8)
- Configurar o Pipeline ASP.NET de forma híbrida:
  - Rotas `/api/auth` e `/api/projects` mapeadas como Minimal APIs REST.
  - Rota `/agent-hub` ativada via `app.MapHub<ChatAgentHub>`.
  - Instanciação Singleton ou Transient do Language Server que escuta as portas I/O do LSP independentemente do servidor HTTP Kestrel.

### 2. O SemaphoreSlim Global de Inferência (Lock Management)
- Como temos Autocompletes vindo via LSP e Chats complexos vindo via SignalR, os dois podem atacar a GPU e a VRAM ao mesmo tempo.
- Criar a abstração `GpuLockManager` que controla o acesso às instâncias dos modelos ONNX (`Qwen-1.5B` e `Qwen-27B`).
- Garantir que se a IA Mestra estiver gerando código no 27B e o VS Code pedir um *Autocomplete*, a requisição de LSP aguarde num bloco assíncrono para evitar Memory Fault ou Error 500 no CUDA.

## Testes e Validações
- Fazer Stress Test Local Híbrido. Um console disparando requests HTTP POST (registro no postgres), outro disparando WebSocket e o terceiro consumindo JSON-RPC simulando o LSP. Se o C# der qualquer aviso de Concurrency Conflict (ex: DbContext sendo usado por 2 threads), refatorar os Scoped Services das Controllers.

## Critérios de Aceite (DoD)
1. Arquitetura unificada. O mesmo backend .NET atende a Automação Inteligente de Código, O chat lateral e a Web API do ecossistema simultaneamente na porta configurada, otimizando muito a memória RAM de hospedagem.
