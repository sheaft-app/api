using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Application.Common.Extensions;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Security;
using Sheaft.Application.Product.Commands;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Web.Api.Controllers
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    [ApiController]
    [Route("upload")]
    public class UploadController : Controller
    {
        private readonly ISheaftMediatr _mediatr;
        private readonly ICurrentUserService _currentUserService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private RequestUser CurrentUser => _currentUserService.GetCurrentUserInfo().Data;

        public UploadController(
            ISheaftMediatr mediatr,
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
        {
            _mediatr = mediatr;
            _currentUserService = currentUserService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("products")]
        [Microsoft.AspNetCore.Authorization.Authorize(Policy = Policies.PRODUCER)]
        public async Task<IActionResult> UploadProductsCatalog(CancellationToken token)
        {
            NewRelic.Api.Agent.NewRelic.SetTransactionName("GraphQL", nameof(UploadProductsCatalog));

            var currentTransaction = NewRelic.Api.Agent.NewRelic.GetAgent().CurrentTransaction;
            currentTransaction.AddCustomAttribute("RequestIdentifier",
                _httpContextAccessor.HttpContext.TraceIdentifier);
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
                        var result = await _mediatr.Process(
                            new QueueImportProductsCommand(CurrentUser)
                            {
                                ProducerId = CurrentUser.Id, FileName = formFile.FileName, FileStream = stream.ToArray()
                            }, token);
                        if (!result.Succeeded)
                            return BadRequest(result);

                        ids.Add(result.Data);
                    }
                }

                return Ok(ids);
            }
            catch (Exception e)
            {
                currentTransaction.AddCustomAttribute("ExceptionMessage", e.Message);
                currentTransaction.AddCustomAttribute("ExceptionKind", ExceptionKind.Unexpected);

                return BadRequest(e);
            }
        }
    }
}