using HotChocolate.Types.Sorting;
using Sheaft.Models.Dto;

namespace Sheaft.GraphQL.Sorts
{
    public class TagSortType : SortInputType<TagDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<TagDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Sortable(c => c.Name);
        }
    }
    public class OrderSortType : SortInputType<OrderDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<OrderDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Sortable(c => c.Status);
            descriptor.Sortable(c => c.CreatedOn);
        }
    }
    public class AgreementSortType : SortInputType<AgreementDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<AgreementDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Sortable(c => c.Status);
            descriptor.Sortable(c => c.CreatedOn);
        }
    }
    public class DeliveryModeSortType : SortInputType<DeliveryModeDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<DeliveryModeDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Sortable(c => c.CreatedOn);
            descriptor.Sortable(c => c.Name);
        }
    }
    public class TimeSlotSortType : SortInputType<TimeSlotDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<TimeSlotDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Sortable(c => c.Day);
        }
    }
    public class DeliveryHourSortType : SortInputType<DeliveryHourDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<DeliveryHourDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Sortable(c => c.Day);
            descriptor.Sortable(c => c.ExpectedDeliveryDate);
        }
    }
    public class ConsumerSortType : SortInputType<ConsumerDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<ConsumerDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Sortable(c => c.CreatedOn);
        }
    }
    public class DepartmentSortType : SortInputType<DepartmentDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<DepartmentDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Sortable(c => c.Code);
        }
    }
    public class RegionSortType : SortInputType<RegionDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<RegionDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Sortable(c => c.Code);
        }
    }
    public class LevelSortType : SortInputType<LevelDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<LevelDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Sortable(c => c.Number);
        }
    }
    public class RewardSortType : SortInputType<RewardDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<RewardDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Sortable(c => c.Name);
        }
    }
    public class JobSortType : SortInputType<JobDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<JobDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Sortable(c => c.Status);
            descriptor.Sortable(c => c.CreatedOn);
            descriptor.Sortable(c => c.Name);
        }
    }
    public class NotificationSortType : SortInputType<NotificationDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<NotificationDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Sortable(c => c.CreatedOn);
        }
    }
    public class ReturnableSortType : SortInputType<ReturnableDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<ReturnableDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Sortable(c => c.Name);
            descriptor.Sortable(c => c.CreatedOn);
        }
    }
    public class ProductSortType : SortInputType<ProductDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<ProductDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Sortable(c => c.Name);
            descriptor.Sortable(c => c.CreatedOn);
            descriptor.Sortable(c => c.WholeSalePrice);
            descriptor.Sortable(c => c.OnSalePrice);
        }
    }
    public class ProducerDeliveriesSortType : SortInputType<ProducerDeliveriesDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<ProducerDeliveriesDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Sortable(c => c.Name);
        }
    }
    public class PurchaseOrderSortType : SortInputType<PurchaseOrderDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<PurchaseOrderDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Sortable(c => c.Reference);
            descriptor.Sortable(c => c.Status);
            descriptor.Sortable(c => c.CreatedOn);
            descriptor.Sortable(c => c.TotalWholeSalePrice);
            descriptor.Sortable(c => c.TotalOnSalePrice);
        }
    }
    public class RatingSortType : SortInputType<RatingDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<RatingDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Sortable(c => c.Value);
            descriptor.Sortable(c => c.CreatedOn);
        }
    }
    public class ProductQuantitySortType : SortInputType<PurchaseOrderProductQuantityDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<PurchaseOrderProductQuantityDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Sortable(c => c.Name);
        }
    }
    public class QuickOrderSortType : SortInputType<QuickOrderDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<QuickOrderDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Sortable(c => c.Name);
        }
    }
}
