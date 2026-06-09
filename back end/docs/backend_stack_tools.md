# Ferramentas, Bibliotecas e Tecnologias Essenciais do Backend

Esta lista cobre exaustivamente a infraestrutura e pacotes necessários para a construção do **Focus.AI** Backend, baseado em **.NET 8**.

## 1. Core Framework & Arquitetura
*   **`.NET 8.0 SDK`**: Base do projeto, utilizando as otimizações do runtime e suporte nativo a AOT (Ahead-of-Time).
*   **`Microsoft.AspNetCore.App`**: Metapacote padrão para Minimal APIs e controladores da Web API.
*   **`MediatR`**: Biblioteca para implementação do padrão CQRS e Mediator. Desacopla controladores da regra de negócio (essencial para a Clean Architecture).

## 2. Acesso a Dados (Bancos SQL e NoSQL)
### PostgreSQL (Relacional - Contas e Autenticação)
*   **`Microsoft.EntityFrameworkCore`**: ORM oficial do .NET.
*   **`Npgsql.EntityFrameworkCore.PostgreSQL`**: Provider específico do Postgres para o EF Core.
*   **`Microsoft.EntityFrameworkCore.Design`**: Necessário para migrations via CLI (Code-First).

### MongoDB (Orientado a Documentos - Sessões e Histórico de Chat)
*   **`MongoDB.Driver`**: Driver oficial. Manipulação assíncrona dos threads de interação, BSON Mapping.

### Qdrant (Vetorial - Memória de Longo Prazo e RAG)
*   **`Qdrant.Client`**: Cliente oficial gRPC do Qdrant para .NET. Permite inserções rápidas de vetores, busca por Similaridade de Cosseno e filtros de payload para Multi-tenancy (separação por *user_id*).

## 3. Inteligência Artificial e Inferência Local
*   **`Microsoft.Extensions.AI`**: Nova biblioteca padrão da Microsoft para abstrair clientes de chat e LLMs (interface `IChatClient`), permitindo trocar facilmente a implementação.
*   **`Microsoft.ML.OnnxRuntime.Gpu`**: Motor de inferência C++ empacotado para C#, vital para rodar os modelos **Qwen3 1.5B e 27B** em formato ONNX utilizando a placa de vídeo.
*   **`Microsoft.ML.Tokenizers`**: Biblioteca para controle preciso de tokens e chunking dos textos enviados ao modelo, vital para não estourar a janela de contexto de longo prazo.

## 4. Comunicação e Integração VS Code (Módulo 2)
*   **`Microsoft.AspNetCore.SignalR`**: Comunicação em tempo real full-duplex entre o frontend web e o backend, utilizada para stream de tokens de IA gerados em tempo real (estilo ChatGPT).
*   **`OmniSharp.Extensions.LanguageServer`**: Implementação do protocolo LSP (Language Server Protocol). Ficará hospedado no backend e será consumido nativamente pela extensão do VS Code para *inline completions* e *diagnostics*.

## 5. Análise de Código e Agente Autônomo (Módulo 3)
*   **`TreeSitter.Bindings`** (ou wrapper customizado sobre o C-API do Tree-sitter): Parseador incremental de Abstract Syntax Trees (AST). Necessário para o Agente entender a estrutura das classes de um projeto lido do disco sem precisar depender unicamente do LLM para deduzir lógica de código.
*   **`LibGit2Sharp`**: Interagir com o repositório Git local do usuário. Identificar branches, commits, ou arquivos com `git diff` não submetidos para guiar o Agente em correções locais.

## 6. Segurança e Autenticação
*   **`Microsoft.AspNetCore.Authentication.JwtBearer`**: Implementação do middleware de tokens JWT para rotas restritas.
*   **`BCrypt.Net-Next`**: Hash seguro para as senhas salvas no PostgreSQL, caso o projeto use identidade própria em vez de OIDC.

## 7. Observabilidade, Logs e Tratamento de Erros
*   **`Serilog.AspNetCore`**: Substitui o log padrão do .NET. Gera logs estruturados (JSON).
*   **`Serilog.Sinks.Console`** e **`Serilog.Sinks.File`**: Para visualização em tela e salvamento rotativo dos logs gerados pelo orquestrador de IA.
*   **`FluentValidation.AspNetCore`**: Validação rica de dados de entrada antes que cheguem à camada de serviço.

## 8. Testes (QA & Reliability)
*   **`xUnit`**: Framework base de testes unitários e de integração.
*   **`Moq`**: Para simular abstrações e serviços que não queremos instanciar em memória.
*   **`FluentAssertions`**: Sintaxe semântica para declarações nos testes (ex: `result.Should().BeTrue()`).
*   **`Testcontainers`** e **`Testcontainers.PostgreSql` / `Testcontainers.MongoDb` / `Testcontainers.Qdrant`**: Sobe containers Docker descartáveis automaticamente ao iniciar a suite de testes, executando um teste 100% isolado do ambiente real.

## 9. Docker e Orquestração
*   **`Docker Compose`**: Responsável por levantar de uma só vez:
    *   API Backend (.NET)
    *   PostgreSQL
    *   MongoDB
    *   Qdrant
    *   Redis (opcional para Cache de queries).
