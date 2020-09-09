using HotChocolate.Types;
using Sheaft.Interop.Enums;
using System;

namespace Sheaft.GraphQL.Enums
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
    public class ValidationStatusEnumType : EnumType<ValidationStatus>
    {
        protected override void Configure(IEnumTypeDescriptor<ValidationStatus> descriptor)
        {
            descriptor.Value(ValidationStatus.Created).Name("CREATED");
            descriptor.Value(ValidationStatus.OutOfDate).Name("OUT_OF_DATE");
            descriptor.Value(ValidationStatus.Refused).Name("REFUSED");
            descriptor.Value(ValidationStatus.Validated).Name("VALIDATED");
            descriptor.Value(ValidationStatus.ValidationAsked).Name("VALIDATION_ASKED");
        }
    }
    public class DocumentKindEnumType : EnumType<DocumentKind>
    {
        protected override void Configure(IEnumTypeDescriptor<DocumentKind> descriptor)
        {
            descriptor.Value(DocumentKind.AddressProof).Name("ADDRESS_PROOF");
            descriptor.Value(DocumentKind.AssociationProof).Name("ASSOCIATION_PROOF");
            descriptor.Value(DocumentKind.IdentityProof).Name("IDENTITY_PROOF");
            descriptor.Value(DocumentKind.RegistrationProof).Name("REGISTRATION_PROOF");
            descriptor.Value(DocumentKind.ShareholderProof).Name("SHAREHOLDER_PROOF");
        }
    }
    public class TransactionKindEnumType : EnumType<TransactionKind>
    {
        protected override void Configure(IEnumTypeDescriptor<TransactionKind> descriptor)
        {
            descriptor.Value(TransactionKind.Payin).Name("PAYIN");
            descriptor.Value(TransactionKind.Payout).Name("PAYOUT");
            descriptor.Value(TransactionKind.Transfer).Name("TRANSFER");
        }
    }
    public class TransactionStatusEnumType : EnumType<TransactionStatus>
    {
        protected override void Configure(IEnumTypeDescriptor<TransactionStatus> descriptor)
        {
            descriptor.Value(TransactionStatus.Created).Name("CREATED");
            descriptor.Value(TransactionStatus.Failed).Name("FAILED");
            descriptor.Value(TransactionStatus.Succeeded).Name("SUCCEEDED");
        }
    }
    public class TransactionNatureEnumType : EnumType<TransactionNature>
    {
        protected override void Configure(IEnumTypeDescriptor<TransactionNature> descriptor)
        {
            descriptor.Value(TransactionNature.Refund).Name("REFUND");
            descriptor.Value(TransactionNature.Regular).Name("REGULAR");
            descriptor.Value(TransactionNature.Repudiation).Name("REPUDIATION");
            descriptor.Value(TransactionNature.Settlement).Name("SETTLEMENT");
        }
    }
    public class PaymentKindEnumType : EnumType<PaymentKind>
    {
        protected override void Configure(IEnumTypeDescriptor<PaymentKind> descriptor)
        {
            descriptor.Value(PaymentKind.Card).Name("CARD");
            descriptor.Value(PaymentKind.Transfer).Name("TRANSFER");
        }
    }
    public class WalletKindEnumType : EnumType<WalletKind>
    {
        protected override void Configure(IEnumTypeDescriptor<WalletKind> descriptor)
        {
            descriptor.Value(WalletKind.Payments).Name("PAYMENTS");
            descriptor.Value(WalletKind.Returnable).Name("RETURNABLE");
        }
    }
    public class JobKindEnumType : EnumType<JobKind>
    {
        protected override void Configure(IEnumTypeDescriptor<JobKind> descriptor)
        {
            descriptor.Value(JobKind.CreateOrders).Name("CREATE_ORDERS");
            descriptor.Value(JobKind.CreatePickingFromOrders).Name("CREATE_PICKING_FROM_ORDERS");
            descriptor.Value(JobKind.ImportProducts).Name("IMPORT_PRODUCTS");
            descriptor.Value(JobKind.ExportUserData).Name("EXPORT_USER_DATA");
            descriptor.Value(JobKind.ExportProducts).Name("EXPORT_PRODUCTS");
            descriptor.Value(JobKind.OrderPicking).Name("ORDER_PICKING");
        }
    }
    public class LegalKindEnumType : EnumType<LegalKind>
    {
        protected override void Configure(IEnumTypeDescriptor<LegalKind> descriptor)
        {
            descriptor.Value(LegalKind.Natural).Name("NATURAL");
            descriptor.Value(LegalKind.Business).Name("BUSINESS");
            descriptor.Value(LegalKind.Individual).Name("INDIVIDUAL");
            descriptor.Value(LegalKind.Organization).Name("ORGANIZATION");
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
    public class ProcessStatusEnumType : EnumType<ProcessStatus>
    {
        protected override void Configure(IEnumTypeDescriptor<ProcessStatus> descriptor)
        {
            descriptor.Value(ProcessStatus.Cancelled).Name("CANCELLED");
            descriptor.Value(ProcessStatus.Done).Name("DONE");
            descriptor.Value(ProcessStatus.Expired).Name("EXPIRED");
            descriptor.Value(ProcessStatus.Failed).Name("FAILED");
            descriptor.Value(ProcessStatus.Paused).Name("PAUSED");
            descriptor.Value(ProcessStatus.Processing).Name("PROCESSING");
            descriptor.Value(ProcessStatus.Rollbacked).Name("ROLLBACKED");
            descriptor.Value(ProcessStatus.Waiting).Name("WAITING");
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
    public class AgreementStatusEnumType : EnumType<AgreementStatus>
    {
        protected override void Configure(IEnumTypeDescriptor<AgreementStatus> descriptor)
        {
            descriptor.Value(AgreementStatus.Accepted).Name("ACCEPTED");
            descriptor.Value(AgreementStatus.Cancelled).Name("CANCELLED");
            descriptor.Value(AgreementStatus.Refused).Name("REFUSED");
            descriptor.Value(AgreementStatus.WaitingForProducerApproval).Name("WAITING_FOR_PRODUCER_APPROVAL");
            descriptor.Value(AgreementStatus.WaitingForStoreApproval).Name("WAITING_FOR_STORE_APPROVAL");
        }
    }
}
