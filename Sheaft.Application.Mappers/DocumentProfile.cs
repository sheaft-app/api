using AutoMapper;
using Sheaft.Application.Commands;
using Sheaft.Domain.Models;
using Sheaft.Application.Models;

namespace Sheaft.Application.Mappers
{
    public class DocumentProfile : Profile
    {
        public DocumentProfile()
        {
            CreateMap<Document, DocumentDto>()
                .ForMember(m => m.Pages, opt => opt.MapFrom(e => e.Pages));

            CreateMap<CreateDocumentInput, CreateDocumentCommand>();
            CreateMap<IdInput, RemoveDocumentCommand>();
        }
    }
}
