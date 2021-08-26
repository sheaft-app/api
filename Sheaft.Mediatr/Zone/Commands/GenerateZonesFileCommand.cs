using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Zone.Commands
{
    public class GenerateZonesFileCommand : Command
    {
        protected GenerateZonesFileCommand()
        {
            
        }
        [JsonConstructor]
        public GenerateZonesFileCommand(RequestUser requestUser) : base(requestUser)
        {
        }
    }

    public class GenerateZonesFileCommandHandler : CommandsHandler,
        IRequestHandler<GenerateZonesFileCommand, Result>
    {
        private readonly IBlobService _blobService;

        public GenerateZonesFileCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IBlobService blobService,
            ILogger<GenerateZonesFileCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _blobService = blobService;
        }

        public async Task<Result> Handle(GenerateZonesFileCommand request, CancellationToken token)
        {
            var departments = await _context.Departments.ToListAsync(token);
            var depts = departments.Select(d => new DepartmentProgress
            {
                Code = d.Code,
                Name = d.Name,
                Points = d.Points,
                Position = d.Position,
                ProducersCount = d.ProducersCount,
                ProducersRequired = d.RequiredProducers,
                ConsumersCount = d.ConsumersCount,
                StoresCount = d.StoresCount
            }).ToList();

            await _blobService.UploadDepartmentsProgressAsync(
                Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(depts)), token);

            return Success();
        }

        private class DepartmentProgress
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public int Position { get; set; }
            public int Points { get; set; }
            public int ProducersCount { get; set; }
            public int? ProducersRequired { get; set; }
            public int ConsumersCount { get; set; }
            public int StoresCount { get; set; }
        }
    }
}