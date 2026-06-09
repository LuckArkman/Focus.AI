# Sprint 41: Integração C# com o Tree-sitter (Bindings)

## Objetivo da Sprint
Os LLMs não sabem ler pastas; eles "chutam" como a arquitetura está baseada no nome do arquivo. Para o Focus.AI ser absurdamente cirúrgico, vamos introduzir a biblioteca **Tree-sitter**, um parseador rápido que constrói Árvores Sintáticas Abstratas (AST) em milissegundos. Nesta sprint, faremos a ponte (Binding) do C (nativa do Tree-sitter) para o .NET.

## Tarefas de Desenvolvimento

### 1. Seleção da Biblioteca de Binding
- Investigar e instalar o pacote NuGet de Binding mais recente para Tree-sitter em C# (ex: `TreeSitter.Bindings` ou compilar via P/Invoke manualmente se não houver um atualizado para .NET 8).
- Importar as bibliotecas nativas (`.dll` no Windows, `.so` no Linux) referentes ao *core* do Tree-sitter no projeto `Infrastructure`.

### 2. Carregamento Dinâmico de Gramáticas (Grammars)
- O Tree-sitter precisa de "Gramáticas" diferentes para cada linguagem de programação.
- Baixar as DLLs de gramática pré-compiladas para as linguagens foco iniciais: C#, Python, JavaScript e TypeScript.
- Criar a classe `TreeSitterGrammarLoader` que carrega a gramática certa dinamicamente em tempo de execução, dependendo da extensão do arquivo lido (`.cs` chama `tree_sitter_c_sharp()`).

### 3. Parseamento Básico
- Escrever um código C# minimalista que envia uma string de código (`public class A {}`) pro Tree-sitter e recebe de volta a raiz da árvore (Root Node).

## Testes e Validações
- **Teste de Memória Não Gerenciada:** Árvores nativas em C exigem limpeza manual. Instanciar 1.000 árvores seguidas num loop, testar o `.Dispose()` nelas e validar se a memória RAM do app não inflou no Diagnostic Tools, confirmando a estabilidade do P/Invoke.

## Critérios de Aceite (DoD)
1. C# se comunicando nativamente com o C do Tree-sitter em frações de milissegundo.
2. Suporte inicial plug-in-play para carregar ao menos as gramáticas de JS e C#.
