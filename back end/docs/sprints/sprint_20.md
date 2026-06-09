# Sprint 20: Polimento da Fase 2 e Otimização Avançada de Índices

## Objetivo da Sprint
O encerramento da Fase 2, focada na Persistência. Os bancos de dados precisam de refinamentos para se saírem bem em ambiente real com dezenas ou centenas de chats diários criados por um engenheiro em uma longa semana de trabalho com IA. Otimização de busca será o foco.

## Tarefas de Desenvolvimento

### 1. Índices (Indexes) no MongoDB
- Utilizar a CLI interna de inicialização do backend para criar **Índices Composto e Simples** dinâmicos no MongoDB (para evitar buscas do tipo Table Scan "sujas").
- Criar Índice na coleção de Sessões: Buscar muito mais rápido a chave composta (`ProjectId`, `UserId`).
- Criar Índice na coleção de Mensagens: Indicar `Timestamp` como decrescente, já que o bot sempre busca "as últimas 10 mensagens" daquele projeto.

### 2. Otimização de Índices (HNSW) no Qdrant
- O Qdrant usa índices HNSW (Hierarchical Navigable Small World) para vetores, e permite criar *Índices de Payload*.
- Por API gRPC, comandar a engine em Rust do Qdrant para criar um `Payload Index` explícito para o campo `user_id`. Isso faz com que antes da busca vetorial matemática iniciar, o Qdrant já ignore fisicamente os discos e blocos dos outros usuários, não ocupando CPU da máquina que hospeda o ambiente local.

### 3. Paginação e Caching no Application (MediatR)
- Revisar as Queries criadas (ex: listar projetos ou sessões).
- Implementar o padrão `IPipelineBehavior` para **Caching** em memória (IMemoryCache) caso o usuário aperte para carregar a lista de chats muitas vezes, poupando bateria do notebook nas requisições desnecessárias.

### 4. Refatoração e Code Review Técnico
- Rodar varreduras com a extensão do Roslyn Analyzers em todo o código .NET para procurar por vazamentos de tarefas assíncronas mal geridas (o temido *Fire-and-Forget* que dá throw em threads perdidas).
- Avaliar fechamento explícito de blocos IDisposable ou `using` na manipulação dos Tensors no código ONNX.

## Testes e Validações
- **Teste de Explanação (Explain Query):** Ativar perfis detalhados nos logs do Mongo ou Qdrant e validar as consultas rodando para garantir que os *Indexes* estão ativando e o banco não acusa varredura lenta.

## Critérios de Aceite (DoD)
1. Fim da Fase 2: Infraestrutura de dados completa, testada sob carga de stress, persistente aos reboots (containers), indexada. O servidor se provou pronto para finalmente acomodar e orquestrar os LLMs enormes a partir da próxima fase.
