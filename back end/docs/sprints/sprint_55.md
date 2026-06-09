# Sprint 55: Diagnostics, Code Actions e Auto-Refactor

## Objetivo da Sprint
Além do "Ghost Text", as IDEs possuem linhas sinuosas azuis ou vermelhas indicando bugs. Quando você clica nelas, aparece a "Lâmpada amarela" com dicas. Faremos a IA injetar sugestões diretamente no editor através dessa interface do LSP.

## Tarefas de Desenvolvimento

### 1. Code Actions Handler (Lâmpada)
- Criar a classe `CodeActionHandler : ICodeActionHandler`.
- Quando o usuário destacar uma linha de código, apertar "Ctrl+." (Quick Fix), o VS Code pergunta ao servidor: *"Tem alguma dica pra esse bloco?"*.

### 2. Acionamento do Agente de Refatoração
- Ao receber o bloco destacado, o C# empacota o texto e a intenção e envia como prompt rápido para o Qwen3 27B (Aqui acionamos o modelo grande, porque refatorar exige precisão lógica profunda).
- Prompt gerado pelo Backend: *"Encontre bugs ou sugira 2 melhorias de design de código (como SOLID) para este trecho: {bloco}."*

### 3. Publicação dos Diagnostics (Linhas Vermelhas)
- O Agente de IA Módulo 3 pode, por livre e espontânea vontade, pedir para sublinhar uma variável do usuário.
- O C# usará a interface de envio `ILanguageServerFacade.TextDocument.PublishDiagnostics` mandando um Array contendo a `Range` geométrica no texto (linha x à y) e a mensagem gerada pela IA: *"Variável nunca usada e com alto acoplamento."*

## Testes e Validações
- Programar um retorno Fixo Mockado para a CodeAction. Injetar num cliente falso um range de linhas e esperar que a API devolva o Array contendo o título: "Refatorar usando Factory Pattern", atestando o mapeamento.

## Critérios de Aceite (DoD)
1. A IA consegue se expressar de forma proativa injetando recomendações em pontos específicos da UI do Visual Studio Code.
2. Suporte completo a blocos selecionados.
