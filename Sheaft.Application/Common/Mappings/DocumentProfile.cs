using AutoMapper;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Application.Common.Models.Inputs;
using Sheaft.Application.Common.Models.ViewModels;
using Sheaft.Application.Document.Commands;

namespace Sheaft.Application.Common.Mappings
{
    public class DocumentProfile : Profile
    {
        public DocumentProfile()
        {
            CreateMap<Domain.Document, DocumentDto>()
                .ForMember(m => m.Pages, opt => opt.MapFrom(e => e.Pages));

            CreateMap<Domain.Document, DocumentShortViewModel>();
            CreateMap<Domain.Document, DocumentViewModel>()
                .ForMember(m => m.Pages, opt => opt.MapFrom(e => e.Pages));

            CreateMap<CreateDocumentInput, CreateDocumentCommand>();
            CreateMap<IdInput, DeleteDocumentCommand>();
        }
    }
}
