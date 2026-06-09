# Sprint 51: Setup do Language Server Protocol (LSP) no .NET 8

## Objetivo da Sprint
A extensão do VS Code não usará apenas HTTP/REST para se comunicar com o modelo. Para injetar "Ghost Text" (código translúcido) e mostrar sublinhados vermelhos interativos (Quick Fix), utilizaremos o padrão da indústria: LSP. O backend rodará um Servidor LSP interno em C#.

## Tarefas de Desenvolvimento

### 1. Adição da Biblioteca Core LSP
- Instalar o pacote NuGet `OmniSharp.Extensions.LanguageServer` no projeto `Application` ou em um novo projeto de borda `Focus.AI.LspHost`.

### 2. Criação do Entrypoint LSP
- Criar um Worker Service ou integrar no próprio pipeline do ASP.NET Core `Program.cs`.
- Inicializar o servidor LSP associando-o a um *Named Pipe* ou *Standard Input/Output (Stdio)* (que é o padrão usado pelo VS Code para rodar servidores locais ocultos).
  ```csharp
  var server = await LanguageServer.From(options =>
      options
          .WithInput(Console.OpenStandardInput())
          .WithOutput(Console.OpenStandardOutput())
          .WithLoggerFactory(loggerFactory)
          ...);
  ```

### 3. Associação com o Container de DI
- Garantir que a instância do `LanguageServer` consiga resolver serviços do repositório padrão do C# (`IChatClient`, `IUserRepository`, etc.) conectando os contêineres de Injeção de Dependência do ASP.NET com o do OmniSharp.

## Testes e Validações
- **Teste de Handshake:** Rodar o backend. Em vez de abrir o Swagger, criar um pequeno script em Node.js usando o pacote `vscode-jsonrpc` que manda a mensagem de inicialização padrão do LSP e valida se o backend responde com as *ServerCapabilities* sem crashear.

## Critérios de Aceite (DoD)
1. O backend .NET atua como um Language Server compatível com a especificação oficial da Microsoft.
2. Interoperabilidade total entre o pipeline LSP e as classes de banco de dados e IA desenvolvidas na Fase 1 a 4.
