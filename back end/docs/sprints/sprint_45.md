# Sprint 45: A Tool de Busca de Referências (Find Usages)

## Objetivo da Sprint
Se o agente decidir apagar uma classe ou mudar a assinatura de uma função, ele não pode quebrar o resto do projeto do usuário. Para isso, o Agente vai invocar a ferramenta `Find Usages` (equivalente ao Shift+F12 do Visual Studio) para verificar quem depende daquele código antes de editá-lo.

## Tarefas de Desenvolvimento

### 1. O Desafio do LSP vs Regex
- Como não estamos embutidos profundamente no compilador C# (Roslyn) nativamente em todos os arquivos de fora, temos duas rotas para encontrar usages.
- Rota A: Usar *RipGrep* (regex ultrarrápida no disco).
- Rota B: Consultar as *Imports/Usings* capturadas pelo Tree-sitter.

### 2. A Tool `FindUsagesTool`
- Vamos usar a Rota A (Ripgrep embutido em C# com otimização) para velocidade, cruzada com Tree-sitter.
- Criar `FindUsagesToolHandler`. O agente passa `SymbolName`.
- O C# faz uma varredura em todos os arquivos mapeados procurando o padrão da palavra exata, garantindo que não seja parte de outra string (usando Word Boundaries regex `\bSymbolName\b`).

### 3. Extração de Contexto de Uso
- Não basta dizer "Arquivo X linha 10 usa isso". A IA precisa ver COMO usa.
- Para cada ocorrência (Match), extrair a linha do hit + 2 linhas acima e 2 linhas abaixo para dar contexto de uso.
- Format Output: 
  ```text
  Found in OrderController.cs (Line 24):
  22: var order = GetCurrentOrder();
  23: // calling the payment method
  24: await paymentService.ProcessPaymentAsync(order.Total);
  ```

## Testes e Validações
- Teste de regex avançada em múltiplos arquivos do disco. Simular um refactor. Pedir usabilidades. O código deve capturar corretamente as linhas em um tempo aceitável (< 1 segundo pra repos médios).

## Critérios de Aceite (DoD)
1. Agente munido de inteligência preventiva, mitigando o risco de ele mesmo gerar erros de compilação em cadeia durante uma refatoração massiva.
