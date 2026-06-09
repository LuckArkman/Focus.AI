# Sprint 70: Testes de Integração E2E da Extensão VS Code + Backend

## Objetivo da Sprint
Configurar e implementar a suíte de testes de integração ponta a ponta (E2E) simulando ações do usuário na extensão do VS Code integrada ao servidor backend local em C#.

## Tarefas de Desenvolvimento

### 1. Setup do Ambiente de Teste E2E (VS Code Test)
- Configurar a biblioteca oficial do VS Code `@vscode/test-electron` para iniciar uma instância isolada e limpa da IDE programaticamente durante a execução dos testes.
- Criar a estrutura de diretórios `front end/src/test/suite/` para acomodar os arquivos de script de testes.

### 2. Implementação dos Fluxos Automatizados
- Escrever testes que abrem um arquivo de código mockado e verificam:
  - Se a extensão inicializa sem disparar erros no log da console.
  - Se a chamada simulada de digitação de código dispara a exibição das completações rápidas (Ghost Text).
  - Se o comando de envio de chat lateral envia a payload correta e recebe respostas em streaming do backend .NET local.
- Configurar mocks rápidos para comportamentos da IDE quando necessário.

### 3. Integração no Pipeline e npm Script
- Adicionar o script `npm run test:e2e` no `package.json` da extensão.
- Garantir que a execução do script prepare o ambiente instalando as extensões de teste do VS Code Electron necessárias e executando os testes da suíte localmente.

## Testes e Validações
- **Execução da Suíte E2E:** Rodar `npm run test:e2e` e certificar-se de que a janela do VS Code abre em modo de testes automatizados, completa os testes estruturados de completação e chat, fecha a janela de forma automática e reporta 100% de sucesso.

## Critérios de Aceite (DoD)
1. Ambiente de testes integrados configurado utilizando o `@vscode/test-electron`.
2. Cobertura de testes de ponta a ponta validando o fluxo crítico de Ghost Text e Chat lateral em tempo real passando com sucesso.
