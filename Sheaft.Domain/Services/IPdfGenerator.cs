namespace Sheaft.Domain;

public interface IPdfGenerator
{
    Task<Result<byte[]>> GeneratePdf<T>(string filename, string templateId, T data, CancellationToken token);
}