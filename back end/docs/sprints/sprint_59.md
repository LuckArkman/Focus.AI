# Sprint 59: LSP Logs & Telemetry e Painel de Depuração

## Objetivo da Sprint
Um servidor Language Server pode travar internamente e a extensão do lado do TypeScript do usuário simplesmente para de mostrar autocompletes sem motivo óbvio. Precisamos de logs bilaterais enviados ao Editor e à API.

## Tarefas de Desenvolvimento

### 1. Uso do `ILanguageServerFacade.Window`
- Em vez de usar apenas o Serilog padrão (que fica na tela preta do C#), usar os envios de notificação oficial do LSP.
- Quando a conexão com Qdrant cair (Sprint 19) no backend, injetar também via LSP: `window/showMessage` enviando "Erro: O banco vetorial Qdrant falhou. As buscas de RAG ficarão lentas".
- O VS Code exibirá o popup de erro no canto inferior direito para o desenvolvedor saber o que está ocorrendo nas entranhas locais sem precisar ler logs de C#.

### 2. Canal de Saída Customizado
- Enviar logs detalhados do processamento de tokens para o painel `Output > Focus.AI Language Server` na IDE.

## Testes e Validações
- Levantar erro forçado ao carregar um modelo ONNX com caminho errado. Validar que o protocolo de LSP empurra a mensagem proativa para cima (LogMessage Notifier).

## Critérios de Aceite (DoD)
1. Alta visibilidade e transparência para o usuário da Extensão (Frontend), mesmo quando a API Backend dá uma falha silenciosa tratada pelo GlobalException.
