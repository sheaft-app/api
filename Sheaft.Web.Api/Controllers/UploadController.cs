using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sheaft.Application.Commands;
using Sheaft.Application.Interop;
using Sheaft.Core;
using Sheaft.Core.Extensions;
using Sheaft.Core.Security;
using Sheaft.Exceptions;
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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private RequestUser CurrentUser
        {
            get
            {
                if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                    return _httpContextAccessor.HttpContext.User.ToIdentityUser(_httpContextAccessor.HttpContext.TraceIdentifier);
                else
                    return new RequestUser(_httpContextAccessor.HttpContext.TraceIdentifier);
            }
        }

        public UploadController(ISheaftMediatr mediatr,
            IHttpContextAccessor httpContextAccessor)
        {
            _mediatr = mediatr;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("products")]
        [Authorize(Policy = Policies.PRODUCER)]
        public async Task<IActionResult> UploadProductsCatalog(CancellationToken token)
        {
            NewRelic.Api.Agent.NewRelic.SetTransactionName("GraphQL", nameof(UploadProductsCatalog));

            var currentTransaction = NewRelic.Api.Agent.NewRelic.GetAgent().CurrentTransaction;
            currentTransaction.AddCustomAttribute("RequestIdentifier", _httpContextAccessor.HttpContext.TraceIdentifier);
            currentTransaction.AddCustomAttribute("UserIdentifier", CurrentUser.Id.ToString("N"));
            currentTransaction.AddCustomAttribute("IsAuthenticated", CurrentUser.IsAuthenticated);
            currentTransaction.AddCustomAttribute("Roles", string.Join(";", CurrentUser.Roles));

            try
            {
                var files = Request.Form.Files;
                if (!files.Any())
                    return BadRequest();

                var ids = new List<Guid>();

                foreach (var formFile in files)
                {
                    if (formFile.Length == 0)
                        continue;

                    using (var stream = new MemoryStream())
                    {
                        await formFile.CopyToAsync(stream, token);
                        var result = await _mediatr.Process(new QueueImportProductsCommand(CurrentUser) { Id = CurrentUser.Id, FileName = formFile.FileName, FileStream = stream }, token);
                        if (!result.Success)
                            return BadRequest(result);

                        ids.Add(result.Data);
                    }
                }

                return Ok(ids);
            }
            catch(Exception e)
            {
                currentTransaction.AddCustomAttribute("ExceptionMessage", e.Message);
                currentTransaction.AddCustomAttribute("ExceptionKind", ExceptionKind.Unexpected);

                return BadRequest(e);
            }
        }
    }
}