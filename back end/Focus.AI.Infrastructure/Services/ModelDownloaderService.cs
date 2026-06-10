using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Focus.AI.Infrastructure.Services;

public class ModelDownloaderService : IHostedService
{
    private readonly ILogger<ModelDownloaderService> _logger;
    private readonly string _modelDir;

    private const string ModelUrl = "https://huggingface.co/Xenova/all-MiniLM-L6-v2/resolve/main/onnx/model_quantized.onnx";
    private const string VocabUrl = "https://huggingface.co/Xenova/all-MiniLM-L6-v2/resolve/main/vocab.txt";

    public ModelDownloaderService(ILogger<ModelDownloaderService> logger)
    {
        _logger = logger;
        _modelDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Models");
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (!Directory.Exists(_modelDir))
        {
            Directory.CreateDirectory(_modelDir);
        }

        await DownloadFileIfNotExistsAsync(ModelUrl, "model.onnx", cancellationToken);
        await DownloadFileIfNotExistsAsync(VocabUrl, "vocab.txt", cancellationToken);
    }

    private async Task DownloadFileIfNotExistsAsync(string url, string fileName, CancellationToken cancellationToken)
    {
        var filePath = Path.Combine(_modelDir, fileName);
        if (!File.Exists(filePath))
        {
            _logger.LogInformation("Downloading {FileName} from {Url}...", fileName, url);
            using var client = new HttpClient();
            var response = await client.GetAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();

            await using var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
            await response.Content.CopyToAsync(fs, cancellationToken);
            _logger.LogInformation("{FileName} downloaded successfully.", fileName);
        }
        else
        {
            _logger.LogInformation("{FileName} already exists at {FilePath}.", fileName, filePath);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
