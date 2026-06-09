# Sprint 22: Download e Validação Local dos Modelos Qwen3 (ONNX)

## Objetivo da Sprint
O core deste projeto é a execução 100% offline. Para isso, o backend deve ser capaz de gerenciar fisicamente os arquivos pesados dos modelos (Qwen3 1.5B e 27B quantizados).

## Tarefas de Desenvolvimento

### 1. Sistema de Gerenciamento de Arquivos
- Criar a classe `LocalModelManager` no `Infrastructure`.
- Essa classe deve varrer um diretório específico configurado no `appsettings.json` (ex: `C:\Models\Focus.AI`) em busca dos arquivos de peso (.onnx, tensores, vocab.txt, tokenizer.json).

### 2. Validação Criptográfica (Opcional, porém Recomendado)
- Ler metadados ou realizar checagem simples de tamanho de arquivo ou Hash MD5 para garantir que o modelo de 15GB não foi corrompido durante o download pelo desenvolvedor.

### 3. Mecanismo de Alerta (Pre-flight check)
- No `Program.cs`, antes de iniciar a API web (no `app.Run()`), rodar um host service chamado `ModelPreFlightService`.
- Se o modelo `Qwen3-1.5B.onnx` ou o `Qwen3-27B.onnx` não forem encontrados, a API de backend **NÃO** deve quebrar subitamente; em vez disso, ela inicia normalmente, porém emite logs de ALERTA VERMELHO e envia via SignalR uma mensagem para o VS Code: *"Modelos ausentes. Acesse a tela de configurações para realizar o download."*

## Testes e Validações
- Excluir propositalmente o arquivo de modelo local. Dar boot na aplicação. Verificar se o Serilog acusou corretamente o erro sem causar exceção fatal na API.

## Critérios de Aceite (DoD)
1. Gerenciamento seguro de arquivos de dezenas de gigabytes sem estouro de RAM no C# (apenas verificação de ponteiros e sistema de arquivos).
2. O sistema avisa o frontend sobre a ausência de modelos via WebSocket para ação do usuário.
