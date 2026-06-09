# Sprint 03: Arquitetura Limpa, Injeção de Dependência (DI) e CQRS

## Objetivo da Sprint
Estabelecer as veias do sistema. Implementar os padrões de design CQRS (Command Query Responsibility Segregation) utilizando a biblioteca `MediatR` e preparar o container de Injeção de Dependência nativo do .NET. Isso garantirá um código testável, desacoplado e escalável conforme os recursos do Módulo 3 de IA forem adicionados.

## Tarefas de Desenvolvimento

### 1. Configuração do MediatR no Projeto Application
- Adicionar o pacote NuGet `MediatR` ao projeto `Focus.AI.Application`.
- Registrar dinamicamente o MediatR no `Program.cs` (`AddMediatR(cfg => cfg.RegisterServicesFromAssembly(...))`) escaneando o assembly da camada Application.
- Criar a estrutura de pastas padronizada dentro de `Application`: `Commands/`, `Queries/`, `Behaviors/`.

### 2. Implementação do Pipeline Behaviors (Cross-Cutting Concerns)
- Implementar um `LoggingBehavior` usando `IPipelineBehavior` do MediatR para logar o tempo de execução de todas as requisições (muito útil no futuro para medir o tempo de resposta do modelo ONNX local).
- Implementar um `ValidationBehavior` utilizando a biblioteca `FluentValidation`. Este pipeline interceptará todos os *Commands* enviados pelo front e retornará erros se a validação falhar, antes mesmo de bater no controller ou caso de uso.

### 3. Orquestração de Injeção de Dependência (DI)
- Criar classes de extensão `DependencyInjection.cs` em cada subprojeto (`Application`, `Infrastructure`, `Domain`).
- No projeto Infrastructure, criar as interfaces vazias (por enquanto) `IUserRepository` e `ISessionRepository`, além do método `services.AddInfrastructure()`.
- Unificar todas as injeções no `Program.cs` da API chamando as extensões.

### 4. Handlers Exemplo de Boas Práticas
- Criar um par simples Command/Handler (ex: `PingCommand` / `PingCommandHandler`) puramente para validar se o fluxo Endpoint -> Controller -> MediatR -> Handler está funcional e retornando a resposta esperada.

## Testes e Validações
- **Unit Testing:** Implementar testes unitários em xUnit para o `ValidationBehavior`, enviando um command inválido e garantindo que ele bloqueie o pipeline.
- **Teste de Rota:** Invocar o Minimal API que chama o `PingCommand` e confirmar se o pipeline passa corretamente pelos comportamentos configurados (incluindo geração de log).

## Critérios de Aceite (DoD)
1. Qualquer regra de negócio só pode ser executada por intermédio de um *Command* ou *Query* do MediatR.
2. Injeção de dependência orquestrada de forma limpa, sem entulhar o `Program.cs`.
3. Validações FluentValidation impedindo execuções de comandos incorretos de forma global.
