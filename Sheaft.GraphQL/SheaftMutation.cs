using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Types;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;

namespace Sheaft.GraphQL
{
    public abstract class SheaftMutation
    {
        protected readonly ICurrentUserService _currentUserService;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        
        protected RequestUser CurrentUser => _currentUserService.GetCurrentUserInfo().Data;
        
        protected SheaftMutation(
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
        {
            _currentUserService = currentUserService;
            _httpContextAccessor = httpContextAccessor;
        }
       
        protected async Task<bool> ExecuteAsync<T>(ISheaftMediatr mediator, T input, CancellationToken token,
            [CallerMemberName] string memberName = null) where T : ICommand
        {
            SetLogTransaction(input, typeof(T).Name);
        
            input.SetRequestUser(CurrentUser);
            var result = await mediator.Process(input, token);
            if (result.Succeeded)
                return true;
            
            throw new SheaftException(result);
        }
        
        protected async Task<TU> ExecuteAsync<T, TU>(ISheaftMediatr mediator, T input, CancellationToken token,
            [CallerMemberName] string memberName = null) where T : ICommand<TU>
        {
            SetLogTransaction(input, typeof(T).Name);
        
            input.SetRequestUser(CurrentUser);
            var result = await mediator.Process(input, token);
            if (result.Succeeded)
                return result.Data;
        
            throw new SheaftException(result);
        }
        
        private void SetLogTransaction(object input, string name)
        {
            NewRelic.Api.Agent.NewRelic.SetTransactionName("GraphQL", name);
            var currentTransaction = NewRelic.Api.Agent.NewRelic.GetAgent().CurrentTransaction;
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