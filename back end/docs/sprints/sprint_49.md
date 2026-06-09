# Sprint 49: Proteção e Tratamento de Erros de Sintaxe (Self-Healing de Leitura)

## Objetivo da Sprint
O usuário está programando e digitando. Se o código dele estiver quebrado (faltando ponto e vírgula, erro de sintaxe), o Tree-sitter acusa erro `(ERROR)` e a árvore sintática fica deficiente. O assistente precisa funcionar mesmo no caos.

## Tarefas de Desenvolvimento

### 1. Mapeador de Nós com Erro (Error Nodes)
- Configurar o `AstParserService` para capturar os Error Nodes do Tree-sitter.
- Em vez de falhar e retornar nulo para a IA, o C# empacota a árvore na estrutura possível, isolando o bloco de erro.

### 2. Ferramenta de "Conserto Rápido" Injetada
- Se o C# detecta erro nativo de parsing na leitura do Workspace, ele força no System Prompt um contexto imediato proativo:
  *"Note: O arquivo X possui um erro sintático grosseiro detectado pelo AST perto da linha Y. O desenvolvedor provavelmente não fechou a chave."*
- Isso faz com que a IA, mesmo antes de tentar rodar compilação pesada via Terminal Tool, já bata o olho e acione o `ReplaceFileContentTool` para botar o ponto e vírgula faltante no lugar.

## Testes e Validações
- Teste de Tolerância a Falhas: Passar um arquivo `.cs` completamente quebrado (sem namespaces e sem fechamento de chaves) para o C#. O AST deve extrair o nome do método de qualquer jeito usando heurística relaxada de error-recovery do Tree-sitter.

## Critérios de Aceite (DoD)
1. O backend continua operando 100% lendo código quebrado que não compilaria normalmente no Visual Studio.
2. Identificação preditiva do local exato da falha de sintaxe por leitura geométrica da árvore em C#.
