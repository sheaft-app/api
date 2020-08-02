﻿using HotChocolate.Types.Filters;
using Sheaft.Models.Dto;

namespace Sheaft.GraphQL.Types.Filters
{
    public class TagFilterType : FilterInputType<TagDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<TagDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Filter(c => c.Id).AllowEquals();
            descriptor.Filter(c => c.Kind).AllowIn();
            descriptor.Filter(c => c.Name).AllowContains();
        }
    }
    public class AddressFilterType : FilterInputType<AddressDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<AddressDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Filter(c => c.Line1).AllowContains();
            descriptor.Filter(c => c.Zipcode).AllowEquals();
            descriptor.Filter(c => c.City).AllowEquals();
        }
    }
    public class AgreementFilterType : FilterInputType<AgreementDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<AgreementDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Filter(c => c.Id).AllowEquals();
            descriptor.Filter(c => c.Status).AllowIn();
        }
    }
    public class DeliveryModeFilterType : FilterInputType<DeliveryModeDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<DeliveryModeDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Filter(c => c.Id).AllowEquals();
            descriptor.Filter(c => c.Kind).AllowIn();
            descriptor.Filter(c => c.Kind).AllowNotIn();
        }
    }
    public class DeliveryFilterType : FilterInputType<DeliveryDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<DeliveryDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Filter(c => c.Id).AllowEquals();
            descriptor.Filter(c => c.Kind).AllowIn();
        }
    }
    public class TimeSlotFilterType : FilterInputType<TimeSlotDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<TimeSlotDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Filter(c => c.Day).AllowIn();

            descriptor.Filter(c => c.From).AllowGreaterThan();
            descriptor.Filter(c => c.From).AllowGreaterThanOrEquals();
            descriptor.Filter(c => c.From).AllowLowerThan();
            descriptor.Filter(c => c.From).AllowLowerThanOrEquals();

            descriptor.Filter(c => c.To).AllowGreaterThan();
            descriptor.Filter(c => c.To).AllowGreaterThanOrEquals();
            descriptor.Filter(c => c.To).AllowLowerThan();
            descriptor.Filter(c => c.To).AllowLowerThanOrEquals();
        }
    }
    public class DeliveryHourFilterType : FilterInputType<DeliveryHourDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<DeliveryHourDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Filter(c => c.Day).AllowIn();

            descriptor.Filter(c => c.From).AllowGreaterThan();
            descriptor.Filter(c => c.From).AllowGreaterThanOrEquals();
            descriptor.Filter(c => c.From).AllowLowerThan();
            descriptor.Filter(c => c.From).AllowLowerThanOrEquals();

            descriptor.Filter(c => c.To).AllowGreaterThan();
            descriptor.Filter(c => c.To).AllowGreaterThanOrEquals();
            descriptor.Filter(c => c.To).AllowLowerThan();
            descriptor.Filter(c => c.To).AllowLowerThanOrEquals();

            descriptor.Filter(c => c.ExpectedDeliveryDate).AllowGreaterThan();
            descriptor.Filter(c => c.ExpectedDeliveryDate).AllowGreaterThanOrEquals();
            descriptor.Filter(c => c.ExpectedDeliveryDate).AllowLowerThan();
            descriptor.Filter(c => c.ExpectedDeliveryDate).AllowLowerThanOrEquals();
        }
    }
    public class UserFilterType : FilterInputType<UserProfileDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<UserProfileDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Filter(c => c.Id).AllowEquals();
        }
    }
    public class StoreFilterType : FilterInputType<StoreDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<StoreDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Filter(c => c.Id).AllowEquals();
            descriptor.Filter(c => c.Name).AllowContains();
        }
    }
    public class ProducerFilterType : FilterInputType<ProducerDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<ProducerDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Filter(c => c.Id).AllowEquals();
            descriptor.Filter(c => c.Name).AllowContains();
        }
    }
    public class CompanyFilterType : FilterInputType<CompanyDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<CompanyDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Filter(c => c.Id).AllowEquals();
            descriptor.Filter(c => c.Name).AllowContains();
            descriptor.Filter(c => c.AppearInBusinessSearchResults).AllowEquals();
            descriptor.Filter(c => c.Kind).AllowIn();
            descriptor.Filter(c => c.Siret).AllowEquals();
        }
    }
    public class DepartmentFilterType : FilterInputType<DepartmentDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<DepartmentDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Filter(c => c.Id).AllowEquals();
            descriptor.Filter(c => c.Name).AllowContains();
            descriptor.Filter(c => c.Code).AllowEquals();
        }
    }
    public class RegionFilterType : FilterInputType<RegionDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<RegionDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Filter(c => c.Id).AllowEquals();
            descriptor.Filter(c => c.Name).AllowContains();
            descriptor.Filter(c => c.Code).AllowEquals();
        }
    }
    public class LevelFilterType : FilterInputType<LevelDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<LevelDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Filter(c => c.Number).AllowEquals();
        }
    }
    public class RewardFilterType : FilterInputType<RewardDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<RewardDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Filter(c => c.Id).AllowEquals();
            descriptor.Filter(c => c.Name).AllowContains();
        }
    }
    public class ProfileFilterType : FilterInputType<UserProfileDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<UserProfileDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Filter(c => c.Id).AllowEquals();
            descriptor.Filter(c => c.Name).AllowContains();
            descriptor.Filter(c => c.Kind).AllowIn();
        }
    }
    public class ExpectedDeliveryFilterType : FilterInputType<ExpectedDeliveryDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<ExpectedDeliveryDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Filter(c => c.Day).AllowIn();
            descriptor.Filter(c => c.ExpectedDeliveryDate).AllowGreaterThan();
            descriptor.Filter(c => c.ExpectedDeliveryDate).AllowGreaterThanOrEquals();
            descriptor.Filter(c => c.ExpectedDeliveryDate).AllowLowerThan();
            descriptor.Filter(c => c.ExpectedDeliveryDate).AllowLowerThanOrEquals();
        }
    }
    public class JobFilterType : FilterInputType<JobDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<JobDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Filter(c => c.Id).AllowEquals();
            descriptor.Filter(c => c.Status).AllowIn();
            descriptor.Filter(c => c.Kind).AllowIn();
            descriptor.Filter(c => c.Name).AllowContains();
        }
    }
    public class NotificationFilterType : FilterInputType<NotificationDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<NotificationDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Filter(c => c.Id).AllowEquals();
            descriptor.Filter(c => c.Kind).AllowIn();
            descriptor.Filter(c => c.Unread).AllowEquals();
        }
    }
    public class PackagingFilterType : FilterInputType<PackagingDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<PackagingDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Filter(c => c.Id).AllowEquals();
            descriptor.Filter(c => c.Name).AllowContains();
        }
    }
    public class ProductFilterType : FilterInputType<ProductDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<ProductDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Filter(c => c.Id).AllowEquals();
            descriptor.Filter(c => c.Available).AllowEquals();
            descriptor.Filter(c => c.Name).AllowContains();
            descriptor.Filter(c => c.Reference).AllowContains();
        }
    }
    public class PurchaseOrderFilterType : FilterInputType<PurchaseOrderDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<PurchaseOrderDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Filter(c => c.Id).AllowEquals();
            descriptor.Filter(c => c.Id).AllowIn();
            descriptor.Filter(c => c.Reference).AllowContains();
            descriptor.Filter(c => c.Status).AllowIn();
        }
    }
    public class RatingFilterType : FilterInputType<RatingDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<RatingDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Filter(c => c.Value).AllowGreaterThan();
            descriptor.Filter(c => c.Value).AllowNotGreaterThanOrEquals();
            descriptor.Filter(c => c.Value).AllowLowerThan();
            descriptor.Filter(c => c.Value).AllowLowerThanOrEquals();
            descriptor.Filter(c => c.CreatedOn).AllowGreaterThan();
            descriptor.Filter(c => c.CreatedOn).AllowNotGreaterThanOrEquals();
            descriptor.Filter(c => c.CreatedOn).AllowLowerThan();
            descriptor.Filter(c => c.CreatedOn).AllowLowerThanOrEquals();
        }
    }
    public class PurchaseOrderProductQuantityFilterType : FilterInputType<PurchaseOrderProductQuantityDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<PurchaseOrderProductQuantityDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
        }
    }
    public class QuickOrderProductQuantityFilterType : FilterInputType<QuickOrderProductQuantityDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<QuickOrderProductQuantityDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
        }
    }
    public class QuickOrderFilterType : FilterInputType<QuickOrderDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<QuickOrderDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Filter(c => c.Id).AllowEquals();
            descriptor.Filter(c => c.IsDefault).AllowEquals();
            descriptor.Filter(c => c.Name).AllowContains();
        }
    }
}
