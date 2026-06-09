# Sprint 52: Sincronização de Arquivos (Text Document Sync)

## Objetivo da Sprint
O VS Code avisa ao LSP quando o usuário abre, edita ou fecha um arquivo. O servidor precisa espelhar exatamente o que o usuário está digitando sem depender de ele "Salvar" (`Ctrl+S`) o arquivo no disco, pois a IA deve ler o código em tempo real.

## Tarefas de Desenvolvimento

### 1. Implementação do `ITextDocumentSyncHandler`
- Criar a classe `TextDocumentSyncHandler` implementando as interfaces do OmniSharp para `DidOpen`, `DidChange`, `DidSave` e `DidClose`.

### 2. O Cache de Buffers Sujos (Dirty Buffers)
- O LSP enviará eventos `DidChange` a cada tecla pressionada. O backend NÃO pode salvar isso no disco.
- Criar o `BufferManagerService` (um Singleton contendo um dicionário em RAM: `ConcurrentDictionary<string, string>`).
- Se o usuário digitar "int a =", o LSP atualiza a string em memória do C#. Assim, se o Agente de IA (Sprint 32) quiser ler o arquivo, o `ReadFileTool` não lê do disco, mas lê PRIMEIRO do `BufferManager` para garantir que ele veja os códigos que ainda não foram salvos.

### 3. Sincronização Incremental
- Configurar as *Capabilities* do servidor para responder que ele suporta **Incremental Sync** (receber apenas as letras que mudaram em vez do texto de 3000 linhas a cada tecla digitada).

## Testes e Validações
- Submeter 5 notificações `DidChange` de forma incremental para a mesma URI de documento. Validar via Unit Test se o conteúdo final na memória (`BufferManagerService`) do backend foi montado perfeitamente sem falha de concorrência.

## Critérios de Aceite (DoD)
1. O backend sabe exatamente o que está na tela do usuário em milissegundos.
2. A memória RAM do servidor é protegida limitando o tamanho dos buffers mantidos em memória por arquivo aberto.
