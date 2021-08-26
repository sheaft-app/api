using AutoMapper;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class DocumentViewProfile : Profile
    {
        public DocumentViewProfile()
        {
            CreateMap<Domain.Document, DocumentShortViewModel>();
            CreateMap<Domain.Document, DocumentViewModel>();
            CreateMap<DocumentViewModel, DocumentShortViewModel>();
            CreateMap<DocumentShortViewModel, DocumentViewModel>();
        }
    }
}
