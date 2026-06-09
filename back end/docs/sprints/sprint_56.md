# Sprint 56: Integração de Dicas (Semantic Hover Tooltips)

## Objetivo da Sprint
Quando você passa o mouse por cima de uma função, aparece um "balãozinho" com uma documentação sobre ela. Nossa IA vai gerar documentações para códigos não-documentados "on-the-fly" (na hora), explicando o que funções complexas antigas fazem, sem que o dev precise abrir o chat principal.

## Tarefas de Desenvolvimento

### 1. O Handler de Hover (Passar o Mouse)
- Criar `HoverHandler : IHoverHandler`.
- É invocado pelo LSP contendo o arquivo e a posição X/Y (Linha/Coluna).

### 2. Descoberta de Função por Tree-Sitter
- Na posição X/Y, usar o AST Tree-sitter (Sprint 42) para descobrir em qual "Método" o mouse repousa.
- Extrair o código bruto daquele método.

### 3. Geração Oculta (On-The-Fly)
- Pegar esse método bruto e passar velozmente pelo modelo ONNX 1.5B com o prompt: *"Explique em 3 frases curtas o que essa função faz. Ignore saudações."*
- Formatar a saída da IA em Markdown:
  ```markdown
  **Focus.AI Analysis:**
  Esta função itera sobre os usuários e filtra aqueles sem licença ativa. Acessa o banco e retorna uma List limpa.
  ```
- Devolver como resposta da Hover ao cliente LSP.

## Testes e Validações
- Teste de Integração: Passar uma coordenada X/Y que cai exatamente dentro do escopo geométrico de uma classe. Validar se o AST descobre o bloco certo antes de enviar a string para a etapa do Mock do LLM.

## Critérios de Aceite (DoD)
1. Agente consegue ler as intenções geométricas do mouse do desenvolvedor e acoplar inteligência descritiva.
2. A requisição utiliza cache (grava as funções já "explicadas" daquele arquivo num MemoryCache temporário por 10 minutos) para não engolir o PC pedindo 30 inferências se o dev mover muito o mouse.
