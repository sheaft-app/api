using AutoMapper;
using Sheaft.Application.Commands;
using Sheaft.Domain.Models;
using Sheaft.Models.Dto;
using Sheaft.Models.Inputs;
using Sheaft.Models.ViewModels;

namespace Sheaft.Mappers
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
