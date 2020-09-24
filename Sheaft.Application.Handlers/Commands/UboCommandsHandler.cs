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
           IRequestHandler<RemoveUboCommand, Result<bool>>
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
            return await ExecuteAsync(async () =>
            {
                var legal = await _context.GetByIdAsync<BusinessLegal>(request.LegalId, token);
                var ubo = new Ubo(Guid.NewGuid(), 
                    request.FirstName, 
                    request.LastName, 
                    request.BirthDate,
                    new UboAddress(request.Address.Line1, request.Address.Line2, request.Address.Zipcode, request.Address.City, request.Address.Country),
                    new BirthAddress(request.BirthPlace.City, request.BirthPlace.Country), 
                    request.Nationality);

                await _context.AddAsync(ubo);
                await _context.SaveChangesAsync(token);

                var result = await _pspService.CreateUboAsync(ubo, legal.UboDeclaration, legal.Business, token);
                if (!result.Success)
                    return Failed<Guid>(result.Exception);

                ubo.SetIdentifier(result.Data);

                _context.Update(ubo);
                await _context.SaveChangesAsync(token);

                return Ok(ubo.Id);
            });
        }

        public async Task<Result<bool>> Handle(UpdateUboCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var legal = await _context.GetSingleAsync<BusinessLegal>(c => c.UboDeclaration.Ubos.Any(u => u.Id == request.Id), token);
                var ubo = await _context.GetByIdAsync<Ubo>(request.Id, token);

                ubo.SetFirstName(request.FirstName);
                ubo.SetLastName(request.LastName);
                ubo.SetBirthDate(request.BirthDate);

                var address = new UboAddress(request.Address.Line1, request.Address.Line2, request.Address.Zipcode, request.Address.City, request.Address.Country);
                ubo.SetAddress(address);

                var birthPlace = new BirthAddress(request.BirthPlace.City, request.BirthPlace.Country);
                ubo.SetBirthPlace(birthPlace);

                var result = await _pspService.UpdateUboAsync(ubo, legal.UboDeclaration, legal.Business, token);
                if (!result.Success)
                    return Failed<bool>(result.Exception);

                _context.Update(ubo);
                var success = await _context.SaveChangesAsync(token) > 0;

                return Ok(success);
            });
        }

        public async Task<Result<bool>> Handle(RemoveUboCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var ubo = await _context.GetByIdAsync<Ubo>(request.Id, token);
                _context.Remove(ubo);

                return Ok(await _context.SaveChangesAsync(token) > 0);
            });
        }
    }
}