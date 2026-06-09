# Sprint 44: A Tool de Busca de Símbolos (Search Symbol)

## Objetivo da Sprint
O mapa mental não entrega o código, só o "título". Quando a IA lê no esqueleto que existe a classe `AuthService` e deseja ler apenas ela (e não o arquivo de 2.000 linhas onde ela reside), nós precisamos de uma ferramenta hiperespecífica de busca sintática.

## Tarefas de Desenvolvimento

### 1. A Tool de Busca Semântica Algorítmica (`FindSymbolTool`)
- Criar `FindSymbolToolHandler : IAgentToolHandler`.
- Parâmetros que a IA preenche: `SymbolName` (ex: "ProcessPaymentAsync").

### 2. Busca Híbrida C#
- O backend primeiro checa o cache/banco de Skeletons (Sprint 43) para achar qual arquivo hospeda o símbolo `ProcessPaymentAsync`.
- Identificando o arquivo `PaymentService.cs`, ele bate no `AstParserService`, roda a query do Tree-sitter para encontrar as linhas X e Y exatas onde aquele método reside.
- Executa o `File.ReadAllLines` mas extrai APENAS as linhas X até Y.

### 3. Retorno Focado
- Devolve para o agente o bloco de código exato:
  *"Símbolo ProcessPaymentAsync encontrado no arquivo PaymentService.cs nas linhas 45 a 82. Código: {código aqui}."*

## Testes e Validações
- Num arquivo com 3 funções gigantes. Requisitar `FindSymbolTool` informando apenas o nome da função número 2. A API deve retornar estritamente a função número 2, ignorando o resto.

## Critérios de Aceite (DoD)
1. A IA consegue ler métodos cirurgicamente sem perder tempo consumindo o resto do arquivo.
2. Diminuição agressiva no consumo da Janela de Contexto (Tokens) no ReAct loop.
