# Sprint 27: Decodificação Especulativa (Speculative Decoding)

## Objetivo da Sprint
Acelerar drasticamente a velocidade na qual o modelo de 27B cospe o código no terminal. Utilizaremos o modelo pequeno (1.5B), que já está na memória (Sprint 26), para cuspir 5 tokens rapidamente. O modelo grande (27B) roda em paralelo validando todos os 5 tokens de uma vez num único passe pela rede neural. Se ele aprovar, ganhamos velocidade 5x. Se reprovar, corrigimos e seguimos.

## Tarefas de Desenvolvimento

### 1. Loop Duplo em C# (Drafting & Verification)
- Modificar o engine de inferência construído na Sprint 25.
- Fase de **Drafting (Rascunho):** O C# dispara a chamada pro ONNX do Qwen3 1.5B para ele prever K passos no futuro (ex: gerar 5 possíveis próximos tokens). O 1.5B é minúsculo e faz isso em milissegundos.
- Fase de **Verification:** Pegar os 5 tensores gerados pelo pequeno e passá-los para a entrada (input_ids) do modelo 27B numa **única invocação** (passagem em lote / Batch pass).
- Analisar as probabilidades do Logit do modelo grande. Se o Logit dele concordar com as previsões do pequeno (probabilidade do token > threshold), aceitamos e enviamos os 5 tokens pro SignalR de uma tacada só. Se houver divergência no token 3, descartamos o 4 e 5 e recalculamos a partir do 3.

### 2. Otimização de Troca de Contexto
- Cuidado extremo para que a troca de GPU para RAM entre os dois modelos não gere gargalos no *Bus* PCIe da placa mãe, matando o ganho de velocidade.
- O ONNX permite amarrar *I/O Bindings* diretamente na GPU.

## Testes e Validações
- Benchmark Local: Escrever um teste que mede o TTFT (Time to First Token) e o TPS (Tokens Per Second). Rodar 100 palavras sem Speculative e 100 palavras COM Speculative. O ganho matemático deve ser superior a 1.5x a 2x de performance de velocidade na tela.

## Critérios de Aceite (DoD)
1. O Modelo 27B gera código muito mais rápido utilizando o modelo de 1.5B como muleta (Draft Model).
2. Sem corrupção sintática nas palavras geradas.
