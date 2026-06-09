# Sprint 62: Empacotamento VSIX, Webpack e Dependências da Extensão

## Objetivo da Sprint
Configurar o processo de build para produção da extensão do VS Code utilizando Webpack, otimizando o tamanho do pacote e empacotando os artefatos em um arquivo `.vsix` distribuível.

## Tarefas de Desenvolvimento

### 1. Integração com Webpack
- Adicionar dependências de desenvolvimento necessárias (`webpack`, `webpack-cli`, `ts-loader`) no `package.json` da extensão.
- Escrever o arquivo `webpack.config.js` para compilar todo o código TypeScript em um único bundle unificado `dist/extension.js`, otimizando o tempo de carregamento da extensão.
- Adicionar scripts de build no `package.json`: `compile` (desenvolvimento) e `vscode:prepublish` (produção).

### 2. Configuração do `.vscodeignore` e Metadados do Pacote
- Configurar o arquivo `.vscodeignore` para garantir que arquivos fonte brutos (`.ts`), documentação, pastas de teste e `node_modules` sejam excluídos do pacote final da extensão.
- Definir os metadados corretos no `package.json` (versão, ícone, repositório, nome de exibição e categorias).

### 3. Geração do Pacote VSIX
- Instalar e configurar a ferramenta global `vsce` (VS Code Extension Manager).
- Configurar um script que empacota opcionalmente os binários compilados do backend C# dentro de uma subpasta `bin/` da extensão de acordo com o target operacional.
- Executar o empacotamento completo com `npx vsce package` gerando o arquivo final `.vsix`.

## Testes e Validações
- **Teste de Instalação Local:** Instalar o arquivo `.vsix` gerado manualmente no VS Code local (`code --install-extension focus-ai-0.1.0.vsix`) e validar se a extensão carrega sem erros de runtime ou dependências faltantes.
- **Validação de Tamanho:** Verificar se o tamanho do pacote `.vsix` está em conformidade com o esperado e não contém arquivos de desenvolvimento.

## Critérios de Aceite (DoD)
1. Webpack configurado e gerando bundle de produção sem erros de compilação.
2. Comando `vsce package` executando com sucesso e gerando o arquivo `.vsix` funcional.
