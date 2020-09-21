using Sheaft.Core;
using Sheaft.Application.Models;
using System;
using System.Linq;

namespace Sheaft.Application.Queries
{
    public interface IDocumentQueries
    {
        IQueryable<DocumentDto> GetDocument(Guid id, RequestUser currentUser);
        IQueryable<DocumentDto> GetDocuments(RequestUser currentUser);
    }
}