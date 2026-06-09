# Sprint 05: Autenticação JWT e Gestão de Identidade

## Objetivo da Sprint
Proteger a API contra acessos não autorizados. Nesta sprint será implementado o fluxo completo de registro de usuário, geração de hashes seguros para senha e a emissão de JSON Web Tokens (JWT) que a Extensão do VS Code e a Interface Web utilizarão para se comunicar com o modelo de IA futuramente.

## Tarefas de Desenvolvimento

### 1. Fluxo de Registro de Usuário (RegisterCommand)
- Criar o *Command* `RegisterUserCommand` recebendo `Email`, `Name` e `Password`.
- Criar o Handler respectivo que:
  - Valida (via FluentValidation) se a senha é forte e o e-mail tem formato correto.
  - Verifica no `IUserRepository` se o e-mail já existe (gera erro de conflito caso exista).
  - Gera o hash da senha utilizando a biblioteca `BCrypt.Net-Next`.
  - Salva a entidade `User` no PostgreSQL.

### 2. Fluxo de Login e Geração JWT (LoginQuery)
- Criar o *Query* `LoginQuery` recebendo `Email` e `Password`.
- Criar serviço interno `JwtTokenGenerator` responsável por criar o payload.
- No Handler: buscar usuário, bater o hash (BCrypt `Verify`), e em caso de sucesso chamar o serviço JWT.
- O token gerado deverá conter as claims de `UserId` e `Role` e expirar em tempo adequado (ex: 8 horas). Opcional: Implementar o conceito de `RefreshToken` se a extensão do VS Code exigir ficar logada indefinidamente.

### 3. Middleware de Autenticação JWT no .NET
- Adicionar o pacote `Microsoft.AspNetCore.Authentication.JwtBearer`.
- Configurar o `services.AddAuthentication` e `AddJwtBearer` no `Program.cs`.
- Ler variáveis críticas como o *Issuer*, *Audience* e o `SecretKey` do `appsettings.json` e repassá-las ao validador do JWT (TokenValidationParameters).

### 4. Proteção de Rotas (Endpoints)
- Criar controllers genéricos com a anotação `[Authorize]` para testar o bloqueio.
- Expor duas rotas públicas essenciais Minimal API ou Controllers: `POST /api/auth/register` e `POST /api/auth/login`.

## Testes e Validações
- **Testes Unitários:** Testar o gerador de JWT mockando o usuário e testando se a claim gerada no Payload do token tem o `UserId` esperado.
- **Teste de Request:** Usar o Swagger para enviar um request não autenticado e garantir que a aplicação devolva `401 Unauthorized`.
- **Fluxo End-to-End:** Fazer um registro usando JSON, fazer o login em seguida, pegar o token gerado, injetá-lo no header `Authorization: Bearer <token>` e tentar acessar o endpoint de teste com sucesso devolvendo `200 OK`.

## Critérios de Aceite (DoD)
1. Nenhuma rota sensível pode ser acessada sem um token JWT válido assinado pela chave da API.
2. Senhas não são armazenadas em plain-text sob nenhuma circunstância.
3. Tratamento de erro explícito para e-mails duplicados e senhas incorretas.
