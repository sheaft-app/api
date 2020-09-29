using Sheaft.Core;
using Sheaft.Application.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace Sheaft.Application.Queries
{
    public interface IDocumentQueries
    {
        IQueryable<DocumentDto> GetDocument(Guid id, RequestUser currentUser);
        IQueryable<DocumentDto> GetDocuments(RequestUser currentUser);
        Task<byte[]> DownloadDocumentPageAsync(Guid documentId, Guid pageId, RequestUser currentUser, CancellationToken token);
        Task<byte[]> DownloadDocumentAsync(Guid documentId, RequestUser currentUser, CancellationToken token);
    }
}