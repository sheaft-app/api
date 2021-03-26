using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Mediatr.Document.Commands;

namespace Sheaft.Mediatr.Mappings
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
