using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Sheaft.Application.Models;
using Sheaft.Application.Queries;
using Sheaft.GraphQL.Enums;
using Sheaft.GraphQL.Filters;
using Sheaft.GraphQL.Sorts;

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
    public class DocumentType : SheaftOutputType<DocumentDto>
    {
        protected override void Configure(IObjectTypeDescriptor<DocumentDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Name);
            descriptor.Field(c => c.ReasonCode);
            descriptor.Field(c => c.ReasonMessage);

            descriptor.Field(c => c.Status)
                .Type<NonNullType<DocumentStatusEnumType>>();

            descriptor.Field(c => c.Pages)
                .Type<ListType<PageType>>();
        }
    }
    public class PageType : SheaftOutputType<PageDto>
    {
        protected override void Configure(IObjectTypeDescriptor<PageDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.FileName);
            descriptor.Field(c => c.Extension);
            descriptor.Field(c => c.Size);
        }
    }
    public class BusinessLegalType : SheaftOutputType<BusinessLegalDto>
    {
        protected override void Configure(IObjectTypeDescriptor<BusinessLegalDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Kind);
            descriptor.Field(c => c.Email);
            descriptor.Field(c => c.Address).Type<AddressType>();
            descriptor.Field(c => c.UboDeclaration).Type<UboDeclarationType>();
            descriptor.Field(c => c.Owner).Type<OwnerType>();
        }
    }
    public class ConsumerLegalType : SheaftOutputType<ConsumerLegalDto>
    {
        protected override void Configure(IObjectTypeDescriptor<ConsumerLegalDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Owner).Type<OwnerType>();
        }
    }
    public class UboDeclarationType : SheaftOutputType<UboDeclarationDto>
    {
        protected override void Configure(IObjectTypeDescriptor<UboDeclarationDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Status);
            descriptor.Field(c => c.ReasonCode);
            descriptor.Field(c => c.ReasonMessage);

            descriptor.Field(c => c.Ubos)
                .Type<ListType<UboType>>();
        }
    }
    public class UboType : SheaftOutputType<UboDto>
    {
        protected override void Configure(IObjectTypeDescriptor<UboDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();

            descriptor.Field(c => c.FirstName);
            descriptor.Field(c => c.LastName);
            descriptor.Field(c => c.BirthDate);

            descriptor.Field(c => c.Nationality)
                .Type<NonNullType<CountryIsoCodeEnumType>>();

            descriptor.Field(c => c.Address)
                .Type<AddressType>();

            descriptor.Field(c => c.BirthPlace)
                .Type<BirthAddressType>();
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
            descriptor.Field(c => c.Kind).Type<NonNullType<ProfileKindEnumType>>();
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
            descriptor.Field(c => c.Kind).Type<NonNullType<ProfileKindEnumType>>();
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
            descriptor.Field(c => c.Kind).Type<NonNullType<ProfileKindEnumType>>();
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
            descriptor.Field(c => c.Kind).Type<NonNullType<TagKindEnumType>>();
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
            descriptor.Field(c => c.Line1);
            descriptor.Field(c => c.Line2);
            descriptor.Field(c => c.Longitude);
            descriptor.Field(c => c.Latitude);
            descriptor.Field(c => c.Zipcode);
            descriptor.Field(c => c.City);
            descriptor.Field(c => c.Country)
                .Type<NonNullType<CountryIsoCodeEnumType>>();
        }
    }

    public class BirthAddressType : ObjectType<BirthAddressDto>
    {
        protected override void Configure(IObjectTypeDescriptor<BirthAddressDto> descriptor)
        {
            descriptor.Field(c => c.City);
            descriptor.Field(c => c.Country)
                .Type<NonNullType<CountryIsoCodeEnumType>>();
        }
    }
    public class OwnerType : ObjectType<OwnerDto>
    {
        protected override void Configure(IObjectTypeDescriptor<OwnerDto> descriptor)
        {
            descriptor.Field(c => c.FirstName);
            descriptor.Field(c => c.LastName);
            descriptor.Field(c => c.Email);
            descriptor.Field(c => c.BirthDate);

            descriptor.Field(c => c.CountryOfResidence)
                .Type<NonNullType<CountryIsoCodeEnumType>>();

            descriptor.Field(c => c.Nationality)
                .Type<NonNullType<CountryIsoCodeEnumType>>();

            descriptor.Field(c => c.Address)
                .Type<AddressType>();
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
                .Type<NonNullType<BusinessProfileType>>();

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
                .Type<NonNullType<BusinessProfileType>>();

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
                .Type<NonNullType<BusinessProfileType>>();
        }
    }
    public class SheaftTimeSpanType : TimeSpanType
    {
        public SheaftTimeSpanType() : base("TimeSpan", null, TimeSpanFormat.DotNet)
        {
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

    public class UserType : SheaftOutputType<UserDto>
    {
        protected override void Configure(IObjectTypeDescriptor<UserDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Kind);
            descriptor.Field(c => c.Phone);
            descriptor.Field(c => c.Picture);

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Email)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.FirstName)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.LastName)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Address)
                .Type<NonNullType<AddressType>>();
        }
    }
    public class UserProfileType : SheaftOutputType<UserProfileDto>
    {
        protected override void Configure(IObjectTypeDescriptor<UserProfileDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Kind);
            descriptor.Field(c => c.Phone);
            descriptor.Field(c => c.Picture);

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Email)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.FirstName)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.LastName)
                .Type<NonNullType<StringType>>();
        }
    }

    public class BusinessProfileType : SheaftOutputType<BusinessProfileDto>
    {
        protected override void Configure(IObjectTypeDescriptor<BusinessProfileDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Phone);
            descriptor.Field(c => c.Picture);
            descriptor.Field(c => c.Description);
            descriptor.Field(c => c.VatIdentifier);
            descriptor.Field(c => c.Siret);

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Email)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.FirstName)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.LastName)
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
            descriptor.Field(c => c.Description);
            descriptor.Field(c => c.VatIdentifier);
            descriptor.Field(c => c.OpenForNewBusiness);
            descriptor.Field(c => c.FirstName);
            descriptor.Field(c => c.LastName);
            descriptor.Field(c => c.Email);
            descriptor.Field(c => c.Siret);
            descriptor.Field(c => c.Name);

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
    public class SearchProductProducerType : SheaftOutputType<UserDto>
    {
        protected override void Configure(IObjectTypeDescriptor<UserDto> descriptor)
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

    public class ProducersSearchType : SheaftOutputType<ProducersSearchDto>
    {
        protected override void Configure(IObjectTypeDescriptor<ProducersSearchDto> descriptor)
        {
            descriptor.Field(c => c.Count);
            descriptor.Field(c => c.Producers).Type<ListType<SearchProducerType>>();
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
            descriptor.Field(c => c.VatIdentifier);
            descriptor.Field(c => c.OpenForNewBusiness);
            descriptor.Field(c => c.FirstName);
            descriptor.Field(c => c.LastName);
            descriptor.Field(c => c.Email);
            descriptor.Field(c => c.Siret);
            descriptor.Field(c => c.Name);

            descriptor.Field(c => c.Address)
                .Type<NonNullType<AddressType>>();

            descriptor.Field(c => c.Tags)
                .Type<ListType<TagType>>()
                .UseFiltering<TagFilterType>();
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
    public class CountryType : SheaftOutputType<CountryDto>
    {
        protected override void Configure(IObjectTypeDescriptor<CountryDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Code)
                .Type<NonNullType<StringType>>();
        }
    }
    public class NationalityType : SheaftOutputType<NationalityDto>
    {
        protected override void Configure(IObjectTypeDescriptor<NationalityDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Code)
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
    public class ConsumerType : SheaftOutputType<ConsumerDto>
    {
        protected override void Configure(IObjectTypeDescriptor<ConsumerDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Phone);
            descriptor.Field(c => c.Picture);
            descriptor.Field(c => c.Kind);
            descriptor.Field(c => c.CreatedOn);
            descriptor.Field(c => c.UpdatedOn);
            descriptor.Field(c => c.Anonymous);

            descriptor.Field(c => c.Address)
                .Type<AddressType>();

            descriptor.Field(c => c.FirstName)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.LastName)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Email)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Name)
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
    public class SirenBusinessType : ObjectType<SirenBusinessDto>
    {
        protected override void Configure(IObjectTypeDescriptor<SirenBusinessDto> descriptor)
        {
            descriptor.Field(c => c.Siren);
            descriptor.Field(c => c.Nic);
            descriptor.Field(c => c.Siret);

            descriptor.Field("name")
                .Resolver(c => c.Parent<SirenBusinessDto>().UniteLegale.DenominationUsuelle1UniteLegale);

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
    public class ReturnableType : SheaftOutputType<ReturnableDto>
    {
        protected override void Configure(IObjectTypeDescriptor<ReturnableDto> descriptor)
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
            descriptor.Field(c => c.Conditioning);
            descriptor.Field(c => c.UpdatedOn);
            descriptor.Field(c => c.Description);
            descriptor.Field(c => c.Weight);
            descriptor.Field(c => c.Rating);
            descriptor.Field(c => c.Picture);
            descriptor.Field(c => c.ImageLarge);
            descriptor.Field(c => c.ImageMedium);
            descriptor.Field(c => c.ImageSmall);
            descriptor.Field(c => c.IsReturnable);

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Reference)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Returnable)
                .Type<ReturnableType>();

            descriptor.Field(c => c.Producer)
                .Type<NonNullType<BusinessProfileType>>();
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
            descriptor.Field(c => c.Conditioning);
            descriptor.Field(c => c.Rating);
            descriptor.Field(c => c.ImageMedium);
            descriptor.Field(c => c.Picture);
            descriptor.Field(c => c.IsReturnable);

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
            descriptor.Field(c => c.Conditioning);
            descriptor.Field(c => c.UpdatedOn);
            descriptor.Field(c => c.Description);
            descriptor.Field(c => c.Weight);
            descriptor.Field(c => c.Rating);
            descriptor.Field(c => c.Picture);
            descriptor.Field(c => c.ImageLarge);
            descriptor.Field(c => c.ImageMedium);
            descriptor.Field(c => c.ImageSmall);
            descriptor.Field(c => c.IsReturnable);
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

            descriptor.Field(c => c.Returnable)
                .Type<ReturnableType>();

            descriptor.Field(c => c.Ratings)
                .Type<ListType<RatingType>>()
                .UseSorting<RatingSortType>()
                .UseFiltering<RatingFilterType>()
                .UsePaging<RatingType>();

            descriptor.Field(c => c.Tags)
                .Type<ListType<TagType>>()
                .UseFiltering<TagFilterType>();

            descriptor.Field(c => c.Producer)
                .Type<NonNullType<BusinessProfileType>>();
        }
    }
    public class PurchaseOrderType : SheaftOutputType<PurchaseOrderDto>
    {
        protected override void Configure(IObjectTypeDescriptor<PurchaseOrderDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.CreatedOn);
            descriptor.Field(c => c.TotalWeight);

            descriptor.Field(c => c.TotalWholeSalePrice);
            descriptor.Field(c => c.TotalVatPrice);
            descriptor.Field(c => c.TotalOnSalePrice);
            descriptor.Field(c => c.TotalReturnableWholeSalePrice);
            descriptor.Field(c => c.TotalReturnableVatPrice);
            descriptor.Field(c => c.TotalReturnableOnSalePrice);
            descriptor.Field(c => c.TotalProductWholeSalePrice);
            descriptor.Field(c => c.TotalProductVatPrice);
            descriptor.Field(c => c.TotalProductOnSalePrice);

            descriptor.Field(c => c.ReturnablesCount);
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

    public class OrderType : SheaftOutputType<OrderDto>
    {
        protected override void Configure(IObjectTypeDescriptor<OrderDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Status);
            descriptor.Field(c => c.DonationKind);
            descriptor.Field(c => c.CreatedOn);

            descriptor.Field(c => c.TotalWholeSalePrice);
            descriptor.Field(c => c.TotalVatPrice);
            descriptor.Field(c => c.TotalOnSalePrice);
            descriptor.Field(c => c.TotalReturnableWholeSalePrice);
            descriptor.Field(c => c.TotalReturnableVatPrice);
            descriptor.Field(c => c.TotalReturnableOnSalePrice);
            descriptor.Field(c => c.TotalProductWholeSalePrice);
            descriptor.Field(c => c.TotalProductVatPrice);
            descriptor.Field(c => c.TotalProductOnSalePrice);
            descriptor.Field(c => c.TotalPrice);

            descriptor.Field(c => c.Donation);

            descriptor.Field(c => c.FeesPrice);
            descriptor.Field(c => c.InternalFeesPrice);
            descriptor.Field(c => c.TotalFees);

            descriptor.Field(c => c.FeesFixedAmount);
            descriptor.Field(c => c.FeesPercent);

            descriptor.Field(c => c.ReturnablesCount);
            descriptor.Field(c => c.LinesCount);
            descriptor.Field(c => c.ProductsCount);

            descriptor.Field(c => c.TotalWeight);

            descriptor.Field(c => c.User)
                .Type<NonNullType<UserProfileType>>();
        }
    }

    public class TransactionType : SheaftOutputType<TransactionDto>
    {
        protected override void Configure(IObjectTypeDescriptor<TransactionDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Status);
            descriptor.Field(c => c.UpdatedOn);
            descriptor.Field(c => c.Fees);
            descriptor.Field(c => c.Identifier);
            descriptor.Field(c => c.Reference);
            descriptor.Field(c => c.ResultCode);
            descriptor.Field(c => c.ResultMessage);

            descriptor.Field(c => c.Kind)
                .Type<NonNullType<TransactionKindEnumType>>();

            descriptor.Field(c => c.Status)
                .Type<NonNullType<TransactionStatusEnumType>>();
        }
    }

    public class PayinTransactionType : SheaftOutputType<PayinTransactionDto>
    {
        protected override void Configure(IObjectTypeDescriptor<PayinTransactionDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Status);
            descriptor.Field(c => c.Fees);
            descriptor.Field(c => c.Credited);
            descriptor.Field(c => c.Identifier);
            descriptor.Field(c => c.Reference);
            descriptor.Field(c => c.ResultCode);
            descriptor.Field(c => c.ResultMessage);

            descriptor.Field(c => c.Kind)
                .Type<NonNullType<TransactionKindEnumType>>();

            descriptor.Field(c => c.Status)
                .Type<NonNullType<TransactionStatusEnumType>>();

            descriptor.Field(c => c.CreditedUser)
                .Type<NonNullType<UserProfileType>>();
        }
    }

    public class PayoutTransactionType : SheaftOutputType<PayoutTransactionDto>
    {
        protected override void Configure(IObjectTypeDescriptor<PayoutTransactionDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Status);
            descriptor.Field(c => c.Fees);
            descriptor.Field(c => c.Debited);
            descriptor.Field(c => c.Identifier);
            descriptor.Field(c => c.Reference);
            descriptor.Field(c => c.ResultCode);
            descriptor.Field(c => c.ResultMessage);

            descriptor.Field(c => c.Kind)
                .Type<NonNullType<TransactionKindEnumType>>();

            descriptor.Field(c => c.Status)
                .Type<NonNullType<TransactionStatusEnumType>>();

            descriptor.Field(c => c.DebitedUser)
                .Type<NonNullType<UserProfileType>>();
        }
    }
    public class TransferTransactionType : SheaftOutputType<TransferTransactionDto>
    {
        protected override void Configure(IObjectTypeDescriptor<TransferTransactionDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Status);
            descriptor.Field(c => c.Fees);
            descriptor.Field(c => c.Credited);
            descriptor.Field(c => c.Debited);
            descriptor.Field(c => c.Identifier);
            descriptor.Field(c => c.Reference);
            descriptor.Field(c => c.ResultCode);
            descriptor.Field(c => c.ResultMessage);

            descriptor.Field(c => c.Kind)
                .Type<NonNullType<TransactionKindEnumType>>();

            descriptor.Field(c => c.Status)
                .Type<NonNullType<TransactionStatusEnumType>>();

            descriptor.Field(c => c.CreditedUser)
                .Type<NonNullType<UserProfileType>>();

            descriptor.Field(c => c.DebitedUser)
                .Type<NonNullType<UserProfileType>>();
        }
    }

    public class WebPayinTransactionType : SheaftOutputType<WebPayinTransactionDto>
    {
        protected override void Configure(IObjectTypeDescriptor<WebPayinTransactionDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Status);
            descriptor.Field(c => c.Identifier);
            descriptor.Field(c => c.Reference);
            descriptor.Field(c => c.ResultCode);
            descriptor.Field(c => c.ResultMessage);
            descriptor.Field(c => c.RedirectUrl);

            descriptor.Field(c => c.Kind)
                .Type<NonNullType<TransactionKindEnumType>>();

            descriptor.Field(c => c.Status)
                .Type<NonNullType<TransactionStatusEnumType>>();
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
            descriptor.Field(c => c.TotalWeight);
            descriptor.Field(c => c.UnitOnSalePrice);
            descriptor.Field(c => c.UnitVatPrice);
            descriptor.Field(c => c.UnitWholeSalePrice);
            descriptor.Field(c => c.UnitVatPrice);
            descriptor.Field(c => c.UnitWeight);

            descriptor.Field(c => c.TotalWholeSalePrice);
            descriptor.Field(c => c.TotalVatPrice);
            descriptor.Field(c => c.TotalOnSalePrice);
            descriptor.Field(c => c.TotalReturnableWholeSalePrice);
            descriptor.Field(c => c.TotalReturnableVatPrice);
            descriptor.Field(c => c.TotalReturnableOnSalePrice);
            descriptor.Field(c => c.TotalProductWholeSalePrice);
            descriptor.Field(c => c.TotalProductVatPrice);
            descriptor.Field(c => c.TotalProductOnSalePrice);

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

            descriptor.Field(c => c.Returnable)
                .Type<ReturnableType>();

            descriptor.Field(c => c.Producer)
                .Type<NonNullType<BusinessProfileType>>();
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
