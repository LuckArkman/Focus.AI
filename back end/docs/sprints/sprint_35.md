# Sprint 35: O Ciclo de Observação-Ação (ReAct Loop)

## Objetivo da Sprint
Conectar todas as Tools criadas. Um agente não apenas chama uma ferramenta e desiste. Se a ferramenta retorna erro, o agente lê o erro, entende a falha, e tenta chamar outra ferramenta até resolver o problema original do usuário. Esse loop contínuo (Reasoning + Acting) é o coração da autonomia do sistema.

## Tarefas de Desenvolvimento

### 1. Orquestrador do ReAct (Reasoning and Acting)
- Criar a classe `AutonomousAgentService`.
- Este serviço envolverá a chamada original do `IChatClient` num laço `while`.
- **Passo 1:** Pede para a IA gerar uma resposta.
- **Passo 2:** A IA chama uma ferramenta (ex: `view_file`). O SignalR avisa o frontend: *"O Agente está lendo arquivos..."*
- **Passo 3:** O C# roda o método `ReadFileToolHandler` em background e obtém o resultado do arquivo.
- **Passo 4:** O C# junta a reposta da ferramenta numa mensagem do tipo *ToolResult* no histórico e **Manda a bola de volta pra IA** sem intervenção do usuário.
- **Passo 5:** O ciclo se repete até a IA finalmente decidir parar de chamar ferramentas e devolver uma mensagem de texto simples para o usuário.

### 2. Bloqueio por Limite de Segurança (Max Iterations)
- Para impedir que um modelo entre em um "Loop Infinito" ou "Alucinação" (onde ele fica chamando erro e rodando `ls` eternamente na mesma pasta sem progresso), estabelecer `MaxAgentIterations = 15`.
- Se a IA bater no limite de ciclos sem apresentar solução, o backend interrompe forçosamente a execução e diz ao usuário: *"O Agente tentou 15 passos mas não conseguiu encontrar a solução de forma autônoma."*

## Testes e Validações
- **Mock de Conversa Complexa:** Criar um teste simulando uma IA determinística falsa que primeiro pede `ls`, depois `cat program.cs`. Validar que o `AutonomousAgentService` mantém o laço de Request-Response de forma assíncrona gerando eventos no barramento sem estourar o limite de iterações.

## Critérios de Aceite (DoD)
1. O backend gerencia o ping-pong (loop) de chamada de função -> execução -> resultado -> nova chamada sem exigir cliques extras do usuário.
2. Sistema anti-loop infinito e limitadores de custo/tempo rodando rigorosamente em memória.
