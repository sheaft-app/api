using AutoMapper;
using Sheaft.Domain;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class CardViewProfile : Profile
    {
        public CardViewProfile()
        {
            CreateMap<Card, CardViewModel>();
        }
    }
}