# Sprint 60: Encerramento do Módulo 2 e Testes de Servidor Oculto

## Objetivo da Sprint
O Backend (Módulo 1), O Agente (Módulo 3) e o Language Server Bridge (Módulo 2) estão perfeitamente modelados. O fim da Fase 6 testa a compilação do executável que será despachado pro usuário final da ferramenta.

## Tarefas de Desenvolvimento

### 1. Validação de Binário (Publish SingleFile)
- O desenvolvedor que vai usar o Focus.AI não precisa baixar o código C#; ele vai receber um `.exe` empacotado que sobe tudo.
- Configurar o `.csproj` do Focus.AI.Api com flags de otimização de release AOT (opcional) ou Self-Contained.
  `<PublishSingleFile>true</PublishSingleFile>`
- Garantir que as *DLLs* nativas do CUDA/OnnxRuntime e do Tree-sitter em "C" sejam copiadas estaticamente ou empacotadas de forma correta pelo Publish do msbuild sem gerar arquivos corrompidos na máquina cliente.

### 2. Stress Test da Execução Pura
- Rodar o `.exe` compilado em ambiente de produção local.
- Lançar pacotes e requisições mistas.

## Testes e Validações
- Empacotar e descompactar o backend em uma pasta limpa. Executar usando a CLI do PowerShell. Bater num teste básico via porta mapeada confirmando que o Kestrel escutou, ligou as bibliotecas não-gerenciadas C/C++ sem reclamar de "DLLNotFoundException".

## Critérios de Aceite (DoD)
1. Fase 6 finalizada. Conexão profunda com a ide completada via protocolo padrão.
2. Garantia de portabilidade de publicação (Release build funcional, com links estáticos ok para bibliotecas pesadas de IA).
