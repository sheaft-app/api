using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Domain;

namespace Sheaft.Application.Common.Interfaces.Queries
{
    public interface IDocumentQueries
    {
        IQueryable<DocumentDto> GetDocument(Guid id, RequestUser currentUser);
        IQueryable<DocumentDto> GetDocuments(RequestUser currentUser);
        Task<byte[]> DownloadDocumentPageAsync(Guid documentId, Guid pageId, RequestUser currentUser, CancellationToken token);
        Task<byte[]> DownloadDocumentAsync(Guid documentId, RequestUser currentUser, CancellationToken token);
    }
}