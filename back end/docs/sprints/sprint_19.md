# Sprint 19: Testes de Integração do Sistema Híbrido (RAG e Bancos)

## Objetivo da Sprint
Esta é uma sprint puramente de garantia de qualidade arquitetural. Nós finalizamos as implementações de Persistência e RAG (Retrieval-Augmented Generation). Antes de plugarmos o Cérebro LLM (Qwen3), o sistema que busca as coisas na memória precisa ter 100% de cobertura de código comprovada e lidar com estresse.

## Tarefas de Desenvolvimento

### 1. Bateria Final de Integração do Workflow
- No projeto `IntegrationTests` construído na Fase 1, criar a suíte `RagWorkflowTests.cs`.
- Roteiro Automatizado a ser testado na memória:
  1. Testcontainer levanta Mongo e Qdrant.
  2. Usuário Fake faz um request enviando uma mensagem ("Aqui está a connection string secreta: postgres://user...").
  3. Aguardar (Atraso artificial pequeno pro evento Background gravar o embedding).
  4. O Usuário faz nova requisição ("Qual a connection string que falei?").
  5. Validar que a API, internamente, acionou o Qdrant, resgatou o exato contexto de "postgres://user..." e devolveu com a Similaridade calculada superior a 0.8.

### 2. Tratamento de Condições de Corrida (Race Conditions)
- Resolver *bugs* comuns neste ecossistema: Se o usuário clica num botão duplo, ou se a interface do VS Code enviar 2 requests ao mesmo tempo, nossa conexão com Mongo não deve tentar criar 2 *Sessions* iguais no banco gerando ID duplicado ou conflitos transacionais. (Revisar logs via Serilog).

### 3. Exposição de Status / Health Check
- Desenvolver um endpoint Minimal API `/api/health` usando o pacote `Microsoft.Extensions.Diagnostics.HealthChecks`.
- O endpoint DEVE pingar o Postgres, o MongoDB e o Qdrant. Se algum dos três containers docker não responder num milissegundo esperado, retornar "Degradado" ou "Não Saudável" em JSON. Isso será consumido na barra de status inferior da Extensão VS Code ("Qdrant: Desconectado" ícone vermelho).

## Testes e Validações
- Bater no `/api/health` em tempo real, parar o container do Qdrant usando CLI `docker stop focus_qdrant` e ver a API identificar a queda do serviço vetorial em menos de 1 segundo refletindo no JSON de status.

## Critérios de Aceite (DoD)
1. Workflow completo, da persistência crua até a vetorização e recuperação simulada, é coberto pelos testes unitários automáticos e roda de maneira autossuficiente (Zero Config).
2. Health Checks avançados informam transparentemente ao Módulo 2 o estado de toda a infraestrutura base.
