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

namespace Sheaft.Web.Api.Controllers
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
    }
}