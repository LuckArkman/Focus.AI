# Sprint 08: Padronização de Logs Estruturados e Exception Handling

## Objetivo da Sprint
O processamento de IA é passível de muitas falhas invisíveis (timeout na geração de tokens, erro no CUDA para VRAM, falhas na validação do json de uma Tool). Precisamos de logs estruturados utilizando o Serilog e de uma rede de segurança (Global Exception Handler) para impedir que a API "quebre" sem enviar o motivo claro ao frontend.

## Tarefas de Desenvolvimento

### 1. Setup do Serilog
- Instalar os pacotes: `Serilog.AspNetCore`, `Serilog.Sinks.Console`, `Serilog.Sinks.File`.
- Substituir o Logging padrão do ASP.NET Core no `Program.cs` pelo Serilog (`builder.Host.UseSerilog(...)`).
- Configurar logs rotativos: Criar regras de salvamento em arquivo (`logs/focus-api-.txt` com rotação diária) para persistir o histórico de debug, especialmente para falhas que vierem do ONNX Runtime interno.

### 2. Formatação Estruturada (Enrichers)
- Adicionar os enrichers para capturar automaticamente informações de ambiente: `Enrich.FromLogContext()`, `Enrich.WithThreadId()`, `Enrich.WithMachineName()`.
- Padronizar a formatação de saída para JSON (Serilog.Formatting.Compact) no caso de logs em arquivo, pois facilita a futura leitura por ferramentas como ELK Stack caso necessário, e texto limpo no console de Dev.

### 3. Middleware de Exception Handling Global (Minimal API)
- Para o .NET 8, em vez do antigo pipeline de exceptions, usar a nova interface `IExceptionHandler`.
- Criar a classe `GlobalExceptionHandler : IExceptionHandler`.
- Regras de interceptação:
  - `ValidationException` (do FluentValidation) -> Retorna HTTP 400 (Bad Request) com lista de erros.
  - `UnauthorizedAccessException` -> Retorna HTTP 401.
  - Exceções Desconhecidas (ex: `NullReference`) -> Loga o StackTrace severo no Serilog e retorna HTTP 500 informando um "Internal Server Error" amigável sem expor dados sensíveis em produção.

### 4. Validação Contínua nos Endpoints
- Registrar a classe `GlobalExceptionHandler` e ativar `app.UseExceptionHandler()` no pipeline do app.

## Testes e Validações
- **Forçando a Quebra:** Criar uma rota oculta no swagger (ex: `/api/test-crash`) que levanta um erro grosseiro. Bater na rota e confirmar se: 
  A) O aplicativo não morreu. 
  B) O HTTP Status devolvido foi 500 com formato JSON padronizado.
  C) O erro completo com Stack Trace foi gravado em `logs/focus-api-...txt`.

## Critérios de Aceite (DoD)
1. Logs nativos do Console são controlados exclusivamente pelo Serilog.
2. Exceções causadas pelo domínio e validação retornam formatação JSON amigável e código HTTP exato (400, 401, 404).
3. Stacktraces só são visíveis no terminal interno, a interface externa recebe mensagens seguras.
