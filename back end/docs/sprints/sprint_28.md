# Sprint 28: Gerenciamento da Janela de Contexto (Poda e Tokenização)

## Objetivo da Sprint
O modelo (1.5B ou 27B) só pode ler (exemplo) 8192 tokens por vez. Se o chat e o RAG tiverem 20.000 tokens, o ONNX vai lançar erro `IndexOutOfRange`. Esta sprint foca em construir o "tesoureiro" das conversas: um limitador dinâmico usando o Tokenizer local antes de invocar o LLM.

## Tarefas de Desenvolvimento

### 1. Implementação do `Tiktoken` ou BPE Nativo C#
- O ML.NET oferece o pacote `Microsoft.ML.Tokenizers`. O Qwen3 usa *Byte-Pair Encoding (BPE)*.
- Instanciar a classe de tokenização usando o `vocab.txt` / `tokenizer.json` do Qwen3.
- Criar a rotina `CountTokens(string text)` para saber o peso exato de cada frase.

### 2. Algoritmo de Poda (Context Pruning)
- O Prompt final consiste em: `[System Prompt]` + `[RAG Context]` + `[Histórico (Mongo)]` + `[Nova Pergunta]`.
- Se a soma disso for maior que 8100 tokens, o `ContextManagerService` deve:
  1. Manter obrigatoriamente `[System Prompt]` e `[Nova Pergunta]`.
  2. Podar os blocos de RAG mais fracos (com menor similaridade no Qdrant).
  3. Descartar as primeiras mensagens (mais velhas) do Histórico do Mongo iterativamente até a conta dar exatos 8100 tokens.

## Testes e Validações
- **Teste de Carga por Token:** Inserir via Mock no Qdrant e no Mongo uma conversa imensa que custaria 15.000 tokens. Disparar a chamada. O `ContextManager` DEVE truncar e descartar dados retornando um Array de tensores com *Length* no máximo de 8100, garantindo o `.Run()` seguro do ONNX sem *crash*.

## Critérios de Aceite (DoD)
1. É impossível que a API quebre por causa de um Prompt grande enviado pelo VS Code.
2. O sistema inteligentemente prioriza descartar memórias remotas velhas para comportar a nova instrução no buffer da GPU.
