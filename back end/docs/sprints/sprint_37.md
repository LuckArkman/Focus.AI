# Sprint 37: Sandboxing e Execução de Comandos com Permissão

## Objetivo da Sprint
Rodar comandos diretos da IA no PC de um programador é absurdamente perigoso (ex: o agente executa `rm -rf /` ou `rmdir /s /q C:\` por falha interpretativa). Nós precisamos de um Sandbox ou de um "Gatekeeper" autoritário no C# para proteger a máquina hospedeira.

## Tarefas de Desenvolvimento

### 1. Sistema de Pedido de Permissão (Permission Boundary)
- Modificar o `RunCommandToolHandler`. Em vez de executar o terminal logo de cara, se o comando acionado usar binários destrutivos identificados por Regex (ex: `rm`, `del`, `drop`, `npm install`, `pip install`), a função retorna um estado pendente de execução: `WaitingForUserApproval`.
- O SignalR envia um alerta ao front: *"O agente deseja rodar 'npm install lodash'. Permitir? (Y/N)"*.

### 2. O Tool "Ask Permission"
- Criar a ferramenta `AskPermissionTool`. O próprio agente pode invocar essa ferramenta para pedir "escopo livre" para rodar N comandos na pasta do projeto. Se o usuário conceder, esse escopo fica salvo temporariamente no Redis local ou em Memória do Backend.

### 3. Isolamento Físico de Processo (Opcional Avançado)
- Em vez de rodar o comando no sistema nativo do PC host do usuário (ex: terminal do Windows original), o Backend deverá rodar os comandos do Agente focados na pasta dentro de um *container WSL ou Docker efêmero local* montando o diretório de código. Se o Agente formatar tudo, ele apenas formata o container isolado (Conceito de DevContainer).

## Testes e Validações
- O Agente solicita a execução de `rm -rf temp`. O C# bloqueia com estado *AwaitingApproval*. O teste unitário envia o evento negativo "Deny". A ferramenta deve devolver à IA a resposta do terminal: *"Failure: Permissão negada pelo usuário."* A IA deve ser forçada a tomar outro caminho.

## Critérios de Aceite (DoD)
1. Comandos com potencial lesivo bloqueados nativamente pela camada protetora.
2. O usuário detém controle total (Opt-In explícito) de comandos arriscados, ou trabalha em regime de Sandbox (Containers).
