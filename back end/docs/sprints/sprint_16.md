# Sprint 16: Chunking Avançado (Fatiamento de Texto e Código)

## Objetivo da Sprint
Modelos de Embedding possuem limite de tokens de entrada (ex: 512 ou 8192 tokens). Se o usuário colar um arquivo de código gigante, ou quisermos ingerir a base do OpenCode, tentar vetorizar tudo de uma vez causará um "estouro" de buffer ou perda de contexto semântico. Precisamos construir uma engine de Chunking inteligente em C# para fatiar o texto antes da vetorização.

## Tarefas de Desenvolvimento

### 1. Criar o ChunkingService (Text Splitter)
- Desenvolver um serviço `TextChunkingService` dentro da camada `Infrastructure` ou `Application`.
- Parametrizar: `ChunkSize` (tamanho máximo de cada fatia) e `ChunkOverlap` (sobreposição, ex: 10%, essencial para que o contexto de uma quebra de frase/método não seja perdido na emenda do pedaço seguinte).

### 2. Chunking Específico para Código (.NET, Python, JS)
- Fatiar código por "número de caracteres" (como os chunkers burros fazem) destrói funções no meio.
- Usar expressões regulares (Regex) ou heurísticas iniciais de saltos de linha duplos (`\n\n`) e chaves (`{`, `}`) para garantir que uma função ou classe pequena não seja cortada exatamente na metade de sua estrutura caso passe do ChunkSize.

### 3. Integração com a Pipeline da Sprint 15
- Modificar o observer/publisher do RAG. Em vez de enviar o texto completo da conversa ou do código para o Qdrant, o fluxo passa a ser:
  1. Passar o documento no `TextChunkingService`. Retorna um `List<string>`.
  2. Executar um laço (`Parallel.ForEachAsync` para máxima performance local) chamando o ONNX Embedding para cada "fatia" (chunk).
  3. Salvar as N fatias no Qdrant, mantendo no Payload um identificador de origem (`ParentDocumentId`) indicando que aquelas N fatias pertencem ao mesmo arquivo ou mesma conversa.

## Testes e Validações
- **Testes Unitários:** Passar um texto fictício de 1000 caracteres. Configurar o chunk size para 300 e overlap para 50. O teste deve garantir que a string foi convertida em 4 partes, que a contagem matemática não perdeu 1 único caractere, e validar a existência do overlap em todas as extremidades.

## Critérios de Aceite (DoD)
1. Nenhum texto maior que a janela do modelo de Embedding estoura o buffer ou causa erro na inferência ONNX.
2. Os pedaços de um mesmo código são agrupados logicamente através de sobreposição e preservação da sintaxe mínima.
