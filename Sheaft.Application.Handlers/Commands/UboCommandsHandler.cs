using Sheaft.Application.Commands;
using Sheaft.Application.Interop;
using System;
using MediatR;
using Sheaft.Core;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Threading;
using Sheaft.Domain.Models;
using System.Linq;

namespace Sheaft.Application.Handlers
{
    public class UboCommandsHandler : ResultsHandler,
           IRequestHandler<CreateUboCommand, Result<Guid>>,
           IRequestHandler<UpdateUboCommand, Result<bool>>,
           IRequestHandler<DeleteUboCommand, Result<bool>>
    {
        private readonly IPspService _pspService;

        public UboCommandsHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPspService pspService,
            ILogger<UboCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
        }

        public async Task<Result<Guid>> Handle(CreateUboCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var legal = await _context.GetSingleAsync<BusinessLegal>(c => c.Declaration.Id == request.DeclarationId, token);
                var ubo = new Ubo(Guid.NewGuid(), 
                    request.FirstName, 
                    request.LastName, 
                    request.BirthDate,
                    new UboAddress(request.Address.Line1, request.Address.Line2, request.Address.Zipcode, request.Address.City, request.Address.Country),
                    new BirthAddress(request.BirthPlace.City, request.BirthPlace.Country), 
                    request.Nationality);

                legal.Declaration.AddUbo(ubo);

                await _context.SaveChangesAsync(token);

                var result = await _pspService.CreateUboAsync(ubo, legal.Declaration, legal.User, token);
                if (!result.Success)
                    return Failed<Guid>(result.Exception);

                ubo.SetIdentifier(result.Data);

                await _context.SaveChangesAsync(token);
                return Ok(ubo.Id);
            });
        }

        public async Task<Result<bool>> Handle(UpdateUboCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var legal = await _context.GetSingleAsync<BusinessLegal>(c => c.Declaration.Ubos.Any(u => u.Id == request.Id), token);
                var ubo = legal.Declaration.Ubos.FirstOrDefault(u => u.Id == request.Id);

                ubo.SetFirstName(request.FirstName);
                ubo.SetLastName(request.LastName);
                ubo.SetBirthDate(request.BirthDate);

                var address = new UboAddress(request.Address.Line1, request.Address.Line2, request.Address.Zipcode, request.Address.City, request.Address.Country);
                ubo.SetAddress(address);

                var birthPlace = new BirthAddress(request.BirthPlace.City, request.BirthPlace.Country);
                ubo.SetBirthPlace(birthPlace);

                var result = await _pspService.UpdateUboAsync(ubo, legal.Declaration, legal.User, token);
                if (!result.Success)
                    return Failed<bool>(result.Exception);

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(DeleteUboCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var legal = await _context.GetSingleAsync<BusinessLegal>(c => c.Declaration.Ubos.Any(u => u.Id == request.Id), token);
                legal.Declaration.RemoveUbo(request.Id);

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }
    }
}