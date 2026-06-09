# Sprint 69: Sincronização de Histórico de Conversas (Offline-first / Sync)

## Objetivo da Sprint
Desenvolver o gerenciamento offline-first das conversas na extensão do VS Code, armazenando históricos locais e sincronizando-os de forma transparente com o MongoDB central do backend.

## Tarefas de Desenvolvimento

### 1. Camada de Persistência Local na IDE
- Implementar um repositório `ChatHistoryStore` na extensão TypeScript que salva as sessões de chat e mensagens correspondentes no armazenamento persistente do VS Code (`context.globalState` ou `context.workspaceState`).
- Garantir que, ao fechar e reabrir o VS Code, a aba lateral de chat carregue instantaneamente a lista de sessões anteriores diretamente do armazenamento local.

### 2. Protocolo de Sincronização em Lote com MongoDB
- Ao inicializar a extensão e validar a conexão ativa com o backend C#, disparar um processo de conciliação e sincronização em segundo plano.
- Enviar as conversas que foram alteradas ou criadas localmente durante eventuais períodos offline para que o MongoDB no backend salve o histórico atualizado.

### 3. Interface de Gerenciamento de Histórico
- Adicionar botões de ação rápida no cabeçalho do painel de chat da extensão: "Nova Conversa" (que limpa o histórico de chat ativo e inicia uma nova sessão limpa) e "Limpar Histórico Completo".

## Testes e Validações
- **Teste de Persistência:** Escrever uma pergunta no chat lateral do Focus.AI, fechar a IDE do VS Code, reabrir e garantir que a conversa anterior continua listada exatamente como estava.
- **Teste de Sincronização Offline:** Desconectar o backend (matar o servidor .NET), enviar 2 perguntas no chat (elas devem falhar ou aguardar em fila local), reconectar o backend e validar se o histórico é devidamente sincronizado no MongoDB no banco centralizado.

## Critérios de Aceite (DoD)
1. Persistência local robusta utilizando os mecanismos nativos de estado da API do VS Code.
2. Sincronização de conversas e mensagens com o MongoDB executando de forma assíncrona em background.
