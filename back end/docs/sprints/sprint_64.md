# Sprint 64: Provedor de Inline Completions (Ghost Text) no VS Code

## Objetivo da Sprint
Integrar a API de Inline Completions do VS Code com o cliente LSP para renderizar sugestões de código translúcido (Ghost Text) geradas dinamicamente pelo modelo local ONNX.

## Tarefas de Desenvolvimento

### 1. Registro do Inline Completion Provider
- Registrar o `vscode.languages.registerInlineCompletionItemProvider` para todas as linguagens suportadas pela extensão.
- Implementar a função `provideInlineCompletionItems` que intercepta o estado de digitação do editor.

### 2. Implementação de Debounce e Cancelamento
- Adicionar um mecanismo de debounce (atraso de 150ms a 250ms) no disparo da requisição de autocomplete para evitar congestionar a fila de inferência do modelo local ONNX com caracteres incompletos enquanto o usuário digita em alta velocidade.
- Mapear o `vscode.CancellationToken` fornecido pela IDE para sinalizar ao backend C# que ele deve abortar a geração de tokens atual caso o usuário digite um novo caractere antes que a completação termine de ser gerada.

### 3. Mapeamento da Resposta do Servidor LSP
- Enviar a requisição para o servidor LSP pedindo sugestões de completação para a posição atual do cursor.
- Converter a resposta JSON-RPC em objetos do tipo `vscode.InlineCompletionItem`, especificando o intervalo de texto a ser substituído e o texto gerado.

## Testes e Validações
- **Teste de Renderização:** Digitar a assinatura de uma função simples (ex: `function calcularIdade(anoNascimento) {`) em um arquivo JS/TS, parar de digitar e verificar se o corpo da função aparece em cinza translúcido.
- **Teste do Gatilho Tab:** Pressionar a tecla `Tab` e validar se o texto cinza é aceito e inserido de fato no editor de código.

## Critérios de Aceite (DoD)
1. Sugestões de autocomplete local renderizadas na IDE como Ghost Text oficial do VS Code.
2. Interrupções de digitação cancelam requisições pendentes via `CancellationToken`.
