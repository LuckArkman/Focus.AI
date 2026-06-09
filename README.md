# Focus.AI - Copiloto Autônomo e Ambiente de Desenvolvimento de IA Local

Bem-vindo ao **Focus.AI**, uma plataforma de inteligência artificial de última geração projetada para atuar não apenas como um assistente de código (copilot), mas como um **Agente Autônomo** capaz de raciocinar, navegar, modificar e compreender repositórios complexos de forma inteiramente local.

Diferente das soluções baseadas em nuvem, o Focus.AI roda o processamento neural diretamente na máquina do desenvolvedor, oferecendo latência zero, total privacidade de dados e recursos avançados de análise de árvores sintáticas.

---

## 🎯 Visão Geral do Produto

O Focus.AI se divide em duas partes fundamentais que trabalham em uníssono:
1. **O Motor Principal (Backend .NET 8):** O "cérebro" do sistema, que hospeda a lógica de inferência, o controle de memória vetorial, o gerenciamento de permissões e atua como o servidor de linguagem (LSP).
2. **A Interface do Desenvolvedor (VS Code Extension em TypeScript):** As "mãos e os olhos" do sistema, vivendo dentro do editor de código do desenvolvedor, providenciando o *Ghost Text* translúcido, chat em tempo real e comandos rápidos de ação.

---

## 🏗️ Arquitetura e Stack Tecnológico

O Focus.AI foi projetado em torno de uma arquitetura limpa (Clean Architecture), CQRS e separação de módulos lógicos. A stack principal consiste em:

### 1. Inferência de IA (Local e Dinâmica)
- **Modelos:** Qwen3 (1.5B para latência ultra-baixa no Autocomplete; 27B para raciocínio denso e agente autônomo).
- **Engine de Inferência:** `Microsoft.ML.OnnxRuntime.Gpu` utilizando aceleração nativa via CUDA/DirectML.
- **Roteamento Inteligente:** Decodificação especulativa e alocação dinâmica (VRAM vs RAM) baseada na capacidade de hardware do usuário.

### 2. Ecosistema de Dados (Persistência Híbrida)
- **Relacional (PostgreSQL + EF Core):** Gerenciamento transacional rígido de Workspaces, permissões de usuários e metadados.
- **Memória de Longo Prazo (Qdrant):** Banco de dados vetorial de alto desempenho para armazenar o contexto do código, gerando embeddings contínuos via RAG Híbrido.
- **Estado e Sessão (MongoDB):** Armazenamento NoSQL ultrarrápido para salvar o histórico de conversas, dumps de pensamento da IA e persistência do protocolo de mensagens.

### 3. Comunicação em Tempo Real e Integração com IDE
- **LSP (Language Server Protocol):** OmniSharp.Extensions para expor diagnósticos, Code Actions e Inline Completions diretamente ao motor nativo do VS Code.
- **SignalR (WebSockets):** Tráfego bidirecional de streaming de tokens para as interfaces de chat sem bloqueio HTTP.

### 4. Análise de Código e Segurança
- **Tree-sitter:** Bindings nativos em C# para realizar o parseamento de Código para Abstract Syntax Trees (AST). Permite à IA "ver" a estrutura do código em grafos e não apenas prever texto estatisticamente.
- **Sandboxing:** Mecanismo rígido para a execução de processos de terminal da IA (`Run Command`), impedindo alterações deletérias forçadas na máquina do programador.

---

## 🗺️ Roadmap de Desenvolvimento (75 Sprints)

O desenvolvimento do Focus.AI foi minunciosamente planejado em uma maratona de 75 Sprints de alto detalhamento técnico. O ciclo de vida da construção cobre os seguintes blocos principais:

### Fase 1: Infraestrutura e Fundações (Sprints 1 a 12)
Configuração do contêiner Docker Compose (PostgreSQL, Mongo, Qdrant), estruturação do Injeção de Dependências em .NET 8, JWT, Entity Framework, e arquitetura de repositórios base.

### Fase 2: Persistência Avançada e Vetorização RAG (Sprints 13 a 25)
Criação dos Workspaces virtuais. Ingestão de dados do projeto de código, estratégias de chunking (fatiamento) sintático para código fonte e comunicação via gRPC com o motor Qdrant.

### Fase 3: Processamento ONNX e IA Local (Sprints 26 a 40)
O coração da IA: download de modelos Qwen3, carregamento na VRAM via ONNX Runtime, geração de streaming de texto (IAsyncEnumerable) e o sofisticado roteamento Speculative Decoding entre modelos menores e maiores.

### Fase 4: O Agente Autônomo (Módulo 3) (Sprints 41 a 55)
Implementação do ReAct Loop, dando à IA ferramentas dinâmicas (Function Calling) para: ler arquivos, escrever no sistema, executar comandos de terminal isolados (Sandbox) e gerenciar sub-agentes especialistas em tarefas longas em background.

### Fase 5: Análise Profunda com Tree-sitter (Sprints 56 a 65)
Integração profunda de AST, habilitando a IA a entender dependências (imports/requires), criar grafos do projeto, identificar referências (find usages) e aplicar proteções de Self-Healing para erros de sintaxe gerados.

### Fase 6 e 7: Protocolo LSP e Integração VS Code (Sprints 66 a 75)
A ponte para a produtividade: construção do servidor LSP no .NET e desenvolvimento da Extensão front-end em TypeScript. Integra o "Ghost Text", painel lateral dinâmico, Quick Fixes (lâmpada amarela) e sincronização offline-first do histórico de conversões do desenvolvedor.

---

## 🛡️ Segurança e Privacidade (Offline-First)

Como uma ferramenta voltada para propriedades intelectuais corporativas rigorosas, o Focus.AI assegura que:
- **Zero-Telemetry Externa:** O código fonte processado nunca sai do ambiente local (localhost) do desenvolvedor ou de sua intranet.
- **Previsibilidade:** Todas as ações que envolvem alteração em lote de arquivos ou comandos de bash passam por validações rígidas de Sandbox e solicitam confirmação humana na IDE (Human-in-the-loop) para ações perigosas.

---

## 🚀 Como Iniciar (Acesso à Documentação)

Para compreender em detalhes a matemática, os padrões arquiteturais e o fluxo de implementação de cada etapa descrita acima, os desenvolvedores devem consultar a documentação específica gerada no diretório:

`/back end/docs/sprints/`

Cada arquivo markdown de sprint lista especificamente os pacotes NuGet/NPM necessários, estratégias de teste, código crítico de implementação e Critérios de Aceite (Definition of Done).