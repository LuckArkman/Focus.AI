# Sprint 23: Integração do ONNX Runtime GPU (Inference Session)

## Objetivo da Sprint
Nesta sprint será inserido o motor C++ de inferência ONNX dentro do C#. Este é o coração do backend que realmente "pensa" e gera as respostas.

## Tarefas de Desenvolvimento

### 1. Setup das Bibliotecas Nativas
- Adicionar no projeto `Infrastructure` os pacotes: `Microsoft.ML.OnnxRuntime.Gpu` ou `Microsoft.ML.OnnxRuntime.DirectML` (se focar em placas AMD/Intel além da Nvidia).
- Para Nvidia, é vital garantir que as DLLs corretas do CUDA e cuDNN sejam mapeadas nas variáveis de ambiente.

### 2. Criação da OnnxInferenceSession
- Criar a classe `OnnxChatClient` implementando a nossa `IChatClient` (substituindo a *Fake* da Sprint 21).
- Instanciar a `InferenceSession` apontando para o caminho do arquivo do modelo (identificado na Sprint 22).

### 3. Configuração de SessionOptions (CUDA Provider)
- No C#, configurar a `SessionOptions` para pendurar a execução primariamente na GPU (CUDA Execution Provider).
- Exemplo de código necessário:
  ```csharp
  var options = new SessionOptions();
  options.AppendExecutionProvider_CUDA(new CUDAProviderOptions { DeviceId = 0 });
  ```

### 4. Gerenciamento de Ciclo de Vida (IDisposable)
- Sessões ONNX armazenam ponteiros não-gerenciados C++ (Unmanaged Memory). É **MANDATÓRIO** que `OnnxChatClient` implemente `IDisposable` para liberar os tensores da memória VRAM e RAM após o aplicativo ser desligado, senão o PC do usuário vai travar completamente com *Memory Leaks*.

## Testes e Validações
- Passar um prompt estático ("Olá, diga 'teste' e pare.") convertê-lo em Tensor, enviar para o `.Run()` do ONNX, pegar os logits, rodar o `ArgMax` (ou multinomial sampling) em C#, pegar o ID do token e destokenizar para string. Validar se a string "teste" é impressa.

## Critérios de Aceite (DoD)
1. O backend consegue interagir diretamente com a GPU local do computador hospedeiro via CUDA ou DirectML.
2. Não há vazamento de memória; a VRAM sobe quando o `Session` inicia e é liberada estritamente quando a classe sofre o `Dispose`.
