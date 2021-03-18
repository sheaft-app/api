using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Application.Interfaces.Queries
{
    public interface IDocumentQueries
    {
        IQueryable<DocumentDto> GetDocument(Guid id, RequestUser currentUser);
        IQueryable<DocumentDto> GetDocuments(RequestUser currentUser);
        Task<byte[]> DownloadDocumentPageAsync(Guid documentId, Guid pageId, RequestUser currentUser, CancellationToken token);
        Task<byte[]> DownloadDocumentAsync(Guid documentId, RequestUser currentUser, CancellationToken token);
    }
}