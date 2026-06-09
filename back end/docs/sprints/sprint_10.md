# Sprint 10: Estruturação dos Testes de Integração com Testcontainers

## Objetivo da Sprint
O último passo da Fase 1 da fundação do backend é blindar tudo o que fizemos contra "efeitos colaterais" em atualizações futuras. Como temos um ambiente que cruza 3 bancos de dados diferentes (Postgres, Mongo, Qdrant), mockar tudo não é seguro. Vamos configurar a biblioteca `Testcontainers` para levantar Docker descartável e injetá-lo nos testes.

## Tarefas de Desenvolvimento

### 1. Configuração do Projeto de Integration Tests
- Criar o projeto `Focus.AI.IntegrationTests` baseado no template do xUnit.
- Instalar bibliotecas pesadas de teste de comportamento: `Testcontainers`, `Testcontainers.PostgreSql`, `Testcontainers.MongoDb`, `FluentAssertions`.

### 2. A Fábrica de Aplicação em Memória
- Usar a classe `WebApplicationFactory<Program>` (já comum no minimal API do ASP.NET) para inicializar a aplicação em memória.
- Desenvolver a sobrecarga do construtor virtual da Factory. Dentro dela, iremos dizer ao `.NET`: *"Apague as connection strings originais do appsettings e substitua por estas URLs dinâmicas que os Testcontainers estão gerando agora"*.

### 3. Setup dos Containers Dinâmicos
- Codificar as *Fixtures* do xUnit para subir programaticamente:
  - 1 Container Postgres (versão idêntica ao docker-compose).
  - 1 Container MongoDB.
  - 1 Container Qdrant.
- O xUnit iniciará os containers paralelamente. Somente após as portas serem expostas (a porta no teste gerada dinamicamente, não as reais como 5432) a rotina de testes inicia.

### 4. Escrevendo o Teste "Golden Path"
- Elaborar um teste completo cobrindo todas as sprints de 1 a 7:
  1. O teste envia um JSON via client HTTP (`HttpClient`) para o endpoint de registro. Valida o Postgres.
  2. O teste envia Login e recupera o Token.
  3. O teste faz uma inserção (via repositório) no MongoDB e no Qdrant usando esse `UserId`.
  4. Tudo precisa funcionar com os containers descartáveis.
- O xUnit derruba o *Testcontainer* assim que a validação finaliza, sem sujar os dados reais do PC de desenvolvimento.

## Testes e Validações
- **Rodar `dotnet test`:** Garantir que o Docker Desktop (ou serviço local) suporte ser invocado programaticamente. O teste demorará um pouco a mais na primeira execução por ter que puxar as imagens do Hub, mas as demais serão muito rápidas.

## Critérios de Aceite (DoD)
1. Há uma bateria de testes cobrindo a integridade dos Bancos de Dados Relacional, Documental e Vetorial rodando isoladamente usando `Testcontainers`.
2. A suíte de testes pode ser rodada limpa (`dotnet test`) em qualquer nova máquina de desenvolvimento que possua o Docker Engine ligado, sem que o dev precise executar configurações manuais de banco antes.
