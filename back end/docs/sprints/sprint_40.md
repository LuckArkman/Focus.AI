# Sprint 40: Teste End-to-End de Resolução de Problema (The Autonomous Test)

## Objetivo da Sprint
Finalizar a Fase 4 (Agentes e Ferramentas) com um teste massivo de Integração End-to-End (E2E). O objetivo aqui é ligar tudo o que foi feito nas últimas 39 Sprints e colocar o modelo Qwen3 local para rodar resolvendo um bug num repositório fictício isolado, puramente guiado por C#.

## Tarefas de Desenvolvimento

### 1. Ambiente Fictício de Repositório (Mock Repo)
- O script de teste de integração inicializará um diretório temporário `C:\temp\FocusTestRepo`.
- Colocará dois arquivos com erros propositais graves: um `calculator.js` (com uma função sum usando soma errada `a - b`) e um `tests.js` (que dá falha ao rodar `npm test`).

### 2. A Invocação Inicial
- O Teste do xUnit emulará o *Frontend* chamando via gRPC / SignalR a mensagem:
  *"Execute os testes neste diretório. Analise os logs do terminal, encontre o arquivo que está causando a falha e conserte o erro matemático. Não pare até os testes passarem."*

### 3. Observabilidade e Telemetria
- Habilitar rastreamento máximo no `Serilog`.
- O backend deve:
  1. O Agente pede o Tool `RunCommand ("npm test")`.
  2. O backend executa em background e falha. Ele lê a mensagem "Expected 4 but got 0 in sum(2,2)".
  3. O Agente pede o Tool `ViewFile ("calculator.js")`.
  4. O Agente localiza o erro matemático "return a - b".
  5. O Agente pede o Tool `ReplaceFileContent` arrumando para `return a + b`.
  6. O Agente pede `RunCommand ("npm test")`. Passa tudo verde.
  7. O Agente finaliza a *Session* informando "Sucesso. Código corrigido."

## Testes e Validações
- **O Teste Final E2E:** A suíte de testes xUnit vai instanciar os bancos (Testcontainers), carregar a API completa em memória e disparar o loop. O teste vai monitorar programaticamente as chamadas de funções JSON interceptadas do ONNX. O teste só passa verde (Pass) quando a saída final do LLM acusar finalização correta das tarefas de terminal.

## Critérios de Aceite (DoD)
1. Fim da Fase 4. O sistema prova, através de um caso de uso real e integrado ao motor C#, que consegue operar autonomamente lendo arquivos, executando terminais, errando, recebendo feedbacks do erro, consertando arquivos via texto cru, e validando tudo até o fim, finalizando o loop ReAct nativo do Módulo 3.
