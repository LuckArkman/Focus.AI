# Sprint 71: Profiling de Memória e Otimização de Garbage Collection (GC)

## Objetivo da Sprint
Realizar uma análise profunda do uso de memória (profiling) no backend em C# (.NET 8) para identificar vazamentos, reduzir alocações desnecessárias e otimizar o Garbage Collector, garantindo estabilidade em sessões longas de uso intensivo da IA.

## Tarefas de Desenvolvimento

### 1. Profiling de Memória no Backend
- Utilizar ferramentas de diagnóstico do .NET (como `dotnet-dump`, `dotnet-trace` e dotMemory/Visual Studio Profiler) para capturar o estado da memória durante a execução intensa do Focus.AI.
- Identificar objetos que sobrevivem indevidamente nas gerações Gen1 e Gen2, com foco especial nos arrays e buffers utilizados durante o streaming de tokens do modelo ONNX e nas requisições do SignalR.

### 2. Redução de Alocações (Zero Allocation Patterns)
- Refatorar caminhos críticos (hot paths) de serialização/deserialização JSON e processamento de strings (usado pesadamente no RAG e no LSP).
- Substituir alocações de `string` e arrays por structs, `Span<T>`, `Memory<T>` e `ArrayPool<T>` onde apropriado.
- Revisar a alocação de objetos no ciclo de vida de requisições de completação em linha (Ghost Text) para minimizar a pressão sobre o GC.

### 3. Ajuste Fino do Garbage Collector (GC)
- Ajustar as configurações do GC no `Focus.AI.Api.csproj` ou `runtimeconfig.json` para o modo ideal de operação (ex: Server GC vs Workstation GC) dependendo se a ferramenta roda primariamente em ambiente de desenvolvimento local (como um processo em background).
- Configurar limites de memória (Heap Limit) de forma conservadora para coexistir pacificamente com a IDE (VS Code) e outras aplicações do usuário.

## Testes e Validações
- **Teste de Vazamento de Memória (Memory Leak Test):** Executar um script automatizado que envia milhares de requisições contínuas de autocomplete e mensagens de chat. Monitorar o uso de memória RAM. A memória deve estabilizar após o aquecimento e não crescer indefinidamente.

## Critérios de Aceite (DoD)
1. Perfil de memória documentado e principais gargalos de alocação eliminados (uso intensivo de `Span<T>` implementado).
2. O processo do backend mantém um consumo de RAM estável e previsível durante uso prolongado, sem picos de CPU causados por pausas longas do GC.
