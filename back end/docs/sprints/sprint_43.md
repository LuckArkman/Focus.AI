# Sprint 43: Criação do Mapa Mental do Projeto (Workspace Skeleton)

## Objetivo da Sprint
Fornecer à IA um "Mapa do GPS". Antes de começar a resolver um bug, o agente pede para ver a estrutura do projeto. Ler 500 arquivos de código quebra o modelo, mas ler o *Skeleton* (apenas a declaração de classes e funções, sem o miolo lógico) custa quase nada em tokens e dá contexto total.

## Tarefas de Desenvolvimento

### 1. A Tool de Mapa do Projeto (`GetProjectSkeletonTool`)
- Criar `ProjectSkeletonToolHandler : IAgentToolHandler`.
- O Agente invoca essa ferramenta passando o caminho de uma pasta (ex: `src/Controllers`).

### 2. Varredura Local Rápida
- O C# usará `Directory.EnumerateFiles` na pasta ignorando `.git`, `node_modules` e `bin`.
- Para cada arquivo válido encontrado (em paralelo usando `Task.WhenAll`), o C# chamará o `AstParserService` (Sprint 42).
- Construir uma String de saída comprimida e otimizada.
  *Exemplo de Output da Tool:*
  ```text
  📄 UserController.cs
    class UserController
      method GetUsers() -> Task<IActionResult>
      method CreateUser(UserDto) -> Task<IActionResult>
  📄 AuthController.cs
    class AuthController
      method Login() -> Task<IActionResult>
  ```

### 3. Caching de Skeleton
- Fazer parse de 5.000 arquivos cada vez que o agente pede o esqueleto demora alguns segundos.
- Salvar a saída do esqueleto do repositório inteiro no banco (MongoDB ou Redis local) na primeira vez, e invalidar a parte afetada apenas quando o `FileSystemWatcher` detectar salvamento físico no disco ou quando a ferramenta de Edição do Agente modificar o arquivo.

## Testes e Validações
- Pegar um micro-projeto Node.js mockado em disco (10 arquivos). Chamar a ferramenta. Conferir se a string gerada descreve fielmente a hierarquia de todos os métodos do projeto de forma resumida, sem exibir lógica de implementação de nenhuma função.

## Critérios de Aceite (DoD)
1. O Agente consegue visualizar toda a arquitetura e nomenclatura de classes do projeto do desenvolvedor gastando uma fração pífia dos tokens da sua janela de contexto.
2. Arquivos perigosos ou pesados (binários, minificados) são ignorados proativamente.
