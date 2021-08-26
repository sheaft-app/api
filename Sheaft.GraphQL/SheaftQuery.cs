using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Domain;

namespace Sheaft.GraphQL
{
    public abstract class SheaftQuery
    {
        protected readonly ICurrentUserService _currentUserService;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected RequestUser CurrentUser => _currentUserService.GetCurrentUserInfo().Data;

        protected SheaftQuery(
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
        {
            _currentUserService = currentUserService;
            _httpContextAccessor = httpContextAccessor;
        }
        
        protected void SetLogTransaction(object input = null, [CallerMemberName] string name = "")
        {
            NewRelic.Api.Agent.NewRelic.SetTransactionName("GraphQL", name);
            var currentTransaction = NewRelic.Api.Agent.NewRelic.GetAgent()?.CurrentTransaction;
            currentTransaction.AddCustomAttribute("GraphQL", name);
            
            if (_httpContextAccessor.HttpContext != null)
            {
                currentTransaction.AddCustomAttribute("RequestId", _httpContextAccessor.HttpContext.TraceIdentifier);
                currentTransaction.AddCustomAttribute("UserIdentifier", CurrentUser.Id.ToString("N"));
                currentTransaction.AddCustomAttribute("IsAuthenticated", CurrentUser.IsAuthenticated().ToString());
                currentTransaction.AddCustomAttribute("Roles", string.Join(";", CurrentUser.Roles));
            }

            if(input != null)
                currentTransaction.AddCustomAttribute("Input", JsonConvert.SerializeObject(input));
        }
    }
}