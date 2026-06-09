# Sprint 63: Estabelecimento da Comunicação LSP via Stdio e SignalR

## Objetivo da Sprint
Configurar o cliente de Language Server Protocol (LSP) na extensão do VS Code para se conectar ao backend C# via stdio, e inicializar um cliente de SignalR para o canal de mensagens em tempo real (chat).

## Tarefas de Desenvolvimento

### 1. Instalação e Setup do Cliente LSP (TypeScript)
- Instalar o pacote `vscode-languageclient` no `package.json` da extensão.
- Configurar as `ServerOptions` para apontar para a entrada e saída padrão (stdio) do processo do backend C# criado na Sprint 61.
- Configurar as `LanguageClientOptions` especificando quais tipos de documentos (ex: `javascript`, `typescript`, `csharp`) serão escutados pelo servidor.

### 2. Inicialização do Cliente SignalR
- Instalar o pacote `@microsoft/signalr` no projeto da extensão.
- Criar um módulo de conexão `SignalRClient` configurado para conectar ao endpoint `/chatHub` do backend C# local.
- Estabelecer a negociação da porta local: ler a porta alocada pelo backend (se dinâmica) e usá-la para compor a URL de conexão do SignalR (ex: `http://localhost:5001/chatHub`).

### 3. Sincronização e Handshake Inicial
- Chamar `client.start()` para iniciar o handshake do LSP com o backend.
- Ligar os handlers de escuta de estado de conexão para notificar o usuário através da barra de status do VS Code se o backend Focus.AI estiver offline ou em processo de inicialização.

## Testes e Validações
- **Teste de Logs LSP:** Acompanhar o console de saída "Focus.AI Language Server" no VS Code e certificar-se de que a mensagem JSON-RPC de inicialização (`initialize`) foi transmitida e o backend respondeu com sucesso indicando suas capacidades.
- **Teste de Handshake SignalR:** Validar via logs no console de depuração se a conexão WebSocket/SignalR atingiu o estado `Connected`.

## Critérios de Aceite (DoD)
1. Handshake do protocolo LSP concluído com sucesso via canais de stdio.
2. SignalR conectado ao backend C# permitindo o tráfego bidirecional de mensagens estruturadas de chat.
