# Sprint 30: Testes de Carga, Monitoramento RAM/VRAM e Benchmarks

## Objetivo da Sprint
O final da Fase 3 marca o momento em que a API C# consegue raciocinar localmente offline. Como IAs pesam muito na máquina, a aplicação deve ser gentil e previsível. Usaremos ferramentas de diagnóstico do .NET 8 para evitar que o PC do desenvolvedor congele completamente de repente.

## Tarefas de Desenvolvimento

### 1. Monitoramento de VRAM em tempo real
- Implementar um Hosted Service que roda a cada 10 segundos.
- O serviço fará uma interrogação na NVIDIA GPU (se existente) utilizando *ManagementObjectSearcher* WMI C# ou wrapper cuDNN.
- Publicará os dados no Serilog: `[HardwareStatus] VRAM Utilizada: 14.5 GB / RAM CPU: 4 GB`.

### 2. Auto-kill de Processos ou Cleanup Aggressivo
- A geração de LLMs, em C#, pode gerar alta pressão no *Garbage Collector (GC)* com os Tensors C++ que não morrem no ciclo convencional de vida C#.
- Implementar chamada tática de `GC.Collect()` na camada do SignalR caso termine a geração pesada de um arquivo enorme. Não é boa prática para API web em geral, mas **é obrigatório** para C#/C++ Interop com ponteiros de inferência ONNX gigabites, se o `Dispose()` não estiver dando conta tão rápido.

### 3. Teste de Stress
- Desenvolver um script via C# ou *Bombardier* local que mande 5 Requisições Simultâneas pelo SignalR tentando rodar o Modelo 27B, imitando 5 abas abertas pelo desenvolvedor. O backend **DEVE** enfileirar requests e trancar (Lock) ou rejeitar requests extras com erro de "*GPU Busy*", não permitindo crash por sobrecarga.

## Testes e Validações
- Testar enviar 2 requisições síncronas. Ver a trava `SemaphoreSlim` segurando a request N.º 2 até a primeira acabar.
- Observar a leitura da WMI pelo Console confirmando se a VRAM da placa de vídeo sofre a liberação correta de memória logo após o token `<|endoftext|>` finalizar.

## Critérios de Aceite (DoD)
1. Fim da Fase 3: Modelo Qwen3 1.5B e 27B rodando local, performático (Speculative Decoding) e gerando Streaming no SignalR.
2. Sem Memory Leaks ou vazamentos de VRAM provados por testes de carga de 2 horas.
