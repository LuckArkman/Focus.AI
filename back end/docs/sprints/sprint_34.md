# Sprint 34: Ferramenta de Execução de Terminal (Run Command)

## Objetivo da Sprint
Para ser um Copiloto Autônomo, o assistente precisa rodar comandos de compilação, executar scripts de teste (ex: `npm test` ou `dotnet build`) e usar o Git em nome do usuário.

## Tarefas de Desenvolvimento

### 1. Handler do Terminal Local
- Criar `RunCommandToolHandler : IAgentToolHandler`.
- Parâmetros: `CommandLine` (string), `Cwd` (Current Working Directory).
- O C# instanciará a classe `Process` (`System.Diagnostics`) rodando `cmd.exe` (no Windows) ou `/bin/bash` (no Linux/Mac).

### 2. Captura de Standard Output/Error
- Configurar o `ProcessStartInfo` com `RedirectStandardOutput = true` e `RedirectStandardError = true`.
- Capturar a string do terminal e devolvê-la para o agente. Ex: se o Agente mandar rodar `git status`, ele precisa receber a string inteira de retorno do Git para decidir os próximos passos.

### 3. Tratamento de Comandos Contínuos (Background Tasks)
- Alguns comandos não retornam rápido (ex: `npm run dev` que roda um servidor).
- Adicionar parâmetro `WaitMsBeforeAsync`. Se o comando demorar mais que o tempo estipulado, o processo não morre, mas é enviado pro *Background*, e a ferramenta devolve um "Job Id" pro agente.
- O Agente poderá usar outra Tool no futuro (`manage_task`) para verificar o status desse job ou mandar matá-lo (`Process.Kill()`).

## Testes e Validações
- **Execução Real:** Programar um teste de integração que invoca o `RunCommandToolHandler` pedindo para rodar `dotnet --version`. O assert deve capturar a string indicando a versão 8.0.x e confirmar o sucesso.

## Critérios de Aceite (DoD)
1. IA pode interagir livremente com a linha de comando do sistema operacional subjacente.
2. Saídas e mensagens de erro do terminal (StdErr) são interceptadas em C# e devolvidas ao modelo de IA.
