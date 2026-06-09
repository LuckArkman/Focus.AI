# Sprint 39: Comunicação Inter-Agentes (Mensageria Interna)

## Objetivo da Sprint
Quando um Agente cria um Sub-Agente, eles precisam conversar. O Agente Filho precisa dizer *"Terminei o HTML"*, e o Agente Pai precisa responder *"Ótimo, agora centralize a div"*. O backend do Focus.AI requer uma caixa de mensagens interna assíncrona.

## Tarefas de Desenvolvimento

### 1. Criação da Tool `SendMessage`
- Construir `SendMessageToolHandler`.
- Parâmetros: `RecipientId` (Guid do Subagente de destino), `Message` (string).
- Esta ferramenta permite que o output de um modelo ONNX vá parar diretamente como *User Input* no fluxo de outro modelo ONNX rodando em outro local da RAM.

### 2. O Barramento Assíncrono (Internal Inbox)
- Usar recursos C# como `Channel<T>` do System.Threading.Channels, que é perfeito para produtor/consumidor leve sem precisar instalar RabbitMQ.
- Quando o Agente 1 mandar uma mensagem para o Agente 2:
  1. A mensagem é colocada no `Channel` do Agente 2.
  2. Se o Agente 2 estiver bloqueado executando código, a mensagem fica na fila.
  3. Quando ele termina o código, ele "desperta" de forma reativa, lê a nova mensagem, e o loop ReAct recomeça gerando a resposta.

### 3. Integração com o MongoDB
- Toda troca de farpas técnica e de código entre agentes não pode se perder; ela é inserida no `ChatSessionRepository` usando o campo `Role` como "Agent_Frontend" e "Agent_Architect", formando a trilha de auditoria para o Desenvolvedor humano poder ler onde as máquinas erraram em caso de conflito.

## Testes e Validações
- **Simulação de Ping Pong:** Criar dois agentes mockados (substituindo o ONNX por retornos pré-gravados) e usar os *Channels* do C# para que eles conversem entre si trocando 5 mensagens. Validar que nenhuma *Thread deadlock* ocorreu e que todos os dados foram persistidos no MongoDB corretamente na mesma *SessionId*.

## Critérios de Aceite (DoD)
1. Modelos de linguagem conseguem interagir uns com os outros diretamente via memória RAM no backend utilizando canais thread-safe de altíssima performance (Channels C#).
2. Transparência completa de todas as conversas B2B (Bot-To-Bot) nos logs transacionais e na interface web/Extensão VS Code.
