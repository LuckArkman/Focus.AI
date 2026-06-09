# Sprint 46: Identificação de Dependências (Imports/Requires)

## Objetivo da Sprint
A compreensão do "Grafo de Dependências" começa mapeando o cabeçalho dos arquivos. Saber que o arquivo A usa o Arquivo B. O Focus.AI fará isso debaixo dos panos, sem intervenção do agente.

## Tarefas de Desenvolvimento

### 1. Queries de Importação no Tree-sitter
- Criar novos arquivos de Query Lisp (`.scm`) para extrair dependências file-to-file.
- Para C#: `using Focus.AI.Models;`
- Para JS/TS: `import { User } from './user';` ou `require('fs');`

### 2. Varredor em Background
- Sempre que a extensão do VS Code informar ao Backend que um novo projeto foi carregado, iniciar o `ProjectIndexingService` (Background Worker no C#).
- Ele lê as primeiras 50 linhas de todo arquivo. Bate a query do Tree-sitter e extrai as importações cruzadas.

### 3. Normalização de Caminhos
- O JavaScript usa imports relativos (`../user.js`). O C# usa Namespaces.
- Criar o resolver C# que mapeia que um Namespace `Focus.AI.Domain` aponta logicamente para o diretório físico `Focus.AI/Domain/`. Isso é complexo mas imperativo para a precisão da IA.

## Testes e Validações
- Colocar arquivos JS locais que se referenciam com `./`, `../` e `@/components`. O serviço de normalização deverá transformar tudo em caminhos absolutos locais provando que o Arquivo A do diretório X realmente depende do Arquivo B no diretório Y.

## Critérios de Aceite (DoD)
1. Mapeamento preciso de quem-chama-quem nos cabos de rede do repositório do usuário, resolvendo ambiguidade entre diretórios virtuais e físicos.
