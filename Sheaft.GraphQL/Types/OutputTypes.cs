using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Microsoft.Extensions.Options;
using Sheaft.Application.Queries;
using Sheaft.Core.Extensions;
using Sheaft.Core.Security;
using Sheaft.GraphQL.Types.Enums;
using Sheaft.GraphQL.Types.Filters;
using Sheaft.GraphQL.Types.Sorts;
using Sheaft.Models.Dto;
using Sheaft.Options;

namespace Sheaft.GraphQL.Types
{
    public class UserPositionType : SheaftOutputType<UserPositionDto>
    {
        protected override void Configure(IObjectTypeDescriptor<UserPositionDto> descriptor)
        {
            descriptor.Field(c => c.Points);
            descriptor.Field(c => c.Position);
        }
    }
    public class ProducerDeliveriesType : SheaftOutputType<ProducerDeliveriesDto>
    {
        protected override void Configure(IObjectTypeDescriptor<ProducerDeliveriesDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();

            descriptor.Field(c => c.Deliveries)
                .Type<ListType<DeliveryType>>()
                .UseFiltering<DeliveryFilterType>();

            descriptor.Field(c => c.Name).Type<NonNullType<StringType>>();
        }
    }
    public class CountryPointsType : SheaftOutputType<CountryPointsDto>
    {

        protected override void Configure(IObjectTypeDescriptor<CountryPointsDto> descriptor)
        {
            descriptor.Field(c => c.Points);
            descriptor.Field(c => c.Users);
        }
    }
    public class RegionPointsType : SheaftOutputType<RegionPointsDto>
    {

        protected override void Configure(IObjectTypeDescriptor<RegionPointsDto> descriptor)
        {
            descriptor.Field(c => c.RegionName);
            descriptor.Field(c => c.Points);
            descriptor.Field(c => c.Position);
            descriptor.Field(c => c.RegionId).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Users);
        }
    }
    public class DepartmentPointsType : SheaftOutputType<DepartmentPointsDto>
    {

        protected override void Configure(IObjectTypeDescriptor<DepartmentPointsDto> descriptor)
        {
            descriptor.Field(c => c.DepartmentName);
            descriptor.Field(c => c.RegionName);
            descriptor.Field(c => c.Code);
            descriptor.Field(c => c.Points);
            descriptor.Field(c => c.Position);
            descriptor.Field(c => c.DepartmentId).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.RegionId).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Users);
        }
    }
    public class CountryUserPointsType : SheaftOutputType<CountryUserPointsDto>
    {

        protected override void Configure(IObjectTypeDescriptor<CountryUserPointsDto> descriptor)
        {
            descriptor.Field(c => c.Name);
            descriptor.Field(c => c.Points);
            descriptor.Field(c => c.Position);
            descriptor.Field(c => c.UserId).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Picture);
        }
    }
    public class RegionUserPointsType : SheaftOutputType<RegionUserPointsDto>
    {

        protected override void Configure(IObjectTypeDescriptor<RegionUserPointsDto> descriptor)
        {
            descriptor.Field(c => c.Name);
            descriptor.Field(c => c.Points);
            descriptor.Field(c => c.Position);
            descriptor.Field(c => c.RegionId).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.UserId).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Picture);
        }
    }
    public class DepartmentUserPointsType : SheaftOutputType<DepartmentUserPointsDto>
    {

        protected override void Configure(IObjectTypeDescriptor<DepartmentUserPointsDto> descriptor)
        {
            descriptor.Field(c => c.Name);
            descriptor.Field(c => c.Points);
            descriptor.Field(c => c.Position);
            descriptor.Field(c => c.DepartmentId).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.UserId).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Picture);
        }
    }
    public class TagType : SheaftOutputType<TagDto>
    {
        protected override void Configure(IObjectTypeDescriptor<TagDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.CreatedOn);
            descriptor.Field(c => c.UpdatedOn);
            descriptor.Field(c => c.Kind);
            descriptor.Field(c => c.Description);

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();
        }
    }
    public class SearchTagType : SheaftOutputType<TagDto>
    {
        protected override void Configure(IObjectTypeDescriptor<TagDto> descriptor)
        {
            descriptor.Name("SearchTagDto");

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();
        }
    }
    public class AddressType : ObjectType<AddressDto>
    {
        protected override void Configure(IObjectTypeDescriptor<AddressDto> descriptor)
        {
            descriptor.Field(c => c.Line1).Type<StringType>();
            descriptor.Field(c => c.Line2);
            descriptor.Field(c => c.Longitude);
            descriptor.Field(c => c.Latitude);

            descriptor.Field(c => c.Zipcode)
                .Type<StringType>();

            descriptor.Field(c => c.City)
                .Type<StringType>();
        }
    }
    public class SearchAddressType : ObjectType<AddressDto>
    {
        protected override void Configure(IObjectTypeDescriptor<AddressDto> descriptor)
        {
            descriptor.Name("SearchAddressType");

            descriptor.Field(c => c.Line1);
            descriptor.Field(c => c.Line2);
            descriptor.Field(c => c.Longitude).Type<NonNullType<DecimalType>>();
            descriptor.Field(c => c.Latitude).Type<NonNullType<DecimalType>>();

            descriptor.Field(c => c.Zipcode)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.City)
                .Type<NonNullType<StringType>>();
        }
    }
    public class SearchProductAddressType : ObjectType<AddressDto>
    {
        protected override void Configure(IObjectTypeDescriptor<AddressDto> descriptor)
        {
            descriptor.Name("SearchProductAddressType");

            descriptor.Field(c => c.Longitude).Type<NonNullType<DecimalType>>();
            descriptor.Field(c => c.Latitude).Type<NonNullType<DecimalType>>();

            descriptor.Field(c => c.Zipcode)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.City)
                .Type<NonNullType<StringType>>();
        }
    }
    public class AgreementType : SheaftOutputType<AgreementDto>
    {
        protected override void Configure(IObjectTypeDescriptor<AgreementDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.CreatedOn);
            descriptor.Field(c => c.UpdatedOn);
            descriptor.Field(c => c.Status);

            descriptor.Field(c => c.Store)
                .Type<NonNullType<CompanyProfileType>>();

            descriptor.Field(c => c.Delivery)
                .Type<NonNullType<AgreementDeliveryModeType>>();

            descriptor.Field(c => c.SelectedHours)
                .Type<ListType<TimeSlotType>>()
                .UseFiltering<TimeSlotFilterType>();
        }
    }
    public class DeliveryModeType : SheaftOutputType<DeliveryModeDto>
    {
        protected override void Configure(IObjectTypeDescriptor<DeliveryModeDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.CreatedOn);
            descriptor.Field(c => c.UpdatedOn);
            descriptor.Field(c => c.LockOrderHoursBeforeDelivery);
            descriptor.Field(c => c.Kind);
            descriptor.Field(c => c.Name);
            descriptor.Field(c => c.Description);

            descriptor.Field(c => c.Address)
                .Type<AddressType>();

            descriptor.Field(c => c.Producer)
                .Type<NonNullType<CompanyProfileType>>();

            descriptor.Field(c => c.OpeningHours)
                .Type<ListType<TimeSlotType>>()
                .UseFiltering<TimeSlotFilterType>();
        }
    }
    public class AgreementDeliveryModeType : SheaftOutputType<AgreementDeliveryModeDto>
    {
        protected override void Configure(IObjectTypeDescriptor<AgreementDeliveryModeDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.CreatedOn);
            descriptor.Field(c => c.UpdatedOn);
            descriptor.Field(c => c.LockOrderHoursBeforeDelivery);
            descriptor.Field(c => c.Name);
            descriptor.Field(c => c.Kind);
            descriptor.Field(c => c.Description);

            descriptor.Field(c => c.Address)
                .Type<AddressType>();

            descriptor.Field(c => c.Producer)
                .Type<NonNullType<CompanyProfileType>>();
        }
    }
    public class TimeSlotType : ObjectType<TimeSlotDto>
    {
        protected override void Configure(IObjectTypeDescriptor<TimeSlotDto> descriptor)
        {
            descriptor.Field(c => c.Day);
            descriptor.Field(c => c.From);
            descriptor.Field(c => c.To);
        }
    }
    public class DeliveryHourType : ObjectType<DeliveryHourDto>
    {
        protected override void Configure(IObjectTypeDescriptor<DeliveryHourDto> descriptor)
        {
            descriptor.Field(c => c.ExpectedDeliveryDate);
            descriptor.Field(c => c.Day);
            descriptor.Field(c => c.From);
            descriptor.Field(c => c.To);
        }
    }
    public class UserProfileType : SheaftOutputType<UserProfileDto>
    {
        protected override void Configure(IObjectTypeDescriptor<UserProfileDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Phone);
            descriptor.Field(c => c.Picture);

            descriptor.Field(c => c.ShortName)
                .Type<StringType>();

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Initials)
                .Type<StringType>();

            descriptor.Field(c => c.Email)
                .Type<NonNullType<StringType>>();
        }
    }
    public class CompanyProfileType : SheaftOutputType<CompanyProfileDto>
    {
        protected override void Configure(IObjectTypeDescriptor<CompanyProfileDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Phone);
            descriptor.Field(c => c.Picture);
            descriptor.Field(c => c.Description);

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Email)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Address)
                .Type<NonNullType<AddressType>>();
        }
    }

    public class SearchStoreType : SheaftOutputType<StoreDto>
    {
        protected override void Configure(IObjectTypeDescriptor<StoreDto> descriptor)
        {
            descriptor.Name("SearchStoreDto");

            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Phone);
            descriptor.Field(c => c.Picture);

            descriptor.Field(c => c.Tags)
                .Type<ListType<SearchTagType>>();

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Email)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Address)
                .Type<NonNullType<SearchAddressType>>();
        }
    }
    public class StoreType : SheaftOutputType<StoreDto>
    {
        protected override void Configure(IObjectTypeDescriptor<StoreDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Phone);
            descriptor.Field(c => c.Picture);

            descriptor.Field(c => c.Tags)
                .Type<ListType<TagType>>();

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Email)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Address)
                .Type<NonNullType<AddressType>>();

            descriptor.Field(c => c.OpeningHours)
                .Type<ListType<TimeSlotType>>()
                .UseFiltering<TimeSlotFilterType>();
        }
    }
    public class SearchProductProducerType : SheaftOutputType<CompanyProfileDto>
    {
        protected override void Configure(IObjectTypeDescriptor<CompanyProfileDto> descriptor)
        {
            descriptor.Name("SearchProductProducerDto");

            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Phone);

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Email)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Address)
                .Type<NonNullType<SearchProductAddressType>>();
        }
    }

    public class ProductsSearchType : SheaftOutputType<ProductsSearchDto>
    {
        protected override void Configure(IObjectTypeDescriptor<ProductsSearchDto> descriptor)
        {
            descriptor.Field(c => c.Count);
            descriptor.Field(c => c.Products).Type<ListType<SearchProductType>>();
        }
    }
    public class StoresSearchType : SheaftOutputType<StoresSearchDto>
    {
        protected override void Configure(IObjectTypeDescriptor<StoresSearchDto> descriptor)
        {
            descriptor.Field(c => c.Count);
            descriptor.Field(c => c.Stores).Type<ListType<SearchStoreType>>();
        }
    }

    public class SearchProducerType : SheaftOutputType<ProducerDto>
    {
        protected override void Configure(IObjectTypeDescriptor<ProducerDto> descriptor)
        {
            descriptor.Name("SearchProducerDto");

            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Phone);
            descriptor.Field(c => c.Picture);

            descriptor.Field(c => c.Tags)
                .Type<ListType<SearchTagType>>();

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Email)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Address)
                .Type<NonNullType<SearchAddressType>>();
        }
    }
    public class ProducerType : SheaftOutputType<ProducerDto>
    {
        protected override void Configure(IObjectTypeDescriptor<ProducerDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Phone);
            descriptor.Field(c => c.Picture);
            descriptor.Field(c => c.Description);

            descriptor.Field(c => c.Tags)
                .Type<ListType<TagType>>();

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Email)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Address)
                .Type<NonNullType<AddressType>>();
        }
    }
    public class CompanyType : SheaftOutputType<CompanyDto>
    {
        protected override void Configure(IObjectTypeDescriptor<CompanyDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.CreatedOn);
            descriptor.Field(c => c.UpdatedOn);
            descriptor.Field(c => c.Kind);
            descriptor.Field(c => c.Phone);
            descriptor.Field(c => c.Picture);
            descriptor.Field(c => c.Description);
            descriptor.Field(c => c.VatIdentifier);
            descriptor.Field(c => c.AppearInBusinessSearchResults);

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Email)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Siret)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Address)
                .Type<NonNullType<AddressType>>();

            descriptor.Field(c => c.Tags)
                .Type<ListType<TagType>>()
                .UseFiltering<TagFilterType>();

            descriptor.Field(c => c.OpeningHours)
                .Type<ListType<TimeSlotType>>()
                .UseFiltering<TimeSlotFilterType>();
        }
    }
    public class DepartmentType : SheaftOutputType<DepartmentDto>
    {
        protected override void Configure(IObjectTypeDescriptor<DepartmentDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();

            descriptor.Field(c => c.Code)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Level)
                .Type<LevelType>();
        }
    }
    public class RegionType : SheaftOutputType<RegionDto>
    {
        protected override void Configure(IObjectTypeDescriptor<RegionDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();

            descriptor.Field(c => c.Code)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();
        }
    }
    public class LevelType : SheaftOutputType<LevelDto>
    {
        protected override void Configure(IObjectTypeDescriptor<LevelDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.CreatedOn);
            descriptor.Field(c => c.UpdatedOn);
            descriptor.Field(c => c.Number);
            descriptor.Field(c => c.Name).Type<NonNullType<StringType>>();
            descriptor.Field(c => c.RequiredPoints);

            descriptor.Field(c => c.Rewards)
                .Type<ListType<RewardType>>()
                .UseFiltering<RewardFilterType>();
        }
    }
    public class RewardType : SheaftOutputType<RewardDto>
    {
        protected override void Configure(IObjectTypeDescriptor<RewardDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.CreatedOn);
            descriptor.Field(c => c.UpdatedOn);
            descriptor.Field(c => c.Image);
            descriptor.Field(c => c.Contact);
            descriptor.Field(c => c.Email);
            descriptor.Field(c => c.Phone);
            descriptor.Field(c => c.Url);

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();
        }
    }
    public class UserType : SheaftOutputType<UserDto>
    {
        protected override void Configure(IObjectTypeDescriptor<UserDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Phone);
            descriptor.Field(c => c.Picture);
            descriptor.Field(c => c.Kind);
            descriptor.Field(c => c.ShortName);
            descriptor.Field(c => c.CreatedOn);
            descriptor.Field(c => c.UpdatedOn);
            descriptor.Field(c => c.Anonymous);

            descriptor.Field(c => c.Department)
                .Type<DepartmentType>();

            descriptor.Field(c => c.FirstName)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.LastName)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Email)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Initials)
                .Type<NonNullType<StringType>>();
        }
    }
    public class ExpectedDeliveryType : ObjectType<ExpectedDeliveryDto>
    {
        protected override void Configure(IObjectTypeDescriptor<ExpectedDeliveryDto> descriptor)
        {
            descriptor.Field(c => c.ExpectedDeliveryDate);
            descriptor.Field(c => c.DeliveryStartedOn);
            descriptor.Field(c => c.DeliveredOn);
            descriptor.Field(c => c.Day);
            descriptor.Field(c => c.From);
            descriptor.Field(c => c.To);
            descriptor.Field(c => c.Kind);
            descriptor.Field(c => c.Name);

            descriptor.Field(c => c.Address)
                .Type<NonNullType<AddressType>>();            
        }
    }
    public class DeliveryType : ObjectType<DeliveryDto>
    {
        protected override void Configure(IObjectTypeDescriptor<DeliveryDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Kind);

            descriptor.Field(c => c.Address)
                .Type<AddressType>();

            descriptor.Field(c => c.DeliveryHours)
                .Type<ListType<DeliveryHourType>>()
                .UseSorting<DeliveryHourSortType>()
                .UseFiltering<DeliveryHourFilterType>();
        }
    }
    public class RankInformationType : ObjectType<RankInformationDto>
    {
        protected override void Configure(IObjectTypeDescriptor<RankInformationDto> descriptor)
        {
            descriptor.Field(c => c.Points);
            descriptor.Field(c => c.NextRank);
            descriptor.Field(c => c.PointsToLevelUp);

            descriptor.Field(c => c.Rank)
                .Type<NonNullType<StringType>>();
        }
    }
    public class SirenCompanyType : ObjectType<SirenCompanyDto>
    {
        protected override void Configure(IObjectTypeDescriptor<SirenCompanyDto> descriptor)
        {
            descriptor.Field(c => c.Siren);
            descriptor.Field(c => c.Nic);
            descriptor.Field(c => c.Siret);

            descriptor.Field("name")
                .Resolver(c => c.Parent<SirenCompanyDto>().UniteLegale.DenominationUsuelle1UniteLegale);

            descriptor.Field(c => c.AdresseEtablissement)
                .Name("address")
                .Type<SirenAddressType>();
            descriptor.Field(c => c.UniteLegale)

                .Name("owner")
                .Type<SirenLegalsType>();
        }
    }
    public class SirenAddressType : ObjectType<SirenAddressDto>
    {
        protected override void Configure(IObjectTypeDescriptor<SirenAddressDto> descriptor)
        {
            descriptor.Field("line1").Resolver(c =>
            {
                var source = c.Parent<SirenAddressDto>();
                return $"{source.NumeroVoieEtablissement} {source.LibelleVoieEtablissement}";
            });

            descriptor.Field(c => c.ComplementAdresseEtablissement)
                .Name("line2");

            descriptor.Field(c => c.CodePostalEtablissement)
                .Name("zipcode");

            descriptor.Field(c => c.LibelleCommuneEtablissement)
                .Name("city");
        }
    }
    public class SirenLegalsType : ObjectType<SirenLegalsDto>
    {
        protected override void Configure(IObjectTypeDescriptor<SirenLegalsDto> descriptor)
        {
            descriptor.Field(c => c.NomUniteLegale)
                .Name("lastName");

            descriptor.Field(c => c.PrenomUsuelUniteLegale)
                .Name("firstName");
        }
    }
    public class JobType : SheaftOutputType<JobDto>
    {
        protected override void Configure(IObjectTypeDescriptor<JobDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Status);
            descriptor.Field(c => c.Kind);
            descriptor.Field(c => c.CreatedOn);
            descriptor.Field(c => c.Archived);
            descriptor.Field(c => c.Retried);
            descriptor.Field(c => c.UpdatedOn);
            descriptor.Field(c => c.StartedOn);
            descriptor.Field(c => c.CompletedOn);
            descriptor.Field(c => c.Message);
            descriptor.Field(c => c.File);

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.User)
                .Type<NonNullType<UserProfileType>>();
        }
    }
    public class NotificationType : SheaftOutputType<NotificationDto>
    {
        protected override void Configure(IObjectTypeDescriptor<NotificationDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.CreatedOn);
            descriptor.Field(c => c.Kind);
            descriptor.Field(c => c.Unread);
            descriptor.Field(c => c.UpdatedOn);
            descriptor.Field(c => c.Method);
            descriptor.Field(c => c.Content);
        }
    }
    public class PackagingType : SheaftOutputType<PackagingDto>
    {
        protected override void Configure(IObjectTypeDescriptor<PackagingDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.CreatedOn);
            descriptor.Field(c => c.Vat);
            descriptor.Field(c => c.VatPrice);
            descriptor.Field(c => c.OnSalePrice);
            descriptor.Field(c => c.WholeSalePrice);
            descriptor.Field(c => c.VatPrice);
            descriptor.Field(c => c.UpdatedOn);
            descriptor.Field(c => c.Description);

            descriptor.Field(c => c.Name)
                .Type<StringType>();
        }
    }
    public class ProductType : SheaftOutputType<ProductDto>
    {
        protected override void Configure(IObjectTypeDescriptor<ProductDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.CreatedOn);
            descriptor.Field(c => c.Vat);
            descriptor.Field(c => c.VatPricePerUnit);
            descriptor.Field(c => c.OnSalePricePerUnit);
            descriptor.Field(c => c.WholeSalePricePerUnit);
            descriptor.Field(c => c.OnSalePrice);
            descriptor.Field(c => c.WholeSalePrice);
            descriptor.Field(c => c.VatPrice);
            descriptor.Field(c => c.Available);
            descriptor.Field(c => c.RatingsCount);
            descriptor.Field(c => c.QuantityPerUnit);
            descriptor.Field(c => c.Unit);
            descriptor.Field(c => c.UpdatedOn);
            descriptor.Field(c => c.Description);
            descriptor.Field(c => c.Weight);
            descriptor.Field(c => c.Rating);
            descriptor.Field(c => c.Picture);
            descriptor.Field(c => c.ImageLarge);
            descriptor.Field(c => c.ImageMedium);
            descriptor.Field(c => c.ImageSmall);
            descriptor.Field(c => c.Packaged);

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Reference)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Packaging)
                .Type<PackagingType>();

            descriptor.Field(c => c.Producer)
                .Type<NonNullType<CompanyProfileType>>();
        }
    }    
    public class SearchProductType : SheaftOutputType<ProductDto>
    {
        protected override void Configure(IObjectTypeDescriptor<ProductDto> descriptor)
        {
            descriptor.Name("SearchProductDto");

            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.OnSalePricePerUnit);
            descriptor.Field(c => c.OnSalePrice);
            descriptor.Field(c => c.RatingsCount);
            descriptor.Field(c => c.QuantityPerUnit);
            descriptor.Field(c => c.Unit);
            descriptor.Field(c => c.Rating);
            descriptor.Field(c => c.ImageMedium);
            descriptor.Field(c => c.Picture);
            descriptor.Field(c => c.Packaged);

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Tags)
                .Type<ListType<SearchTagType>>();

            descriptor.Field(c => c.Producer)
                .Type<NonNullType<SearchProductProducerType>>();
        }
    }
    public class ProductDetailsType : SheaftOutputType<ProductDto>
    {
        protected override void Configure(IObjectTypeDescriptor<ProductDto> descriptor)
        {
            descriptor.Name("ProductDetailsDto");

            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.CreatedOn);
            descriptor.Field(c => c.Vat);
            descriptor.Field(c => c.VatPricePerUnit);
            descriptor.Field(c => c.OnSalePricePerUnit);
            descriptor.Field(c => c.WholeSalePricePerUnit);
            descriptor.Field(c => c.OnSalePrice);
            descriptor.Field(c => c.WholeSalePrice);
            descriptor.Field(c => c.VatPrice);
            descriptor.Field(c => c.Available);
            descriptor.Field(c => c.RatingsCount);
            descriptor.Field(c => c.QuantityPerUnit);
            descriptor.Field(c => c.Unit);
            descriptor.Field(c => c.UpdatedOn);
            descriptor.Field(c => c.Description);
            descriptor.Field(c => c.Weight);
            descriptor.Field(c => c.Rating);
            descriptor.Field(c => c.Picture);
            descriptor.Field(c => c.ImageLarge);
            descriptor.Field(c => c.ImageMedium);
            descriptor.Field(c => c.ImageSmall);
            descriptor.Field(c => c.Packaged);
            descriptor.Field("currentUserHasRatedProduct")
                .Type<NonNullType<BooleanType>>()
                .Resolver(async c =>
                {
                    var user = GetRequestUser(c.ContextData);
                    if (!user.IsAuthenticated)
                        return false;

                    return await c.Service<IProductQueries>().ProductIsRatedByUserAsync(c.Parent<ProductDto>().Id, user.Id, user, c.RequestAborted);
                });

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Reference)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Packaging)
                .Type<PackagingType>();

            descriptor.Field(c => c.Ratings)
                .Type<ListType<RatingType>>()
                .UseSorting<RatingSortType>()
                .UseFiltering<RatingFilterType>()
                .UsePaging<RatingType>();

            descriptor.Field(c => c.Tags)
                .Type<ListType<TagType>>()
                .UseFiltering<TagFilterType>();

            descriptor.Field(c => c.Producer)
                .Type<NonNullType<CompanyProfileType>>();
        }
    }
    public class PurchaseOrderType : SheaftOutputType<PurchaseOrderDto>
    {
        protected override void Configure(IObjectTypeDescriptor<PurchaseOrderDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.CreatedOn);
            descriptor.Field(c => c.TotalWholeSalePrice);
            descriptor.Field(c => c.TotalVatPrice);
            descriptor.Field(c => c.TotalOnSalePrice);
            descriptor.Field(c => c.TotalWeight);
            descriptor.Field(c => c.LinesCount);
            descriptor.Field(c => c.ProductsCount);
            descriptor.Field(c => c.Status);
            descriptor.Field(c => c.UpdatedOn);
            descriptor.Field(c => c.Reason);
            descriptor.Field(c => c.Comment);

            descriptor.Field(c => c.Reference)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.ExpectedDelivery)
                .Type<NonNullType<ExpectedDeliveryType>>();

            descriptor.Field(c => c.Sender)
                .Type<NonNullType<UserProfileType>>();

            descriptor.Field(c => c.Vendor)
                .Type<NonNullType<UserProfileType>>();

            descriptor.Field(c => c.Products)
                .Type<NonNullType<ListType<PurchaseOrderProductQuantityType>>>()
                .UseFiltering<PurchaseOrderProductQuantityFilterType>();
        }
    }
    public class RatingType : SheaftOutputType<RatingDto>
    {
        protected override void Configure(IObjectTypeDescriptor<RatingDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.CreatedOn);
            descriptor.Field(c => c.Value);
            descriptor.Field(c => c.UpdatedOn);
            descriptor.Field(c => c.Comment);

            descriptor.Field(c => c.User)
                .Type<NonNullType<UserProfileType>>();
        }
    }
    public class PurchaseOrderProductQuantityType : SheaftOutputType<PurchaseOrderProductQuantityDto>
    {
        protected override void Configure(IObjectTypeDescriptor<PurchaseOrderProductQuantityDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Quantity);
            descriptor.Field(c => c.Vat);
            descriptor.Field(c => c.TotalVatPrice);
            descriptor.Field(c => c.TotalOnSalePrice);
            descriptor.Field(c => c.TotalWholeSalePrice);
            descriptor.Field(c => c.TotalWeight);
            descriptor.Field(c => c.UnitOnSalePrice);
            descriptor.Field(c => c.UnitVatPrice);
            descriptor.Field(c => c.UnitWholeSalePrice);
            descriptor.Field(c => c.UnitVatPrice);
            descriptor.Field(c => c.UnitWeight);

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Reference)
                .Type<NonNullType<StringType>>();
        }
    }
    public class QuickOrderProductQuantityType : SheaftOutputType<QuickOrderProductQuantityDto>
    {
        protected override void Configure(IObjectTypeDescriptor<QuickOrderProductQuantityDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Quantity);
            descriptor.Field(c => c.Vat);
            descriptor.Field(c => c.UnitOnSalePrice);
            descriptor.Field(c => c.UnitVatPrice);
            descriptor.Field(c => c.UnitWholeSalePrice);
            descriptor.Field(c => c.UnitVatPrice);
            descriptor.Field(c => c.UnitWeight);

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Reference)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Packaging)
                .Type<PackagingType>();

            descriptor.Field(c => c.Producer)
                .Type<NonNullType<CompanyProfileType>>();
        }
    }
    public class QuickOrderType : SheaftOutputType<QuickOrderDto>
    {
        protected override void Configure(IObjectTypeDescriptor<QuickOrderDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.CreatedOn);
            descriptor.Field(c => c.UpdatedOn);
            descriptor.Field(c => c.IsDefault);
            descriptor.Field(c => c.Description);

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.User)
                .Type<NonNullType<UserProfileType>>();

            descriptor.Field(c => c.Products)
                .Type<ListType<QuickOrderProductQuantityType>>()
                .UseFiltering<QuickOrderProductQuantityFilterType>();
        }
    }
}
