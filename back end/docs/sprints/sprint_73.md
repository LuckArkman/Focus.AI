# Sprint 73: Testes de Estresse, Carga Extrema e Recuperação de Falhas

## Objetivo da Sprint
Submeter todo o sistema Focus.AI a condições extremas e cenários de falha induzida (Chaos Engineering) para validar a resiliência do sistema como um todo, desde o LSP até a orquestração do Agente Autônomo e os bancos de dados (MongoDB, PostgreSQL, Qdrant).

## Tarefas de Desenvolvimento

### 1. Simulação de Alta Carga Concorrente
- Escrever scripts de teste de carga (usando ferramentas como NBomber, k6 ou um script customizado em C#) simulando dezenas de instâncias ou processos da IDE batendo no mesmo servidor backend simultaneamente.
- Simular rajadas contínuas de solicitações LSP (Ghost Text, Tooltips, diagnósticos) intercaladas com comandos complexos no Chat e invocação de Sub-Agentes autônomos.

### 2. Recuperação de OOM (Out Of Memory) e Timeouts
- Simular intencionalmente situações em que o ONNX Runtime fique sem memória de vídeo (OOM) enviando payloads massivas acima da janela de contexto de 8192/32k tokens.
- O backend deve falhar de forma graciosa (graceful degradation), não crashear (não derrubar o servidor HTTP/SignalR), relatar o erro de forma clara na interface do VS Code, liberar recursos, limpar o estado do modelo e continuar funcionando para a próxima solicitação menor.

### 3. Validação do Auto-Restart e Sandboxing
- Matar intencionalmente processos filhos simulados e verificar se o sistema orquestrador recupera o controle, reiniciando agentes ou conexões de bancos de dados se necessário, mantendo o Sandboxing intacto durante travamentos bruscos.

## Testes e Validações
- **Teste de Resiliência E2E:** Deixar o script de Chaos Engineering rodando por 4 horas contínuas. Acompanhar telemetria para garantir zero travamentos permanentes (`Deadlocks`) e retorno da integridade do serviço após o alívio da carga.

## Critérios de Aceite (DoD)
1. Sistema não crasheia o servidor host sob condições de Out of Memory ou uso abusivo de contexto.
2. Recuperação automática e gracefully de falhas catastróficas simuladas nos componentes de IA e bancos de dados.
