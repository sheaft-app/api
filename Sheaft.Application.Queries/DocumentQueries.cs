using System;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Sheaft.Application.Interop;
using Sheaft.Application.Models;
using Sheaft.Core;

namespace Sheaft.Application.Queries
{
    public class DocumentQueries : IDocumentQueries
    {
        private readonly IAppDbContext _context;
        private readonly IConfigurationProvider _configurationProvider;

        public DocumentQueries(IAppDbContext context, IConfigurationProvider configurationProvider)
        {
            _context = context;
            _configurationProvider = configurationProvider;
        }

        public IQueryable<DocumentDto> GetDocument(Guid id, RequestUser currentUser)
        {
            return _context.Documents
                    .Get(d => d.Id == id, true)
                    .ProjectTo<DocumentDto>(_configurationProvider);
        }

        public IQueryable<DocumentDto> GetDocuments(RequestUser currentUser)
        {
            return _context.Documents
                    .Get(null, true)
                    .ProjectTo<DocumentDto>(_configurationProvider);
        }
    }
}