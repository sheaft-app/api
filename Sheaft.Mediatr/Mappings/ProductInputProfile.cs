using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Mediatr.Product.Commands;

namespace Sheaft.Mediatr.Mappings
{
    public class ProductInputProfile : Profile
    {
        public ProductInputProfile()
        {
            CreateMap<UpdateResourceIdPictureDto, UpdateProductPreviewCommand>()
                .ForMember(c => c.ProductId, opt => opt.MapFrom(d => d.Id));

            CreateMap<AddPictureToResourceIdDto, AddPictureToProductCommand>()
                .ForMember(c => c.ProductId, opt => opt.MapFrom(r => r.Id));

            CreateMap<ResourceIdDto, RemoveProductPictureCommand>();
            CreateMap<ResourceIdsDto, RemoveProductPicturesCommand>();
            
            CreateMap<CreateProductDto, CreateProductCommand>();
            CreateMap<UpdateProductDto, UpdateProductCommand>()
                .ForMember(c => c.ProductId, opt => opt.MapFrom(r => r.Id));
            CreateMap<RateProductDto, RateProductCommand>()
                .ForMember(c => c.ProductId, opt => opt.MapFrom(r => r.Id));
            CreateMap<UpdateResourceIdPictureDto, UpdateProductPreviewCommand>()
                .ForMember(c => c.ProductId, opt => opt.MapFrom(r => r.Id));
            CreateMap<SetResourceIdsAvailabilityDto, SetProductsAvailabilityCommand>()
                .ForMember(c => c.ProductIds, opt => opt.MapFrom(r => r.Ids));
            CreateMap<SetResourceIdsVisibilityDto, SetProductsSearchabilityCommand>()
                .ForMember(c => c.ProductIds, opt => opt.MapFrom(r => r.Ids));
            CreateMap<ResourceIdsDto, DeleteProductsCommand>()
                .ForMember(c => c.ProductIds, opt => opt.MapFrom(r => r.Ids));;
        }
    }
}
