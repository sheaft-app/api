using Microsoft.EntityFrameworkCore;
using Sheaft.Domain;
using Sheaft.Domain.DocumentManagement;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.Infrastructure.DocumentManagement;

internal class DocumentRepository : Repository<Document, DocumentId>, IDocumentRepository
{
    public DocumentRepository(IDbContext context)
        : base(context)
    {
    }

    public override Task<Result<Document>> Get(DocumentId identifier, CancellationToken token)
    {
        return QueryAsync(async () =>
        {
            var result = await Values
                .SingleOrDefaultAsync(e => e.Identifier == identifier, token);

            return result != null
                ? Result.Success(result)
                : Result.Failure<Document>(ErrorKind.NotFound, "invoice.not.found");
        });
    }
}