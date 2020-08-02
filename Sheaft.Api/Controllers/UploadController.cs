using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sheaft.Application.Commands;
using Sheaft.Core.Extensions;
using Sheaft.Core.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sheaft.Api.Controllers
{

    [Authorize]
    [ApiController]
    [Route("upload")]
    public class UploadController : ControllerBase
    {
        private readonly IMediator _mediatr;

        public UploadController(IMediator mediatr)
        {
            _mediatr = mediatr;
        }

        [HttpPost("products")]
        [Authorize(Policy = Policies.PRODUCER)]
        public async Task<IActionResult> UploadProductsCatalog()
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
                    await formFile.CopyToAsync(stream);
                    var result = await _mediatr.Send(new QueueImportProductsCommand(requestUser) { CompanyId = requestUser.CompanyId, FileName = formFile.FileName, FileStream = stream });
                    if (!result.Success)
                        return BadRequest(result);

                    ids.Add(result.Result);
                }
            }

            return Ok(ids);
        }
    }
}