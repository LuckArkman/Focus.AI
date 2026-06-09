# Sprint 11: Implementação dos Casos de Uso de Gestão de Projetos

## Objetivo da Sprint
O assistente atua no nível de *Workspaces* ou *Projetos* dentro do VS Code. Para isso, o backend precisa mapear logicamente quais projetos pertencem a qual usuário. Esta sprint focará na camada de `Application` e `Domain`, construindo a lógica de negócio (CQRS) para a criação e gerência de Projetos.

## Tarefas de Desenvolvimento

### 1. Entidade de Domínio "Project"
- Criar a classe `Project` no `Focus.AI.Domain`.
- Propriedades: `Id` (Guid), `Name` (ex: focus.ai), `LocalPath` (ex: i:\Focus.AI), `LanguageStack` (string ou enum, ex: C#, Node), `UserId` (Guid), `CreatedAt`.

### 2. Mapeamento no EF Core (Infrastructure)
- Adicionar `DbSet<Project>` no `ApplicationDbContext`.
- Configurar `IEntityTypeConfiguration<Project>`, criando a chave estrangeira (Foreign Key) virtual ligando `Project.UserId` -> `User.Id`.
- Rodar nova `Migration` via CLI do EF Core e aplicar no banco.

### 3. Repositório de Projetos
- Criar a interface `IProjectRepository` e implementá-la em `ProjectRepository`.
- Adicionar os métodos básicos: buscar por usuário, adicionar projeto, verificar se o diretório local já está mapeado para aquele usuário.

### 4. Commands e Queries (MediatR)
- **CreateProjectCommand:** Recebe `Name`, `LocalPath` e `LanguageStack`. O Handler verifica se o usuário já não possui esse projeto (pelo path) e salva.
- **GetProjectsByUserQuery:** Handler que busca no banco todos os projetos daquele `UserId` (extraído do Token).
- Validações: Usar FluentValidation para impedir `LocalPath` nulo ou nomes em branco.

## Testes e Validações
- **Testes Unitários do Handler:** Criar Mock do `IProjectRepository`, enviar um `CreateProjectCommand` válido e atestar que o repositório disparou a função `.AddAsync()` corretamente. Garantir que tenta inserir um projeto já existente lance exceção de negócio.

## Critérios de Aceite (DoD)
1. Lógica de negócio via MediatR isolada sem acesso direto aos DbContexts nos controllers.
2. Tabela de Projetos criada fisicamente no PostgreSQL e vinculada por chaves relacionais aos Usuários.
