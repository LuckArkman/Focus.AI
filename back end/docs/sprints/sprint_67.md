# Sprint 67: Quick Fixes e Code Actions Integrados à IDE

## Objetivo da Sprint
Implementar o provedor de Code Actions do VS Code para exibir comandos rápidos de inteligência artificial (como refatorações e correções automáticas de bugs) diretamente no menu da lâmpada amarela (Quick Fix).

## Tarefas de Desenvolvimento

### 1. Criação do CodeActionProvider
- Registrar um provedor nativo `vscode.languages.registerCodeActionsProvider` para todas as linguagens suportadas pela ferramenta.
- Implementar a função `provideCodeActions` para verificar diagnósticos de compiladores locais ativos ou erros repassados pelo LSP do Focus.AI.

### 2. Mapeamento de Correções Rápidas (Auto-Fix)
- Gerar objetos `vscode.CodeAction` do tipo `QuickFix` ou `Refactor` quando o cursor estiver posicionado sobre uma linha de erro.
- Definir ações que, ao serem clicadas pelo usuário, chamam o backend .NET 8 solicitando uma correção localizada via IA para o trecho de código correspondente.

### 3. Execução de WorkspaceEdit (Mutação de Código)
- Desenvolver a aplicação do patch de código na extensão: receber a sugestão de correção em formato de diff/texto do backend e aplicá-la diretamente no editor ativo por meio da API `vscode.workspace.applyEdit`.
- Adicionar suporte para desfazer (`Ctrl+Z`) a alteração automaticamente preservando o histórico do editor.

## Testes e Validações
- **Teste da Lâmpada de Ação:** Inserir um código propositalmente errático no editor, passar o cursor por cima e validar se a lâmpada do VS Code aparece com a opção: "Focus.AI: Corrigir Erro de Sintaxe".
- **Teste de Mutação de Arquivo:** Acionar a ação rápida de correção e verificar se o código é modificado automaticamente na tela mantendo a indentação original.

## Critérios de Aceite (DoD)
1. Ações Rápidas (Code Actions) integradas aos menus contextuais do VS Code.
2. Aplicação de refatorações de código no editor do usuário funcionando de forma limpa via `WorkspaceEdit`.
