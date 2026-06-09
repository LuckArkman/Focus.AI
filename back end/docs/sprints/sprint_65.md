# Sprint 65: Interface do Painel de Chat (Webview) e Comunicação Bidirecional

## Objetivo da Sprint
Desenvolver e integrar o Painel de Chat lateral no VS Code através da API de Webviews, permitindo mensagens e fluxo conversacional dinâmico (streaming) com o backend.

## Tarefas de Desenvolvimento

### 1. Registro do WebviewViewProvider
- Criar a classe `FocusChatViewProvider` que implementa a interface `vscode.WebviewViewProvider`.
- Registrar o provider no ciclo de vida da extensão apontando para o identificador visual definido na área de `views` do `package.json`.

### 2. Implementação da Interface HTML/CSS/JS (Sidebar)
- Construir um layout web limpo e responsivo para o chat utilizando HTML5 e CSS Vanilla.
- Estilizar o painel com variáveis CSS nativas do VS Code (ex: `var(--vscode-editor-background)`, `var(--vscode-button-background)`) para que as cores do chat combinem dinamicamente com o tema ativo do usuário.

### 3. Canal de Comunicação Interno (`postMessage`)
- Estabelecer a troca de mensagens bidirecional:
  - Do Webview para o Extension Host: quando o usuário clica em "Enviar", repassar o texto da mensagem.
  - Do Extension Host para o Webview: enviar os blocos de texto gerados por streaming da IA para renderização progressiva no chat.
- Ligar as mensagens ao cliente SignalR (desenvolvido na Sprint 63) para realizar a ponte de comunicação com o backend em C#.

## Testes e Validações
- **Teste de Envio e Resposta:** Abrir a barra lateral do VS Code, digitar uma pergunta de teste e garantir que a interface de chat mostra a mensagem do usuário e exibe a resposta da IA sendo escrita caractere por caractere (streaming via SignalR).
- **Teste de Cores do Tema:** Mudar o tema do VS Code (de escuro para claro ou vice-versa) e verificar se todos os elementos de texto e fundo da Webview adaptam-se imediatamente e de forma limpa.

## Critérios de Aceite (DoD)
1. Painel de Chat lateral integrado como aba oficial da barra de atividades do VS Code.
2. Renderização em streaming de tokens do modelo local ocorrendo sem travamento da UI da IDE.
