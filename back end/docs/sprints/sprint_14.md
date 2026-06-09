# Sprint 14: Desenvolvimento da Pipeline de Geração de Embeddings

## Objetivo da Sprint
Antes de jogar dados no banco vetorial Qdrant, o texto bruto da conversa ou código-fonte precisa virar um Array Numérico (Vector Embedding). Em vez de chamar a API da OpenAI para pagar por embeddings, vamos desenvolver um gerador de embeddings local 100% hospedado no backend .NET.

## Tarefas de Desenvolvimento

### 1. Seleção e Download do Modelo de Embedding
- O modelo recomendado (leve e excelente para código/inglês/português) é o `BGE-M3` (BAAI) ou `all-MiniLM-L6-v2` exportado para formato ONNX.
- Instruir a aplicação, via classe `ModelDownloaderService` inicial, a verificar se o modelo `.onnx` está na pasta local. Se não estiver, baixar ou exibir erro alertando a falta.

### 2. Classe de Inferência C# para Vetorização
- Usar a biblioteca `Microsoft.ML.OnnxRuntime`.
- Criar o serviço `LocalEmbeddingGenerator : IEmbeddingGenerator`.
- O método `GenerateEmbeddingAsync(string text)` deverá:
  1. Tokenizar o texto de entrada (usando a bibliotea Tokenizers compatível com o BERT ou modelo escolhido).
  2. Preparar os tensores de entrada (`input_ids`, `attention_mask`).
  3. Fazer o *Run* na sessão ONNX em RAM/CPU (como o modelo é minúsculo, não exige GPU para rodar em 5ms, liberando a GPU pro Qwen 27B).
  4. Extrair o Tensor resultante (Pooler Output) como um `float[]`.

### 3. Cache e Normalização Numérica
- Normalizar o vetor de saída pelo método de normalização L2 (necessário para que a similaridade de cosseno no Qdrant funcione perfeitamente).

## Testes e Validações
- **Validação Matemática (Assert):** O tamanho do array retornado deve ser estritamente igual à dimensão do modelo (ex: `float[384]`).
- **Validação Lógica:** Passar duas frases quase iguais (`"Deletar a tabela do postgres"` e `"Remover tabela no PostgreSQL"`) e garantir que a distância de cosseno entre elas passe de 0.85 (alta similaridade semântica) nos testes do xUnit.

## Critérios de Aceite (DoD)
1. Geração de vetores 100% offline, em ambiente local, implementada em C#.
2. Normalização matemática L2 aplicada sobre o Output Tensor do ONNX.
