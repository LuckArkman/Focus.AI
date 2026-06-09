# Sprint 18: Pipeline de Ingestão de Dados (Dataset OpenCode)

## Objetivo da Sprint
O modelo precisa aprender dados técnicos específicos ou documentações externas para ser mais assertivo em frameworks modernos. Nós usaremos o dataset Hugging Face OpenCode. Diferente da ingestão de chat que é reativa, essa ingestão é um script *Batch* (lote) para carregar o banco Global.

## Tarefas de Desenvolvimento

### 1. Script C# ou Job Dedicado (Worker Service)
- Criar um Worker Service secundário dentro do backend, ou apenas um Endpoint "Admin", encarregado de rodar processos pesados.

### 2. Leitura e Parse de Arquivos Grandes
- O dataset baixado provavelmente será gigabytes de arquivos JSONL ou CSV.
- Criar uma rotina de leitura por fluxo (*Stream*) via `StreamReader` em C# que leia o arquivo OpenCode linha a linha, não sobrecarregando a memória RAM local do servidor.

### 3. Fluxo Híbrido de Ingestão (Chunk + Embedding + Batch Insert)
- A cada linha (ou arquivo de código validado) lido:
  1. Mandar o código para o `TextChunkingService` da Sprint 16.
  2. Converter as dezenas de fatias (chunks) num lote de vetores.
  3. Fazer o *Upsert* **EM LOTE** no Qdrant. O uso do gRPC batch upload (ex: envio de 100 vetores a cada 1 milissegundo) é estritamente vital aqui para ingerir GBs de dados num tempo razoável.
  
### 4. Collection Global Separada
- Gravar isso no Qdrant numa collection apartada, ex: `GlobalKnowledgeBase`, e não na `UserInteractions`. O payload não necessitará de `UserId`.

## Testes e Validações
- **Teste de Vazamento de Memória (Memory Leak):** Pegar um arquivo de 1GB de dados fictícios. Acionar o endpoint de Ingestão do OpenCode e usar o Visual Studio *Diagnostic Tools* para garantir que o *Garbage Collector* (GC) está descartando os chunks gerados de RAM, e a memória do backend fica estável (linha reta e não subindo em rampa até bater *Out Of Memory Exception*).

## Critérios de Aceite (DoD)
1. Processamento massivo de arquivos gigantes com estabilidade de memória em C#.
2. Geração de logs (Serilog) a cada 1000 inserts no banco vetorial para monitoramento da evolução da carga (progress bar de terminal).
