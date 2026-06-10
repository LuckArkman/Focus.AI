using Focus.AI.Application.Interfaces.Services;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using Microsoft.ML.Tokenizers;

namespace Focus.AI.Infrastructure.Services;

public class LocalEmbeddingGenerator : IEmbeddingGenerator, IDisposable
{
    private readonly InferenceSession _session;
    private readonly BertTokenizer _tokenizer;

    public LocalEmbeddingGenerator()
    {
        var modelDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Models");
        var modelPath = Path.Combine(modelDir, "model.onnx");
        var vocabPath = Path.Combine(modelDir, "vocab.txt");

        _session = new InferenceSession(modelPath);
        _tokenizer = BertTokenizer.Create(vocabPath);
    }

    public Task<float[]> GenerateEmbeddingAsync(string text, CancellationToken cancellationToken = default)
    {
        // 1. Tokenize
        var inputIdsList = _tokenizer.EncodeToIds(text);
        
        // 2. Prepare inputs
        // MiniLM expects input_ids, attention_mask, token_type_ids
        var inputIds = inputIdsList.Select(i => (long)i).ToArray();
        var attentionMask = Enumerable.Repeat(1L, inputIds.Length).ToArray();
        var tokenTypeIds = Enumerable.Repeat(0L, inputIds.Length).ToArray();

        var inputIdsTensor = new DenseTensor<long>(inputIds.AsMemory(), new int[] { 1, inputIds.Length });
        var attentionMaskTensor = new DenseTensor<long>(attentionMask.AsMemory(), new int[] { 1, inputIds.Length });
        var tokenTypeIdsTensor = new DenseTensor<long>(tokenTypeIds.AsMemory(), new int[] { 1, inputIds.Length });

        var inputs = new List<NamedOnnxValue>
        {
            NamedOnnxValue.CreateFromTensor("input_ids", inputIdsTensor),
            NamedOnnxValue.CreateFromTensor("attention_mask", attentionMaskTensor),
            NamedOnnxValue.CreateFromTensor("token_type_ids", tokenTypeIdsTensor)
        };

        // 3. Inference
        using var results = _session.Run(inputs);
        
        // 4. Mean Pooling
        var outputTensor = results.First(v => v.Name == "last_hidden_state").AsTensor<float>();
        var embedding = new float[384];

        for (int i = 0; i < inputIds.Length; i++)
        {
            for (int j = 0; j < 384; j++)
            {
                embedding[j] += outputTensor[0, i, j];
            }
        }

        for (int j = 0; j < 384; j++)
        {
            embedding[j] /= inputIds.Length;
        }

        // 5. L2 Normalization
        float sumOfSquares = 0;
        for (int j = 0; j < 384; j++)
        {
            sumOfSquares += embedding[j] * embedding[j];
        }

        float magnitude = (float)Math.Sqrt(sumOfSquares);
        for (int j = 0; j < 384; j++)
        {
            embedding[j] /= magnitude;
        }

        return Task.FromResult(embedding);
    }

    public void Dispose()
    {
        _session?.Dispose();
    }
}
