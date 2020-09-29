using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sheaft.Application.Commands;
using Sheaft.Application.Interop;
using Sheaft.Core.Extensions;
using Sheaft.Core.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sheaft.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("upload")]
    public class UploadController : ControllerBase
    {
        private readonly ISheaftMediatr _mediatr;

        public UploadController(ISheaftMediatr mediatr)
        {
            _mediatr = mediatr;
        }

        [HttpPost("products")]
        [Authorize(Policy = Policies.PRODUCER)]
        public async Task<IActionResult> UploadProductsCatalog(CancellationToken token)
        {
            var files = Request.Form.Files;
            if (!files.Any())
                return BadRequest();

            var ids = new List<Guid>();
            var requestUser = HttpContext.User.ToIdentityUser(HttpContext.TraceIdentifier);

            foreach (var formFile in files)
            {
                if (formFile.Length == 0)
                    continue;

                using (var stream = new MemoryStream())
                {
                    await formFile.CopyToAsync(stream, token);
                    var result = await _mediatr.Process(new QueueImportProductsCommand(requestUser) { Id = requestUser.Id, FileName = formFile.FileName, FileStream = stream }, token);
                    if (!result.Success)
                        return BadRequest(result);

                    ids.Add(result.Data);
                }
            }

            return Ok(ids);
        }

        [HttpPost("kyc")]
        [Authorize(Policy = Policies.STORE_OR_PRODUCER)]
        public async Task<IActionResult> UploadKycDocuments(Guid documentId, CancellationToken token)
        {
            var files = Request.Form.Files;
            if (!files.Any())
                return BadRequest();

            var requestUser = HttpContext.User.ToIdentityUser(HttpContext.TraceIdentifier);

            //TODO handle zip files !
            var commands = new List<UploadPageCommand>();
            foreach (var formFile in files)
            {
                if (formFile.Length == 0)
                    continue;

                byte[] data = null;
                using (var stream = new MemoryStream())
                {
                    await formFile.CopyToAsync(stream, token);
                    stream.Position = 0;

                    data = stream.ToArray();
                }

                commands.Add(new UploadPageCommand(requestUser)
                {
                    DocumentId = documentId,
                    Data = data,
                    Extension = Path.GetExtension(formFile.FileName),
                    FileName = Path.GetFileNameWithoutExtension(formFile.FileName),
                    Size = formFile.Length
                });
            }

            foreach (var command in commands)
            {
                var result = await _mediatr.Process(command, token);
                if (!result.Success)
                    return BadRequest(result.Exception);
            }

            return Ok();
        }
    }
}