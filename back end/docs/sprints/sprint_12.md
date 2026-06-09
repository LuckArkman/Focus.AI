# Sprint 12: Desenvolvimento da API RESTful de Workspaces (Projetos)

## Objetivo da Sprint
Expor a lógica de negócios da Sprint 11 por meio de uma API Web segura. O VS Code Extension precisará chamar esses endpoints na primeira vez em que abrir uma pasta para "registrar" ou "carregar" aquele workspace no backend do Focus.AI.

## Tarefas de Desenvolvimento

### 1. Criação do Endpoint Controller/Minimal API
- Criar o `ProjectsController` (ou rotas Minimal API no `Program.cs` sob o grupo `/api/projects`).
- Anotar os métodos com `[Authorize]` para barrar não logados.

### 2. Rotas e Retornos
- Rota `POST /api/projects`: Injeta o `IMediator`, envia o `CreateProjectCommand` (recuperando o UserId de forma segura via `HttpContext.User.Claims`).
- Rota `GET /api/projects`: Recupera a lista com os dados do projeto.
- Rota `GET /api/projects/{projectId}/status`: Uma rota preliminar para, no futuro, retornar se os vetores (Qdrant) do projeto atual já foram 100% carregados em memória.

### 3. Obtenção Segura de Claims JWT
- Criar um *Extension Method* `ClaimsPrincipalExtensions.GetUserId()` para extrair o Guid do JWT de forma limpa, evitando a repetição do `Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)` por toda a controller.

## Testes e Validações
- **Postman / Swagger:** Logar na API (Sprint 5) recebendo o JWT, colar o JWT no cabeçalho.
- Fazer um POST mandando `{ "name": "Teste", "localPath": "C:\\dev\\teste" }`.
- O banco Postgres deve persistir a associação correta e devolver o HTTP 201 Created.

## Critérios de Aceite (DoD)
1. Endpoints HTTP blindados por Autorização que mapeiam 1:1 para os Casos de Uso (Commands) criados.
2. Extração limpa e livre de falhas de segurança da Identidade do requisitante.
