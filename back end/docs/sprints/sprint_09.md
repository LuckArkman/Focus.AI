# Sprint 09: Setup do SignalR para Comunicação WebSockets e Token Stream

## Objetivo da Sprint
Preparar a via expressa pela qual o modelo de IA (Módulo 1 / Backend) vai "digitar" o código e as respostas em tempo real para o usuário no Módulo 2 (VS Code Extension). A comunicação tradicional via REST (Request-Response) é inadequada para *stream* fluido de inteligência artificial. Para isso usaremos WebSockets com ASP.NET SignalR.

## Tarefas de Desenvolvimento

### 1. Instalação e Configuração Base do SignalR
- Sinalizar a ativação dos serviços WebSockets no backend: no `Program.cs`, usar `builder.Services.AddSignalR()`.
- O SignalR do .NET 8 já é otimizado para lidar com altos volumes de dados minúsculos (os tokens gerados pelo LLM).

### 2. Criação do Hub de Comunicação da Inteligência Artificial
- Criar o `ChatAgentHub` (herda de `Hub`) no projeto da API.
- Definir os métodos estritos que a interface/extensão poderão invocar, por exemplo: `Task SendPromptToAgent(string prompt, string sessionId)`.

### 3. Conexão Segura com JWT
- O SignalR não funciona nativamente mandando Header de `Authorization: Bearer` via navegador de forma simples (eles mandam na Query String nos clientes JS, mas a extensão do VS Code pode mandar header).
- Adicionar no pipeline do JWT (`AddJwtBearer()`) um evento `OnMessageReceived` para verificar se a requisição está vindo para a rota `/agent-hub` e resgatar o token do parâmetro `access_token` caso a conexão WebSocket passe por ele.
- Anotar a classe `ChatAgentHub` com `[Authorize]` para que hackers locais na rede não consumam processamento de GPU anonimamente.

### 4. Controle de Ocupação de GPU
- O *Hub* do SignalR deve ter capacidade de mapear o status do usuário conectado e sua requisição simultânea, ou seja, impedir que um usuário envie um Request via chat de 3 perguntas pesadas ao mesmo tempo travando o Qwen3 ONNX para si. (Definir flags de concorrência na conexão).

## Testes e Validações
- **Console App de Teste:** Criar um projeto simples de Console Application (C# puro) com o pacote `Microsoft.AspNetCore.SignalR.Client`. Realizar login (chamando a API REST do JWT), pegar o token, instanciar um `HubConnection`, conectar no backend e disparar a mensagem *SendPromptToAgent*. O backend deve responder via evento de WebSocket.

## Critérios de Aceite (DoD)
1. A rota base `/agent-hub` está exposta e respondendo protocolos WSS (WebSocket Secure).
2. Clientes tentarem conexão sem Token JWT ativo recebem recusa imediata de *Handshake*.
3. O servidor SignalR consegue emitir mensagens avulsas (eventos em background) aos clientes autenticados.
