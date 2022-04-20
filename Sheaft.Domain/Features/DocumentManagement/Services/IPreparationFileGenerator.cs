namespace Sheaft.Domain.DocumentManagement;

public interface IPreparationFileGenerator
{
    Task<Result<string>> Generate(PreparationDocumentData data, CancellationToken token);
}