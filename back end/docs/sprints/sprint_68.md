# Sprint 68: Configurações da Extensão (User Settings & Customizations)

## Objetivo da Sprint
Criar e expor as opções de configurações da extensão nas preferências do VS Code, permitindo que o usuário customize portas, atalhos, roteamento de modelo e limites de recursos físicos.

## Tarefas de Desenvolvimento

### 1. Definição do Schema de Configurações no package.json
- Declarar o objeto de configurações no nó `contributes.configuration` do manifesto da extensão.
- Adicionar chaves e descrições detalhadas para:
  - `focusai.backend.port` (inteiro, porta de escuta do backend).
  - `focusai.model.routingMode` (enumeração: `Automático (Routing)`, `Apenas 1.5B (Rápido)`, `Apenas 27B (Lento/Preciso)`).
  - `focusai.performance.vramLimitGB` (número, limite máximo de alocação de memória de vídeo).
  - `focusai.features.enableGhostText` (booleano, habilitar/desabilitar autocomplete em tempo real).

### 2. Tratamento Dinâmico de Configurações (`onDidChangeConfiguration`)
- Escutar alterações de configuração na extensão TypeScript utilizando `vscode.workspace.onDidChangeConfiguration`.
- Se a porta do backend ou o roteamento do modelo forem alterados pelo usuário em tempo de execução, notificar o backend C# via chamadas personalizadas de notificação no cliente LSP ou reiniciar a conexão SignalR.

### 3. Registro de Atalhos de Teclado
- Registrar atalhos padrão na seção `contributes.keybindings` do `package.json` para facilitar o acesso de teclado rápido:
  - Atalho para focar/abrir a barra lateral de chat.
  - Atalho para aceitar/descartar sugestões do Ghost Text.

## Testes e Validações
- **Teste de Mudança de Configuração:** Abrir as configurações visuais do VS Code (`Ctrl+,`), pesquisar por "Focus.AI", mudar o roteamento do modelo para "Apenas 1.5B" e certificar-se de que a mudança se reflete no comportamento da IA local sem precisar reiniciar a IDE.
- **Teste de Porta Customizada:** Mudar a porta padrão de comunicação do backend e verificar se o cliente reconecta com sucesso na nova porta configurada.

## Critérios de Aceite (DoD)
1. Painel de configurações visíveis e customizáveis integrado nativamente no VS Code.
2. Atualização dinâmica dos comportamentos da IA e do servidor local disparados reativamente à alteração de configurações.
