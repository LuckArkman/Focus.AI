# Sprint 66: Contexto Ativo da IDE (Workspace Synchronization)

## Objetivo da Sprint
Capturar dados em tempo real da área de trabalho ativa do usuário (arquivo selecionado, linhas de código em torno do cursor e trecho de código selecionado) para anexar automaticamente às requisições do chat da IA.

## Tarefas de Desenvolvimento

### 1. Captura de Contexto do Editor Ativo
- Implementar funções auxiliares que acessam o `vscode.window.activeTextEditor`.
- Extrair metadados estruturados do arquivo em foco: caminho absoluto do arquivo no workspace, linguagem configurada no editor, conteúdo de texto total e as coordenadas exatas da seleção de linhas atual do desenvolvedor.

### 2. Painel de Contexto Anexado na Webview
- Desenvolver um componente de UI flutuante no topo ou base da caixa de texto do Chat na Webview para mostrar de forma visual e amigável qual bloco de código está "anexado" à conversa (ex: "📎 UserService.ts - Linhas 12-25").
- Permitir que o usuário "desanexe" o código ativo ou adicione arquivos adicionais clicando no painel.

### 3. Payload de Contexto do Prompt (Ponte de Dados)
- Estruturar a payload da mensagem do SignalR de modo que envie não apenas a string digitada pelo usuário, mas um objeto de contexto contendo o snippet de código selecionado, permitindo que a pipeline do RAG Híbrido (Sprint 48) no C# utilize esses dados com prioridade máxima.

## Testes e Validações
- **Teste de Seleção Dinâmica:** Selecionar uma função no editor do VS Code, digitar no chat da IA "Explique esta função" e certificar-se de que a resposta analisa precisamente as linhas selecionadas.
- **Teste sem Seleção:** Enviar uma pergunta sem arquivos abertos e certificar-se de que a extensão não lança exceções de ponteiro nulo (`null reference`).

## Critérios de Aceite (DoD)
1. Arquivos abertos e trechos selecionados de código são coletados e expostos programaticamente de forma reativa.
2. A Webview exibe corretamente o contexto dinâmico anexado a cada pergunta do desenvolvedor.
