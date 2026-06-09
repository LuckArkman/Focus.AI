# Sprint 21: Setup Base do Microsoft.Extensions.AI (Abstração do Chat)

## Objetivo da Sprint
Em vez de acoplar o sistema diretamente à biblioteca do ONNX, usaremos o padrão mais moderno da Microsoft: o `Microsoft.Extensions.AI`. Isso permite tratar qualquer IA (local ou remota) sob a mesma interface `IChatClient`, facilitando testes unitários e possibilitando o "Fallback Híbrido" (Módulo 3) no futuro, caso o usuário não tenha GPU e precise bater numa API externa de emergência.

## Tarefas de Desenvolvimento

### 1. Instalação do Pacote Abstrato
- Instalar `Microsoft.Extensions.AI` e `Microsoft.Extensions.AI.Abstractions` no projeto `Application`.

### 2. Implementação do Cliente Fake (Para Testes Iniciais)
- Antes do ONNX, precisamos criar uma ponte para testar o SignalR.
- Criar a classe `FakeChatClient : IChatClient` no projeto de testes/infraestrutura que apenas ecoa (Echo) a mensagem do usuário com um pequeno atraso (simulando stream de IA).

### 3. Registro do IChatClient
- Configurar o Injetor de Dependência (`IServiceCollection`) para fornecer esse cliente quando solicitado pelos MediatR Handlers.
- Implementar o padrão *Factory* para permitir devolver múltiplos clientes de chat (ex: um factory para o 1.5B e outro para o 27B) caso seja solicitado futuramente no roteador.

## Testes e Validações
- **Teste de Pipeline:** Chamar a interface `IChatClient.CompleteAsync` através do controlador e garantir que o retorno chega ao front-end corretamente formatado.

## Critérios de Aceite (DoD)
1. A interface `IChatClient` está perfeitamente integrada no projeto.
2. Nenhum código de aplicação (Handlers) faz referência a bibliotecas externas proprietárias como OpenAI ou ONNX (Inversão de Dependência limpa).
