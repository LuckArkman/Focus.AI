# Sprint 58: Previsão de Falhas no Agente (Code Completion Verifier)

## Objetivo da Sprint
Impedir a "Alucinação" sintática. Se o LSP enviar ao VS Code um autocomplete com falta de parênteses (ex: gerado quebrado pelo ONNX), o projeto do usuário fica com erros instantâneos. Vamos adicionar uma camada de checagem.

## Tarefas de Desenvolvimento

### 1. Verificador de Sintaxe com Tree-sitter em Voô
- Quando a `InlineCompletionHandler` for prever o bloco, nós juntamos o Prefixo + Resultado Gerado + Sufixo em RAM em C#.
- Jogamos essa nova string para o parser do Tree-sitter (Sprint 41).
- Se a árvore criada acusar erro grave `(ERROR)` e o arquivo anterior (o do usuário antes do agente opinar) não tinha, significa que **A IA corrompeu a sintaxe**.
- Ação do backend: Descarta silenciosamente o retorno do autocomplete, fingindo que a IA não sugeriu nada, salvando a tela do desenvolvedor de receber lixo.

## Testes e Validações
- Passar um prefix e suffix. Fazer o mock do LLM devolver algo que fecha errado as chaves `{{ }`. O Tree-sitter retornará erro na árvore temporária. A ferramenta deve cancelar e retornar "No Completions".

## Critérios de Aceite (DoD)
1. Filtro estrito de qualidade para o autocomplete via análise de árvore da string imaginária prévia.
