using HotChocolate.Types;
using Sheaft.Interop.Enums;
using System;

namespace Sheaft.GraphQL.Types.Enums
{
    public class OrderStatusEnumType : EnumType<OrderStatusKind>
    {
        protected override void Configure(IEnumTypeDescriptor<OrderStatusKind> descriptor)
        {
            descriptor.Value(OrderStatusKind.Accepted).Name("ACCEPTED");
            descriptor.Value(OrderStatusKind.Cancelled).Name("CANCELLED");
            descriptor.Value(OrderStatusKind.Completed).Name("COMPLETED");
            descriptor.Value(OrderStatusKind.Delivered).Name("DELIVERED");
            descriptor.Value(OrderStatusKind.Processing).Name("PROCESSING");
            descriptor.Value(OrderStatusKind.Refused).Name("REFUSED");
            descriptor.Value(OrderStatusKind.Shipping).Name("SHIPPING");
            descriptor.Value(OrderStatusKind.Waiting).Name("WAITING");
        }
    }
    public class AddressKindEnumType : EnumType<AddressKind>
    {
        protected override void Configure(IEnumTypeDescriptor<AddressKind> descriptor)
        {
            descriptor.Value(AddressKind.Billing).Name("BILLING");
            descriptor.Value(AddressKind.Contact).Name("CONTACT");
            descriptor.Value(AddressKind.Legals).Name("LEGALS");
            descriptor.Value(AddressKind.Shipping).Name("SHIPPING");
        }
    }
    public class UserKindEnumType : EnumType<UserKind>
    {
        protected override void Configure(IEnumTypeDescriptor<UserKind> descriptor)
        {
            descriptor.Value(UserKind.Consumer).Name("CONSUMER");
            descriptor.Value(UserKind.Owner).Name("OWNER");
            descriptor.Value(UserKind.User).Name("USER");
        }
    }
    public class JobKindEnumType : EnumType<JobKind>
    {
        protected override void Configure(IEnumTypeDescriptor<JobKind> descriptor)
        {
            descriptor.Value(JobKind.CreateOrders).Name("CREATE_ORDERS");
            descriptor.Value(JobKind.CreatePickingFromOrders).Name("CREATE_PICKING_FROM_ORDERS");
            descriptor.Value(JobKind.ImportProducts).Name("IMPORT_PRODUCTS");
            descriptor.Value(JobKind.ExportAccountData).Name("EXPORT_ACCOUNT_DATA");
            descriptor.Value(JobKind.ExportProducts).Name("EXPORT_PRODUCTS");
            descriptor.Value(JobKind.OrderPicking).Name("ORDER_PICKING");
        }
    }
    public class TagKindEnumType : EnumType<TagKind>
    {
        protected override void Configure(IEnumTypeDescriptor<TagKind> descriptor)
        {
            descriptor.Value(TagKind.Allergen).Name("ALLERGEN");
            descriptor.Value(TagKind.Category).Name("CATEGORY");
            descriptor.Value(TagKind.Diet).Name("DIET");
            descriptor.Value(TagKind.Ingredient).Name("INGREDIENT");
            descriptor.Value(TagKind.Label).Name("LABEL");
        }
    }
    public class ProcessStatusEnumType : EnumType<ProcessStatusKind>
    {
        protected override void Configure(IEnumTypeDescriptor<ProcessStatusKind> descriptor)
        {
            descriptor.Value(ProcessStatusKind.Cancelled).Name("CANCELLED");
            descriptor.Value(ProcessStatusKind.Done).Name("DONE");
            descriptor.Value(ProcessStatusKind.Expired).Name("EXPIRED");
            descriptor.Value(ProcessStatusKind.Failed).Name("FAILED");
            descriptor.Value(ProcessStatusKind.Paused).Name("PAUSED");
            descriptor.Value(ProcessStatusKind.Processing).Name("PROCESSING");
            descriptor.Value(ProcessStatusKind.Rollbacked).Name("ROLLBACKED");
            descriptor.Value(ProcessStatusKind.Waiting).Name("WAITING");
        }
    }
    public class NotificationKindEnumType : EnumType<NotificationKind>
    {
        protected override void Configure(IEnumTypeDescriptor<NotificationKind> descriptor)
        {
            descriptor.Value(NotificationKind.Business).Name("BUSINESS");
            descriptor.Value(NotificationKind.None).Name("NONE");
            descriptor.Value(NotificationKind.Technical).Name("TECHNICAL");
        }
    }
    public class PointKindEnumType : EnumType<PointKind>
    {
        protected override void Configure(IEnumTypeDescriptor<PointKind> descriptor)
        {
            descriptor.Value(PointKind.BugBounty).Name("BUG_BOUNTY");
            descriptor.Value(PointKind.PurchaseOrder).Name("PURCHASE_ORDER");
            descriptor.Value(PointKind.RateProduct).Name("RATE_PRODUCT");
            descriptor.Value(PointKind.Sponsoring).Name("SPONSORING");
        }
    }
    public class DeliveryKindEnumType : EnumType<DeliveryKind>
    {
        protected override void Configure(IEnumTypeDescriptor<DeliveryKind> descriptor)
        {
            descriptor.Value(DeliveryKind.Collective).Name("COLLECTIVE");
            descriptor.Value(DeliveryKind.ExternalToConsumer).Name("EXTERNAL_TO_CONSUMER");
            descriptor.Value(DeliveryKind.ExternalToStore).Name("EXTERNAL_TO_STORE");
            descriptor.Value(DeliveryKind.Farm).Name("FARM");
            descriptor.Value(DeliveryKind.Market).Name("MARKET");
            descriptor.Value(DeliveryKind.ProducerToConsumer).Name("PRODUCER_TO_CONSUMER");
            descriptor.Value(DeliveryKind.ProducerToStore).Name("PRODUCER_TO_STORE");
            descriptor.Value(DeliveryKind.Withdrawal).Name("WITHDRAWAL");
        }
    }
    public class DayOfWeekEnumType : EnumType<DayOfWeek>
    {
        protected override void Configure(IEnumTypeDescriptor<DayOfWeek> descriptor)
        {
            descriptor.Value(DayOfWeek.Monday).Name("MONDAY");
            descriptor.Value(DayOfWeek.Tuesday).Name("TUESDAY");
            descriptor.Value(DayOfWeek.Wednesday).Name("WEDNESDAY");
            descriptor.Value(DayOfWeek.Thursday).Name("THURSDAY");
            descriptor.Value(DayOfWeek.Friday).Name("FRIDAY");
            descriptor.Value(DayOfWeek.Saturday).Name("SATURDAY");
            descriptor.Value(DayOfWeek.Sunday).Name("SUNDAY");
        }
    }
    public class ProfileKindEnumType : EnumType<ProfileKind>
    {
        protected override void Configure(IEnumTypeDescriptor<ProfileKind> descriptor)
        {
            descriptor.Value(ProfileKind.Producer).Name("PRODUCER");
            descriptor.Value(ProfileKind.Store).Name("STORE");
            descriptor.Value(ProfileKind.Consumer).Name("CONSUMER");
        }
    }
    public class UnitKindEnumType : EnumType<UnitKind>
    {
        protected override void Configure(IEnumTypeDescriptor<UnitKind> descriptor)
        {
            descriptor.Value(UnitKind.g).Name("G");
            descriptor.Value(UnitKind.kg).Name("KG");
            descriptor.Value(UnitKind.l).Name("L");
            descriptor.Value(UnitKind.ml).Name("ML");
            descriptor.Value(UnitKind.unit).Name("UNIT");
        }
    }
    public class AgreementStatusEnumType : EnumType<AgreementStatusKind>
    {
        protected override void Configure(IEnumTypeDescriptor<AgreementStatusKind> descriptor)
        {
            descriptor.Value(AgreementStatusKind.Accepted).Name("ACCEPTED");
            descriptor.Value(AgreementStatusKind.Cancelled).Name("CANCELLED");
            descriptor.Value(AgreementStatusKind.Refused).Name("REFUSED");
            descriptor.Value(AgreementStatusKind.WaitingForProducerApproval).Name("WAITING_FOR_PRODUCER_APPROVAL");
            descriptor.Value(AgreementStatusKind.WaitingForStoreApproval).Name("WAITING_FOR_STORE_APPROVAL");
        }
    }
}
