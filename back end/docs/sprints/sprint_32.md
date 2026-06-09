# Sprint 32: Implementação da Ferramenta de Leitura (Read File Tool)

## Objetivo da Sprint
Um agente que ajuda a programar precisa saber ler o código do desenvolvedor. O VS Code já faz isso repassando o arquivo atual, mas o *Agente Autônomo* pode precisar ler arquivos que não estão abertos na tela. Nesta sprint, criaremos a primeira ferramenta real do agente.

## Tarefas de Desenvolvimento

### 1. Handler da Ferramenta de Leitura
- Criar a classe `ReadFileToolHandler : IAgentToolHandler`.
- Nome da ferramenta: `view_file`.
- Parâmetros esperados (JSON Schema): `AbsolutePath` (string), `StartLine` (int, opcional), `EndLine` (int, opcional).

### 2. Acesso Seguro ao Sistema de Arquivos (File System)
- O código em C# usará `File.ReadAllLinesAsync()`.
- **Regra de Segurança Estrita:** O backend só tem permissão para ler arquivos que estejam DENTRO do diretório `LocalPath` do projeto do usuário (definido na tabela de projetos).
- Implementar verificação de path (Path Traversal Prevention): Se o agente tentar ler `C:\Windows\System32\config` ou algo fora do workspace, a ferramenta retorna acesso negado.

### 3. Paginação Nativa de Arquivos
- LLMs não devem ler um arquivo gigante inteiro (estoura a janela de contexto).
- Se o agente não informar `StartLine` e `EndLine`, o C# obriga a leitura de no máximo as primeiras 800 linhas, informando ao agente: *"O arquivo é muito grande, apenas as primeiras 800 linhas foram mostradas. Use paginação para ler o resto."*

## Testes e Validações
- **Teste de Path Traversal:** No xUnit, passar um caminho malicioso (ex: `../../../../secret.txt`) para o método e validar se a exceção de segurança é acionada bloqueando a leitura.
- **Leitura Paginada:** Pedir para ler as linhas 10 a 20 de um arquivo de teste e conferir se o Array de strings retornado tem exatas 11 linhas com conteúdo idêntico ao disco local.

## Critérios de Aceite (DoD)
1. A IA ganha a habilidade de vasculhar livremente qualquer arquivo do repositório local sem intervenção humana.
2. Segurança implacável contra fugas de diretório (*Directory Traversal Attacks*).
