# Sprint 48: RAG Híbrido Avançado (Vetores + AST)

## Objetivo da Sprint
Um banco vetorial (Qdrant) sozinho é "burro" para programação. Ele entende palavras ("Postgres", "Conexão"), mas não entende que a *classe X implementa a interface Y*. Esta sprint une o RAG de Machine Learning com a Exatidão Matemática do Tree-sitter.

## Tarefas de Desenvolvimento

### 1. Pesagem de Resultados Híbridos (Hybrid Scoring)
- Quando fazemos o RAG e o Qdrant retorna o arquivo `EmailSender.cs` com 0.92 de similaridade semântica.
- O C# pega esse arquivo e checa no AST: "Esse arquivo possui uma classe? Ela herda de alguma interface?". Resposta do AST: "Herda de IMessageSender".
- O C# busca no Qdrant o arquivo que contém `IMessageSender` (mesmo que tenha similaridade 0.0, porque a palavra Email não está lá) e Puxa ele pro contexto de "carona".
- Chamamos isso de *AST-Augmented Retrieval*.

### 2. Fusão Semântica C#
- O backend garante que nenhuma interface abstrata será mandada para o LLM sem sua classe concreta, e vice-versa.
- Criar a rotina `ContextFusionService` que mescla os vetores do Qdrant com os nós próximos do Dependency Graph.

## Testes e Validações
- Submeter um request para gerar testes unitários da classe `EmailSender`. A RAG tradicional traria apenas ela. A RAG Híbrida deve obrigatoriamente trazer pro prompt da IA a `IMessageSender.cs` junto, permitindo à IA mockar a interface perfeitamente no xUnit.

## Critérios de Aceite (DoD)
1. Qualidade absurda e quase mágica do contexto inserido no prompt do LLM, unindo deduções linguísticas (Vetores) e verdades de compilação (AST).
