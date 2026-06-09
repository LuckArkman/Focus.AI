# Sprint 36: Monitoramento de Tarefas em Background (Task Tracking)

## Objetivo da Sprint
O usuário pode mandar o agente fazer algo longo: *"Analise toda a pasta de controllers, encontre as brechas de segurança e corrija"*. O agente vai ficar 10 minutos chamando tools. O usuário precisa saber o que diabos está acontecendo nos bastidores em tempo real, sem ficar olhando para uma tela de carregamento congelada.

## Tarefas de Desenvolvimento

### 1. Sistema de Publicação de Pensamentos (Thought Stream)
- Quando a IA retorna a chamada de uma ferramenta, geralmente ela emite um texto oculto antes (o "Thinking" process).
- Modificar o `OnnxChatClient` para identificar quando o token gerado pertence ao bloco de raciocínio.
- Enviar esses tokens de raciocínio via SignalR num canal secundário (ex: `AgentThinkingStream`).

### 2. Lista de Tarefas (Artifact "task.md" Dinâmico)
- A própria IA criará um arquivo oculto (ou artefato virtual em RAM no C#) gerenciando o status da missão (TODO list).
- À medida que o Agente do Módulo 3 avança no *ReAct Loop* (Sprint 35), o `AutonomousAgentService` envia eventos de log padronizados: *"Executando ferramenta: Run Command (npm test)"*.

### 3. Interrupção por parte do Usuário (Kill Switch)
- Adicionar ao SignalR um comando `CancelAgentTask(sessionId)`.
- Se o usuário perceber que o agente está apagando a pasta errada ou fazendo bobeira, ele aperta um botão no VS Code. O Backend aciona um `CancellationTokenSource`, cancela a inferência do ONNX no meio do processamento da GPU, interrompe o loop do agente e revoga qualquer comando no CMD em andamento.

## Testes e Validações
- Validar o `CancellationToken`: Programar uma task pesada que demorará 5 minutos no C#. Disparar o comando de aborto via WebSocket e atestar através dos logs do Serilog que a thread não-gerenciada da GPU foi morta corretamente e a memória RAM devolvida em menos de 2 segundos.

## Critérios de Aceite (DoD)
1. Total transparência para o usuário do fluxo interno de tomadas de decisão da IA.
2. Um botão "Stop/Abortar" na interface do desenvolvedor tem poder absoluto de cancelar a execução da máquina inteligente no Backend em nível de Threads de hardware (CancellationToken seguro).
