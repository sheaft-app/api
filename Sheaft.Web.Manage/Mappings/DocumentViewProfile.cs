using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class DocumentViewProfile : Profile
    {
        public DocumentViewProfile()
        {
            CreateMap<Domain.Document, DocumentShortViewModel>();
            CreateMap<Domain.Document, DocumentViewModel>();
            CreateMap<DocumentViewModel, DocumentDto>();
            CreateMap<DocumentViewModel, DocumentShortViewModel>();
            CreateMap<DocumentShortViewModel, DocumentViewModel>();
            CreateMap<DocumentShortViewModel, DocumentDto>();
            CreateMap<DocumentDto, DocumentViewModel>();
            CreateMap<DocumentDto, DocumentShortViewModel>();
        }
    }
}
