# Sprint 47: Construção do Grafo de Dependências (Memória de Vizinhança)

## Objetivo da Sprint
Juntar os dados das duas Sprints anteriores em um único objeto de inteligência gigante dentro do C#. O Grafo Direcionado.

## Tarefas de Desenvolvimento

### 1. Estrutura de Dados em C# (Directed Graph)
- Criar o `DependencyGraphManager`.
- Usar um `Dictionary<string, HashSet<string>>` onde a chave é o Path do Arquivo, e o HashSet contém todos os paths dos arquivos que **dependem dele**. (Lista de adjacência).

### 2. Interseção com Banco Vetorial (Dependency RAG)
- Modificar o fluxo de Retrieval (Sprint 17). Se a pergunta do usuário ou a ação da IA envolver a classe `InvoiceService`, o Retriever encontra o vetor.
- O Novo Poder: Antes de devolver pro LLM, o backend injeta na resposta de forma invisível:
  *"Nota do Sistema: O InvoiceService é chamado diretamente por InvoiceController.cs e ReportGenerator.cs."*
- Isso cria uma Consciência Espacial para a IA, onde ela ganha um "sentido de vizinhança" sem nem precisar perder tempo chamando a ferramenta de `FindUsages`.

## Testes e Validações
- Teste de Ciclos: Código com importações circulares (A importa B, B importa A) não pode gerar *StackOverflowException* na construção do grafo em C#. Proteger recursividade.

## Critérios de Aceite (DoD)
1. Criação de um Grafo Acíclico (ou que suporte ciclos) seguro em memória RAM.
2. Injeção de "Sentido de Aranha" no contexto RAG do LLM (conhecimento sobre impactos adjacentes sem que a IA tenha solicitado explicitamente).
