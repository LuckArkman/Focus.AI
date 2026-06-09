# Sprint 61: Inicialização da Extensão VS Code (TypeScript) e Gerenciamento do Processo Servidor C#

## Objetivo da Sprint
Inicializar a base da extensão do VS Code utilizando TypeScript e desenvolver o orquestrador do ciclo de vida que executa o backend em .NET 8 como um processo oculto filho (`child_process`).

## Tarefas de Desenvolvimento

### 1. Setup da Extensão com TypeScript
- Inicializar o projeto na pasta `front end` utilizando a ferramenta oficial `yo code` com suporte a TypeScript e empacotador básico.
- Configurar os arquivos base: `package.json`, `tsconfig.json`, `extension.ts` (entrypoint).
- Configurar o escopo de ativação no `package.json` (`activationEvents` definidos para `onStartupFinished` ou comandos específicos).

### 2. Orquestração do Processo Filho (C# Server)
- Desenvolver um serviço `ServerManager` no TypeScript que localiza o executável compilado do backend C# (`Focus.AI.Api.exe` no Windows ou binário ELF correspondente no Linux/macOS).
- Implementar o lançamento do processo utilizando `child_process.spawn`. Passar argumentos de linha de comando necessários (ex: porta a escutar, modo de execução).
- Capturar erros na inicialização e logs do console do backend (`stdout` e `stderr`) redirecionando-os para um `vscode.OutputChannel` dedicado ("Focus.AI Server Output").

### 3. Gerenciamento do Ciclo de Vida e Resiliência
- Garantir a limpeza de processos: ao chamar a função `deactivate()` da extensão, enviar um sinal de finalização adequado para matar o processo do servidor C# e evitar processos órfãos (zumbis).
- Adicionar uma política básica de reconexão automática: se o servidor C# falhar inesperadamente, tentar reiniciá-lo até 3 vezes com um delay exponencial.

## Testes e Validações
- **Teste de Ciclo de Vida:** Abrir a extensão no VS Code em modo debug (F5). Validar se o processo do backend .NET surge no Gerenciador de Tarefas do Windows/Monitor de Atividade e desaparece imediatamente ao fechar a janela do VS Code.
- **Validação de Logs:** Verificar se as saídas do logger do ASP.NET Core aparecem no canal de output da extensão.

## Critérios de Aceite (DoD)
1. Extensão do VS Code inicializa no TypeScript sem disparar avisos ou erros.
2. O servidor C# é iniciado e encerrado de maneira acoplada ao ciclo de vida da extensão do VS Code.
