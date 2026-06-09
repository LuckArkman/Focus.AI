# Sprint 33: Implementação da Ferramenta de Edição (Write/Edit File Tool)

## Objetivo da Sprint
Dar "mãos" ao Agente. Ele não deve apenas analisar o código, mas ser capaz de criar arquivos novos ou aplicar refatorações em arquivos existentes de forma autônoma. Essa é a ferramenta mais delicada do sistema.

## Tarefas de Desenvolvimento

### 1. Handler de Criação de Arquivos (Write File)
- Criar `WriteFileToolHandler : IAgentToolHandler`.
- Parâmetros: `TargetFile` (path absoluto), `CodeContent` (string), `Overwrite` (boolean).
- Se o arquivo e seus diretórios pais não existirem, o C# usará `Directory.CreateDirectory` para montá-los e `File.WriteAllTextAsync` para salvar.

### 2. Handler de Substituição de Código (Replace File Content)
- Criar `ReplaceFileContentToolHandler : IAgentToolHandler`.
- Parâmetros: `TargetFile`, `TargetContent` (string exata a buscar), `ReplacementContent` (string para colocar no lugar).
- Para evitar que a IA destrua código por acidente, o C# fará um `Replace` textual estrito. Se o `TargetContent` que a IA disse não existir idêntico (com mesmos espaços e quebras) no arquivo, a ferramenta retorna um Erro para a IA: *"Conteúdo alvo não encontrado. Verifique os espaços"*, obrigando a IA a pensar e tentar de novo.

### 3. Sistema de Backup Automático (Undo Capability)
- Antes de qualquer alteração física no disco feita pela IA, o backend copia o arquivo original para uma pasta oculta no projeto (`.focusai/backups/`).
- Isso é imperativo para que o usuário possa reverter facilmente as cagadas que a inteligência artificial cometer no código dele, sem depender inteiramente de commits do git.

## Testes e Validações
- Simular um pedido da IA de *Replace* onde o texto original submetido tem um "Tab" a mais. Garantir que a ferramenta rejeita a alteração para não causar corrupção cega no arquivo do cliente.

## Critérios de Aceite (DoD)
1. Agente consegue salvar novos arquivos e alterar trechos cirúrgicos de arquivos abertos.
2. Alterações destrutivas não intencionais são contidas pela validação exata do bloco a ser substituído e pelo auto-backup interno.
