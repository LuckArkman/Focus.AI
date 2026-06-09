# Sprint 04: Implementação do PostgreSQL com Entity Framework Core

## Objetivo da Sprint
Fornecer ao sistema a capacidade de persistir dados transacionais robustos. Nesta sprint, focaremos no módulo de identidade do usuário e suas permissões (Roles). O Entity Framework Core será mapeado para salvar essas tabelas no container do PostgreSQL previamente configurado.

## Tarefas de Desenvolvimento

### 1. Modelagem do Domínio Base (User Entity)
- No projeto `Focus.AI.Domain`, criar a entidade `User` e `Role` (ou herdar de *IdentityUser* caso prefira usar o Microsoft Identity, mas o padrão manual é recomendado para controle rígido).
- Propriedades de `User`: `Id` (Guid), `Email`, `PasswordHash`, `Name`, `CreatedAt`, `IsActive`.

### 2. Setup do EF Core no Infrastructure
- Adicionar os pacotes NuGet `Microsoft.EntityFrameworkCore.Design` (no projeto API) e `Npgsql.EntityFrameworkCore.PostgreSQL` (no projeto Infrastructure).
- Criar a classe `ApplicationDbContext` herdando de `DbContext`.
- Configurar os DbSets (`public DbSet<User> Users { get; set; }`).

### 3. Mapeamento Fluente (Fluent API)
- Para evitar sujar as entidades de Domínio com *Data Annotations*, criar configurações usando `IEntityTypeConfiguration<User>` no projeto de Infrastructure.
- Garantir que o e-mail seja marcado como `IsUnique` e ter índices.
- Configurar tamanho máximo de varchar (ex: `HasMaxLength(255)`).

### 4. Implementação de Migrations Automáticas
- Executar no terminal: `dotnet ef migrations add InitialCreate -s Focus.AI.Api -p Focus.AI.Infrastructure`.
- Adicionar lógica no `Program.cs` para, em ambiente de Dev (ou via variável de ambiente no docker), rodar `dbContext.Database.MigrateAsync()` automaticamente no boot para criar a estrutura no PostgreSQL.

### 5. Padrão Repositório (Repository Pattern)
- Criar a interface `IUserRepository` no Domain.
- Implementar `UserRepository` no Infrastructure com os métodos `GetByIdAsync`, `GetByEmailAsync`, `AddAsync` e `UpdateAsync`.
- Registrar a injeção (`services.AddScoped<IUserRepository, UserRepository>()`).

## Testes e Validações
- **Testcontainers Integrado:** Configurar um projeto `Focus.AI.IntegrationTests`. Levantar um container do postgres real no teste usando `Testcontainers`, aplicar migrations dinamicamente e salvar um usuário verificando se não gera erro (Validando as chaves únicas).
- **Teste Manual:** Dar o boot no Docker do projeto real e olhar pelo DBeaver se a tabela `Users` foi perfeitamente desenhada.

## Critérios de Aceite (DoD)
1. Conexão com o banco PostgreSQL lendo da variável `ConnectionStrings:DefaultConnection`.
2. O sistema é capaz de aplicar migrations de banco vazias e criar as tabelas.
3. Testes de integração validando o CRUD do `UserRepository` contra um banco postgres *in-memory* ou container de teste.
