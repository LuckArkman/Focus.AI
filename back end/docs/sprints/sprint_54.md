# Sprint 54: Captura de Contexto FIM (Fill-In-the-Middle)

## Objetivo da Sprint
Para a IA autocompletar bem, ela não pode ler apenas o que está "Acima" do cursor. Se você for preencher o meio de um IF, a IA precisa saber que existe um fechamento de chaves `}` logo abaixo. Essa técnica de formatação se chama FIM (Fill-in-the-Middle).

## Tarefas de Desenvolvimento

### 1. Extração de Prefixo e Sufixo
- No `InlineCompletionHandler`, pegar o buffer do arquivo atual da memória (Sprint 52).
- Dividir a string gigante em duas partes exatas no ponto do cursor (Line/Character).
  - Parte 1: `Prefix` (Texto do início até o cursor).
  - Parte 2: `Suffix` (Texto do cursor até o final do arquivo).

### 2. Formatação do Prompt FIM
- O modelo Qwen3-Coder suporta tokens especiais para FIM, como `<|fim_prefix|>`, `<|fim_middle|>`, e `<|fim_suffix|>`.
- Construir a string concatenada (Prompt FIM):
  `<|fim_prefix|>` + `[Conteúdo do Prefix]` + `<|fim_suffix|>` + `[Conteúdo do Suffix]` + `<|fim_middle|>`
- Quando o modelo for inferir os tokens (rodar o ONNX), ele continuará gerando a string exatamente no *Middle* (meio) tentando ligar o começo com o fim logicamente.

### 3. Poda Sensível do Sufixo
- Se o arquivo for muito grande para os tokens do 1.5B, podar preferencialmente o meio do arquivo para cima (começo muito distante) e o final extremo do Sufixo, preservando fortemente o código em um raio de 100 linhas em volta do cursor.

## Testes e Validações
- Validar se o construtor do Prompt FIM embute os tokens mágicos `<|fim_...|>` exatamente como esperado, utilizando a rotina de um Assert testando a concatenação das strings.

## Critérios de Aceite (DoD)
1. Autocomplete contextualizado com a estrutura que vem depois do cursor, evitando que a IA sugira fechar parênteses que já estão fechados na linha de baixo.
