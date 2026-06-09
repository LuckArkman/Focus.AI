# Sprint 42: Parseamento AST de Múltiplas Linguagens

## Objetivo da Sprint
O Agente IA vai pedir para "Entender o arquivo `UserService.js`". O backend não devolverá o texto cru gigante. Ele vai construir a árvore sintática, navegar pelos nós e extrair o "esqueleto" semântico do código.

## Tarefas de Desenvolvimento

### 1. Serviço de Extração de AST (`AstParserService`)
- Criar o serviço abstrato para varrer nós do Tree-sitter utilizando o padrão *Visitor* ou *Tree Cursor*.
- O cursor desce na árvore arquivo-fonte e filtra os dados irrelevantes (espaços, comentários vazios) buscando apenas os nós de declaração (Declarações de Classes e Funções).

### 2. Queries do Tree-sitter (S-Expressions)
- A forma correta de buscar dados no Tree-sitter é usando "Queries" lisp-like.
- Criar o arquivo `csharp-symbols.scm` contendo a query: `(class_declaration name: (identifier) @class.name)`
- Criar o arquivo `javascript-symbols.scm` contendo a query para arrow functions e classes ES6.
- A aplicação C# carregará esses arquivos `.scm` e os rodará contra o código fonte do usuário.

### 3. Estruturação do Output C#
- O parser converterá o retorno complexo do C para uma estrutura limpa de POCOs C#:
  ```csharp
  public class CodeSymbol {
      public string Name { get; set; } // ex: GetUserById
      public SymbolType Type { get; set; } // Method, Class, Interface
      public int StartLine { get; set; }
      public int EndLine { get; set; }
  }
  ```

## Testes e Validações
- Teste Unitário com código C# misturado. Passar um texto contendo 2 classes, 3 métodos e 1 enum. A classe `AstParserService` deve retornar uma Lista exata com 6 `CodeSymbol` mapeados, atestando as linhas onde cada bloco começa e termina.

## Critérios de Aceite (DoD)
1. A extração não depende do LLM, é algorítmica, exata e matemática.
2. Identificação precisa do escopo geográfico do arquivo (linhas de início e fim) que são essenciais para uma futura edição cirúrgica (Substituição de Texto restrita a uma função).
