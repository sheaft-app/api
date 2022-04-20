namespace Sheaft.Domain.DocumentManagement;

public interface IPreparationFileGenerator
{
    Task<Result<byte[]>> Generate(PreparationDocumentData data, CancellationToken token);
}