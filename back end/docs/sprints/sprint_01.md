# Sprint 01: Configuração Inicial do Repositório e Setup do Backend .NET 8

## Objetivo da Sprint
Estabelecer a base estrutural do projeto backend utilizando o .NET 8, configurando a solution, o projeto Web API principal e a organização inicial de pastas orientada à Clean Architecture. O sucesso desta sprint garante que os desenvolvedores tenham um ambiente unificado e padronizado para compilação.

## Tarefas de Desenvolvimento

### 1. Inicialização do Projeto e da Solution
- Executar os comandos via CLI do .NET para criar a estrutura base (Solution e Projeto Web API):
  ```bash
  dotnet new sln -n Focus.AI.Backend
  dotnet new webapi -n Focus.AI.Api -f net8.0
  dotnet sln add Focus.AI.Api/Focus.AI.Api.csproj
  ```
- Configurar o projeto `Focus.AI.Api` para remover os templates genéricos do *WeatherForecast*, limpando arquivos inúteis.

### 2. Estruturação da Clean Architecture
- Criar os subprojetos para separação de responsabilidades e adicioná-los à Solution:
  - `Focus.AI.Domain` (Entidades, Interfaces de Repositório).
  - `Focus.AI.Application` (Casos de Uso, CQRS, Interfaces de Serviços).
  - `Focus.AI.Infrastructure` (Implementações de EF Core, MongoDB Driver, Integração Qdrant).
- Configurar as referências cruzadas (`ProjectReference`) mantendo o sentido único de dependência de fora para dentro (Api -> Infrastructure -> Application -> Domain).

### 3. Configurações Globais (.editorconfig e Git)
- Configurar o `.editorconfig` na raiz do repositório para forçar padrões rigorosos de código C# (formatação de chaves, espaçamentos, nomenclatura de interfaces com o prefixo 'I').
- Criar e customizar o `.gitignore` específico para C# e Visual Studio, além de exclusões para as futuras pastas de modelos ONNX (ex: `*.onnx`, `*.bin`) para evitar subidas acidentais de arquivos de 20GB no Git.

### 4. Suporte a Minimal APIs e Swagger
- Configurar a inicialização no `Program.cs` utilizando o padrão Minimal APIs do ASP.NET Core.
- Garantir que a documentação da API Swagger / OpenAPI seja gerada corretamente em ambiente de desenvolvimento.

## Testes e Validações
- **Validação:** Rodar `dotnet build` e garantir compilação com Zero Warnings.
- **Validação:** Rodar `dotnet run` e verificar o acesso à rota do Swagger UI no navegador.
- **Validação Estrutural:** Assegurar que o projeto `Domain` não possua referências para nenhum pacote NuGet externo indesejado.

## Critérios de Aceite (DoD)
1. A solution compila perfeitamente em modo Release.
2. A estrutura de Clean Architecture (Api, Application, Domain, Infrastructure) existe e tem suas dependências mapeadas corretamente.
3. Repositório versionado com Git na ramificação `main` contendo o commit estrutural.
