# Sprint 24: Offloading e Alocação Dinâmica de Memória (VRAM vs RAM)

## Objetivo da Sprint
O Qwen3 de 27B quantizado ocupará cerca de 16-20GB. Muitos desenvolvedores podem ter GPUs com apenas 8GB ou 12GB (como uma RTX 3060). É vital que o .NET backend suporte offloading dinâmico: rodar o máximo de blocos possíveis na GPU, mas repassar o excesso para a RAM normal (CPU), permitindo que qualquer PC rode o assistente, mesmo que com menor velocidade.

## Tarefas de Desenvolvimento

### 1. Detector de Hardware C#
- Utilizar consultas WMI (Windows Management Instrumentation) ou bibliotecas como `System.Management` ou wrappers NVML para tentar descobrir quanta VRAM dedicada existe na máquina local.

### 2. Configuração de Arena Memory no ONNX
- Usar a API do ONNX Runtime em C# para gerenciar a "Arena de Memória".
- Dividir a alocação de tensores usando configurações avançadas no `SessionOptions`, definindo o `gpu_mem_limit`.
- O ONNX Runtime fará automaticamente o offloading dos nós do grafo da rede neural que não couberem na GPU diretamente para o Execution Provider do Host (CPU).

### 3. Paralelismo de CPU
- Para a parte do modelo que "vazar" para a RAM, instruir o `SessionOptions.IntraOpNumThreads` para usar o número máximo de threads lógicas disponíveis da CPU do usuário (`Environment.ProcessorCount - 1`), permitindo inferência multithread no processador.

## Testes e Validações
- **Teste de Hardware Baixo:** Simular nas configurações um limite de 4GB de VRAM. Carregar o modelo de 27B. Validar que o ONNX não "crasha" com `CUDA_OUT_OF_MEMORY`, mas continua processando a resposta (mesmo que demore mais segundos do que o normal).

## Critérios de Aceite (DoD)
1. O assistente roda o 27B independentemente do tamanho da VRAM do usuário através da técnica de Offloading.
2. A CPU trabalha intensamente distribuída em multicore caso o modelo caia pra RAM.
