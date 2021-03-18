using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Services.Document.Commands;

namespace Sheaft.Services.Mappings
{
    public class DocumentInputProfile : Profile
    {
        public DocumentInputProfile()
        {
            CreateMap<CreateDocumentDto, CreateDocumentCommand>();
            CreateMap<ResourceIdDto, DeleteDocumentCommand>()
                    .ForMember(c => c.DocumentId, opt => opt.MapFrom(r => r.Id));
        }
    }
}
