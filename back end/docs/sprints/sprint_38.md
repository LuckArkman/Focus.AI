# Sprint 38: Abstração Multi-Agents (Sub-Agentes Especialistas)

## Objetivo da Sprint
Um dos conceitos mais modernos de IA é a delegação de tarefas (*Multi-Agent Orchestration*). Quando um usuário pedir um sistema inteiro, o Agente Principal (Main Agent) não fará tudo. Ele vai invocar *Sub-Agentes*: um focado apenas em escrever Banco de Dados, outro para CSS Frontend. O backend .NET precisa permitir que um Agente invoque outros Agentes.

## Tarefas de Desenvolvimento

### 1. Criação da Tool `InvokeSubagent`
- Desenvolver `InvokeSubagentToolHandler : IAgentToolHandler`.
- O JSON que a IA mandará contém: `AgentRole` (ex: Frontend_Dev), `Prompt` (ex: "Crie a interface de login em React").

### 2. Orquestração Multi-Threading de Sessões
- O `AutonomousAgentService` ao receber o pedido, criará uma **nova** instância de `IChatClient` (usando o 1.5B para o sub-agente, ou o 27B).
- Ele iniciará um segundo loop `ReAct` num Thread em background separado da Thread do Agente Pai.
- Isso permitirá que múltiplos agentes codifiquem múltiplos arquivos em paralelo para o mesmo projeto no C#.

### 3. Registro e Definição Dinâmica de Agentes (`DefineSubagent`)
- Criar ferramenta `DefineSubagentTool`.
- O Agente Mestre pode "criar" uma persona no ar (System Prompt dinâmico injetado no momento da instanciação), equipando esse novo sub-agente apenas com a ferramenta `WriteFile`, e não com ferramentas de linha de comando, restringindo seu escopo por segurança.

## Testes e Validações
- Teste de Concorrência: Iniciar um Agente Pai que invoca 2 Agentes Filhos ao mesmo tempo para criarem 2 arquivos distintos. Assegurar que o ONNX Runtime não dê falha de concorrência (lock handling) já que os tensores estarão sendo processados em streams independentes, enfileirando acessos pesados à GPU.

## Critérios de Aceite (DoD)
1. Arquitetura robusta onde Agentes podem gerar novos Agentes em *Threads* diferentes, compartilhando memória e projeto.
2. Acessos simultâneos ao banco e ferramentas protegidos por *Locks* e *Semáforos* para não causar colisão de disco (dois arquivos gravando ao mesmo tempo).
