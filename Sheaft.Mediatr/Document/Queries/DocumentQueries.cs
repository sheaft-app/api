using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Queries;
using Sheaft.Application.Models;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Document.Queries
{
    public class DocumentQueries : IDocumentQueries
    {
        private readonly IAppDbContext _context;
        private readonly IBlobService _blobService;
        private readonly IConfigurationProvider _configurationProvider;

        public DocumentQueries(
            IAppDbContext context, 
            IBlobService blobService,
            IConfigurationProvider configurationProvider)
        {
            _context = context;
            _blobService = blobService;
            _configurationProvider = configurationProvider;
        }

        public IQueryable<DocumentDto> GetDocument(Guid id, RequestUser currentUser)
        {
            return _context.Legals
                    .Where(l => l.Documents.Any(d => d.Id == id))
                    .SelectMany(l => l.Documents)
                    .ProjectTo<DocumentDto>(_configurationProvider);
        }

        public IQueryable<DocumentDto> GetDocuments(RequestUser currentUser)
        {
            return _context.Legals
                    .SelectMany(l => l.Documents)
                    .ProjectTo<DocumentDto>(_configurationProvider);
        }

        public async Task<byte[]> DownloadDocumentAsync(Guid documentId, RequestUser currentUser, CancellationToken token)
        {
            var legal = await _context.Legals
                    .SingleOrDefaultAsync(l => l.Documents.Any(d => d.Id == documentId), token);

            var document = legal.Documents.FirstOrDefault(d => d.Id == documentId);
            if (document == null)
                return null;

            byte[] archiveFile;
            using (var archiveStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create, true))
                {
                    foreach (var page in document.Pages)
                    {
                        var result = await _blobService.DownloadDocumentPageAsync(document.Id, page.Id, legal.User.Id, token);
                        if (!result.Succeeded)
                            throw SheaftException.Unexpected(result.Exception, result.Message, result.Params);

                        var zipArchiveEntry = archive.CreateEntry(page.Filename + page.Extension, CompressionLevel.Optimal);
                        using (var zipStream = zipArchiveEntry.Open())
                            await zipStream.WriteAsync(result.Data, 0, result.Data.Length, token);
                    }
                }

                archiveFile = archiveStream.ToArray();
            }

            return archiveFile;
        }

        public async Task<byte[]> DownloadDocumentPageAsync(Guid documentId, Guid pageId, RequestUser currentUser, CancellationToken token)
        {
            var legal = await _context.Legals
                    .SingleOrDefaultAsync(l => l.Documents.Any(d => d.Id == documentId), token);

            var document = legal.Documents.FirstOrDefault(d => d.Id == documentId);
            if (document == null)
                return null;

            var result = await _blobService.DownloadDocumentPageAsync(document.Id, pageId, legal.User.Id, token);
            if (!result.Succeeded)
                return null;

            return result.Data;
        }
    }
}