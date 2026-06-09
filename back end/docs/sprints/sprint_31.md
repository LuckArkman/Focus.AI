# Sprint 31: Arquitetura Base de Function Calling (Tool Use)

## Objetivo da Sprint
IAs por si só não têm mãos; elas apenas cospem texto. Para que o Focus.AI seja um agente autônomo (Módulo 3) como o Devin ou Cursor, o modelo precisa saber que possui "ferramentas" à disposição. Esta sprint foca em construir a infraestrutura em C# para passar o esquema de *Tools* (Function Calling) no JSON do modelo Qwen3 e capturar a resposta quando ele decide usar uma ferramenta.

## Tarefas de Desenvolvimento

### 1. Modelagem do Esquema de Ferramentas (JSON Schema)
- Criar a classe `AgentTool` na camada `Application`.
- A classe deve conter: `Name`, `Description`, `Parameters` (um JsonSchema puro descrevendo os tipos de entrada da ferramenta).
- O Qwen3-Coder suporta *Function Calling* nativamente no seu formato de ChatML. Criar um serializador que pega as instâncias de `AgentTool` e as anexa ao System Prompt no formato que o modelo entende.

### 2. Interceptador de Resposta (Tool Call Parser)
- Modificar o `OnnxChatClient` (Streaming).
- Se a IA começar a responder no formato `{"tool_calls": [...] }` ou com a tag específica de chamada do Qwen, o C# **não** deve mandar esse JSON quebrado para a tela do usuário.
- O C# deve interceptar esse texto, fazer o *Parse* do JSON, e entender que a IA pausou a fala para pedir a execução de uma função no servidor.

### 3. Registro Dinâmico de Ferramentas via Reflection
- Criar a interface `IAgentToolHandler`.
- Usar *Reflection* no `Program.cs` para escanear todas as classes que implementam `IAgentToolHandler` e injetá-las no contêiner de dependências automaticamente.

## Testes e Validações
- **Teste de Mapeamento:** Passar um schema de função `GetWeather(location)` pro modelo ONNX através do prompt. Perguntar "Como está o tempo em São Paulo?". O modelo não deve responder texto livre, mas sim retornar um JSON válido de Tool Call direcionado à função `GetWeather`. Validar se o parser de C# capturou esse pedido corretamente.

## Critérios de Aceite (DoD)
1. O backend .NET consegue apresentar ferramentas de forma dinâmica pro modelo.
2. Pedidos de acionamento de ferramentas gerados pela IA são interceptados silenciosamente no servidor, sem poluir a interface do usuário.
