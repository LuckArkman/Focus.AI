# Sprint 72: Otimização de Latência do Modelo ONNX e Gestão Avançada de VRAM

## Objetivo da Sprint
Focar exclusivamente no desempenho do tempo de inferência da Inteligência Artificial. Garantir que os modelos Qwen3 (tanto o de 1.5B para Ghost Text quanto o de 27B para chat avançado) respondam com latência mínima, explorando quantização e otimização de cache.

## Tarefas de Desenvolvimento

### 1. Gestão de KV Cache (Key-Value Cache)
- Revisar a implementação da sessão de inferência do `Microsoft.ML.OnnxRuntime.Gpu` para garantir que o estado interno do modelo (KV Cache) seja gerenciado e descartado de forma eficiente entre as mensagens de chat e requisições concorrentes de autocomplete.
- Implementar mecanismos para pré-alocar buffers de cache ou limitar seu tamanho dinamicamente com base no contexto disponível.

### 2. Experimentação com Quantização Avançada
- Avaliar diferentes níveis de quantização (como INT8, INT4, AWQ ou GPTQ se suportados via ONNX) fornecidos pela biblioteca `onnxruntime-genai`.
- Testar comparativamente o balanço entre a degradação da qualidade da resposta do modelo e o ganho expressivo de velocidade/redução de VRAM consumida.
- Configurar a interface C# para carregar dinamicamente a versão otimizada baseada no hardware detectado do usuário.

### 3. Melhoria na Estratégia de Decodificação (Sampling)
- Revisar a lógica do loop de geração de tokens, ajustando e expondo hiperparâmetros como Temperature, Top-P, Top-K e Repetition Penalty.
- Otimizar o processo de Speculative Decoding (implementado na Sprint 27) para garantir que a taxa de aceitação (acceptance rate) dos tokens previstos pelo modelo menor compense o overhead computacional adicional.

## Testes e Validações
- **Benchmark de Latência (Time-To-First-Token - TTFT):** Medir o tempo exato desde a pressão da tecla no VS Code até a recepção do primeiro token translúcido. A meta é manter o TTFT para Ghost Text consistentemente abaixo de 250ms.
- **Taxa de Transferência (Tokens Per Second - TPS):** Medir a velocidade de geração do chat do modelo de 27B. Garantir leitura humana confortável em placas de vídeo de nível consumidor (ex: série RTX 3000/4000).

## Critérios de Aceite (DoD)
1. Tempo de primeira resposta e taxa de tokens por segundo otimizados e documentados.
2. VRAM consumida otimizada por meio do carregamento inteligente e quantização ajustada.
