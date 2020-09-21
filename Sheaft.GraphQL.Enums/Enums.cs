using HotChocolate.Types;
using Sheaft.Domain.Enums;
using System;

namespace Sheaft.GraphQL.Enums
{
    public class OrderStatusEnumType : EnumType<OrderStatus>
    {
        protected override void Configure(IEnumTypeDescriptor<OrderStatus> descriptor)
        {
            descriptor.Value(OrderStatus.Created).Name("CREATED");
            descriptor.Value(OrderStatus.Waiting).Name("WAITING");
            descriptor.Value(OrderStatus.Validated).Name("VALIDATED");
            descriptor.Value(OrderStatus.Refused).Name("REFUSED");
            descriptor.Value(OrderStatus.Archived).Name("ARCHIVED");
            descriptor.Value(OrderStatus.Expired).Name("EXPIRED");
        }
    }
    public class DonationKindEnumType : EnumType<DonationKind>
    {
        protected override void Configure(IEnumTypeDescriptor<DonationKind> descriptor)
        {
            descriptor.Value(DonationKind.None).Name("NONE");
            descriptor.Value(DonationKind.Euro).Name("EURO");
            descriptor.Value(DonationKind.Rounded).Name("ROUNDED");
            descriptor.Value(DonationKind.Free).Name("FREE");
        }
    }
    public class PurchaseOrderStatusEnumType : EnumType<PurchaseOrderStatus>
    {
        protected override void Configure(IEnumTypeDescriptor<PurchaseOrderStatus> descriptor)
        {
            descriptor.Value(PurchaseOrderStatus.Accepted).Name("ACCEPTED");
            descriptor.Value(PurchaseOrderStatus.Cancelled).Name("CANCELLED");
            descriptor.Value(PurchaseOrderStatus.Completed).Name("COMPLETED");
            descriptor.Value(PurchaseOrderStatus.Delivered).Name("DELIVERED");
            descriptor.Value(PurchaseOrderStatus.Processing).Name("PROCESSING");
            descriptor.Value(PurchaseOrderStatus.Refused).Name("REFUSED");
            descriptor.Value(PurchaseOrderStatus.Shipping).Name("SHIPPING");
            descriptor.Value(PurchaseOrderStatus.Waiting).Name("WAITING");
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
    public class DocumentStatusEnumType : EnumType<DocumentStatus>
    {
        protected override void Configure(IEnumTypeDescriptor<DocumentStatus> descriptor)
        {
            descriptor.Value(DocumentStatus.NotSpecified).Name("NOT_SPECIFIED");
            descriptor.Value(DocumentStatus.Created).Name("CREATED");
            descriptor.Value(DocumentStatus.OutOfDate).Name("OUT_OF_DATE");
            descriptor.Value(DocumentStatus.Refused).Name("REFUSED");
            descriptor.Value(DocumentStatus.Validated).Name("VALIDATED");
            descriptor.Value(DocumentStatus.ValidationAsked).Name("VALIDATION_ASKED");
            descriptor.Value(DocumentStatus.WaitingForCreation).Name("WAITING_FOR_CREATION");
            descriptor.Value(DocumentStatus.WaitingForFirstOrder).Name("WAITING_FOR_FIRST_ORDER");
        }
    }
    public class DeclarationStatusEnumType : EnumType<DeclarationStatus>
    {
        protected override void Configure(IEnumTypeDescriptor<DeclarationStatus> descriptor)
        {
            descriptor.Value(DeclarationStatus.Created).Name("CREATED");
            descriptor.Value(DeclarationStatus.Incomplete).Name("INCOMPLETE");
            descriptor.Value(DeclarationStatus.Refused).Name("REFUSED");
            descriptor.Value(DeclarationStatus.Validated).Name("VALIDATED");
            descriptor.Value(DeclarationStatus.ValidationAsked).Name("VALIDATION_ASKED");
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
            descriptor.Value(TransactionKind.PayinWeb).Name("PAYIN_WEB");
            descriptor.Value(TransactionKind.PayinCard).Name("PAYIN_CARD");
            descriptor.Value(TransactionKind.PayinCheck).Name("PAYIN_CHECK");
            descriptor.Value(TransactionKind.PayinExternal).Name("PAYIN_EXTERNAL");
            descriptor.Value(TransactionKind.PayinMoney).Name("PAYIN_MONEY");
            descriptor.Value(TransactionKind.Transfer).Name("TRANSFER");
            descriptor.Value(TransactionKind.Payout).Name("PAYOUT");
            descriptor.Value(TransactionKind.RefundPayin).Name("REFUND_PAYIN");
            descriptor.Value(TransactionKind.RefundPayout).Name("REFUND_PAYOUT");
            descriptor.Value(TransactionKind.RefundTransfer).Name("REFUND_TRANSFER");
            descriptor.Value(TransactionKind.Repudiation).Name("REPUDIATION");
            descriptor.Value(TransactionKind.Settlement).Name("SETTLEMENT");
        }
    }
    public class TransactionStatusEnumType : EnumType<TransactionStatus>
    {
        protected override void Configure(IEnumTypeDescriptor<TransactionStatus> descriptor)
        {
            descriptor.Value(TransactionStatus.NotSpecified).Name("NOT_SPECIFIED");
            descriptor.Value(TransactionStatus.Created).Name("CREATED");
            descriptor.Value(TransactionStatus.Failed).Name("FAILED");
            descriptor.Value(TransactionStatus.Succeeded).Name("SUCCEEDED");
            descriptor.Value(TransactionStatus.Waiting).Name("WAITING");
        }
    }
    public class PaymentKindEnumType : EnumType<PaymentKind>
    {
        protected override void Configure(IEnumTypeDescriptor<PaymentKind> descriptor)
        {
            descriptor.Value(PaymentKind.Card).Name("CARD");
            descriptor.Value(PaymentKind.BankAccount).Name("BANK_ACCOUNT");
        }
    }
    public class WalletKindEnumType : EnumType<WalletKind>
    {
        protected override void Configure(IEnumTypeDescriptor<WalletKind> descriptor)
        {
            descriptor.Value(WalletKind.Payments).Name("PAYMENTS");
            descriptor.Value(WalletKind.Returnables).Name("RETURNABLE");
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
            descriptor.Value(UnitKind.NotSpecified).Name("NOT_SPECIFIED");
            descriptor.Value(UnitKind.g).Name("G");
            descriptor.Value(UnitKind.kg).Name("KG");
            descriptor.Value(UnitKind.l).Name("L");
            descriptor.Value(UnitKind.ml).Name("ML");
        }
    }
    public class ConditioningKindEnumType : EnumType<ConditioningKind>
    {
        protected override void Configure(IEnumTypeDescriptor<ConditioningKind> descriptor)
        {
            descriptor.Value(ConditioningKind.Bouquet).Name("BOUQUET");
            descriptor.Value(ConditioningKind.Bulk).Name("BULK");
            descriptor.Value(ConditioningKind.Box).Name("BOX");
            descriptor.Value(ConditioningKind.Bunch).Name("BUNCH");
            descriptor.Value(ConditioningKind.Piece).Name("PIECE");
            descriptor.Value(ConditioningKind.Not_Specified).Name("NOT_SPECIFIED");
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
    public class CountryIsoCodeEnumType : EnumType<CountryIsoCode>
    {
        protected override void Configure(IEnumTypeDescriptor<CountryIsoCode> descriptor)
        {
            descriptor.Value(CountryIsoCode.NotSpecified);
            descriptor.Value(CountryIsoCode.AD);
            descriptor.Value(CountryIsoCode.AE);
            descriptor.Value(CountryIsoCode.AF);
            descriptor.Value(CountryIsoCode.AG);
            descriptor.Value(CountryIsoCode.AI);
            descriptor.Value(CountryIsoCode.AL);
            descriptor.Value(CountryIsoCode.AM);
            descriptor.Value(CountryIsoCode.AO);
            descriptor.Value(CountryIsoCode.AQ);
            descriptor.Value(CountryIsoCode.AR);
            descriptor.Value(CountryIsoCode.AS);
            descriptor.Value(CountryIsoCode.AT);
            descriptor.Value(CountryIsoCode.AU);
            descriptor.Value(CountryIsoCode.AW);
            descriptor.Value(CountryIsoCode.AX);
            descriptor.Value(CountryIsoCode.AZ);
            descriptor.Value(CountryIsoCode.BA);
            descriptor.Value(CountryIsoCode.BB);
            descriptor.Value(CountryIsoCode.BD);
            descriptor.Value(CountryIsoCode.BE);
            descriptor.Value(CountryIsoCode.BF);
            descriptor.Value(CountryIsoCode.BG);
            descriptor.Value(CountryIsoCode.BH);
            descriptor.Value(CountryIsoCode.BI);
            descriptor.Value(CountryIsoCode.BJ);
            descriptor.Value(CountryIsoCode.BL);
            descriptor.Value(CountryIsoCode.BM);
            descriptor.Value(CountryIsoCode.BN);
            descriptor.Value(CountryIsoCode.BO);
            descriptor.Value(CountryIsoCode.BQ);
            descriptor.Value(CountryIsoCode.BR);
            descriptor.Value(CountryIsoCode.BS);
            descriptor.Value(CountryIsoCode.BT);
            descriptor.Value(CountryIsoCode.BV);
            descriptor.Value(CountryIsoCode.BW);
            descriptor.Value(CountryIsoCode.BY);
            descriptor.Value(CountryIsoCode.BZ);
            descriptor.Value(CountryIsoCode.CA);
            descriptor.Value(CountryIsoCode.CC);
            descriptor.Value(CountryIsoCode.CD);
            descriptor.Value(CountryIsoCode.CF);
            descriptor.Value(CountryIsoCode.CG);
            descriptor.Value(CountryIsoCode.CH);
            descriptor.Value(CountryIsoCode.CI);
            descriptor.Value(CountryIsoCode.CK);
            descriptor.Value(CountryIsoCode.CL);
            descriptor.Value(CountryIsoCode.CM);
            descriptor.Value(CountryIsoCode.CN);
            descriptor.Value(CountryIsoCode.CO);
            descriptor.Value(CountryIsoCode.CR);
            descriptor.Value(CountryIsoCode.CU);
            descriptor.Value(CountryIsoCode.CV);
            descriptor.Value(CountryIsoCode.CW);
            descriptor.Value(CountryIsoCode.CX);
            descriptor.Value(CountryIsoCode.CY);
            descriptor.Value(CountryIsoCode.CZ);
            descriptor.Value(CountryIsoCode.DE);
            descriptor.Value(CountryIsoCode.DJ);
            descriptor.Value(CountryIsoCode.DK);
            descriptor.Value(CountryIsoCode.DM);
            descriptor.Value(CountryIsoCode.DO);
            descriptor.Value(CountryIsoCode.DZ);
            descriptor.Value(CountryIsoCode.EC);
            descriptor.Value(CountryIsoCode.EE);
            descriptor.Value(CountryIsoCode.EG);
            descriptor.Value(CountryIsoCode.EH);
            descriptor.Value(CountryIsoCode.ER);
            descriptor.Value(CountryIsoCode.ES);
            descriptor.Value(CountryIsoCode.ET);
            descriptor.Value(CountryIsoCode.FI);
            descriptor.Value(CountryIsoCode.FJ);
            descriptor.Value(CountryIsoCode.FK);
            descriptor.Value(CountryIsoCode.FM);
            descriptor.Value(CountryIsoCode.FO);
            descriptor.Value(CountryIsoCode.FR);
            descriptor.Value(CountryIsoCode.GA);
            descriptor.Value(CountryIsoCode.GB);
            descriptor.Value(CountryIsoCode.GD);
            descriptor.Value(CountryIsoCode.GE);
            descriptor.Value(CountryIsoCode.GF);
            descriptor.Value(CountryIsoCode.GG);
            descriptor.Value(CountryIsoCode.GH);
            descriptor.Value(CountryIsoCode.GI);
            descriptor.Value(CountryIsoCode.GL);
            descriptor.Value(CountryIsoCode.GM);
            descriptor.Value(CountryIsoCode.GN);
            descriptor.Value(CountryIsoCode.GP);
            descriptor.Value(CountryIsoCode.GQ);
            descriptor.Value(CountryIsoCode.GR);
            descriptor.Value(CountryIsoCode.GS);
            descriptor.Value(CountryIsoCode.GT);
            descriptor.Value(CountryIsoCode.GU);
            descriptor.Value(CountryIsoCode.GW);
            descriptor.Value(CountryIsoCode.GY);
            descriptor.Value(CountryIsoCode.HK);
            descriptor.Value(CountryIsoCode.HM);
            descriptor.Value(CountryIsoCode.HN);
            descriptor.Value(CountryIsoCode.HR);
            descriptor.Value(CountryIsoCode.HT);
            descriptor.Value(CountryIsoCode.HU);
            descriptor.Value(CountryIsoCode.ID);
            descriptor.Value(CountryIsoCode.IE);
            descriptor.Value(CountryIsoCode.IL);
            descriptor.Value(CountryIsoCode.IM);
            descriptor.Value(CountryIsoCode.IN);
            descriptor.Value(CountryIsoCode.IO);
            descriptor.Value(CountryIsoCode.IQ);
            descriptor.Value(CountryIsoCode.IR);
            descriptor.Value(CountryIsoCode.IS);
            descriptor.Value(CountryIsoCode.IT);
            descriptor.Value(CountryIsoCode.JE);
            descriptor.Value(CountryIsoCode.JM);
            descriptor.Value(CountryIsoCode.JO);
            descriptor.Value(CountryIsoCode.JP);
            descriptor.Value(CountryIsoCode.KE);
            descriptor.Value(CountryIsoCode.KG);
            descriptor.Value(CountryIsoCode.KH);
            descriptor.Value(CountryIsoCode.KI);
            descriptor.Value(CountryIsoCode.KM);
            descriptor.Value(CountryIsoCode.KN);
            descriptor.Value(CountryIsoCode.KP);
            descriptor.Value(CountryIsoCode.KR);
            descriptor.Value(CountryIsoCode.KW);
            descriptor.Value(CountryIsoCode.KY);
            descriptor.Value(CountryIsoCode.KZ);
            descriptor.Value(CountryIsoCode.LA);
            descriptor.Value(CountryIsoCode.LB);
            descriptor.Value(CountryIsoCode.LC);
            descriptor.Value(CountryIsoCode.LI);
            descriptor.Value(CountryIsoCode.LK);
            descriptor.Value(CountryIsoCode.LR);
            descriptor.Value(CountryIsoCode.LS);
            descriptor.Value(CountryIsoCode.LT);
            descriptor.Value(CountryIsoCode.LU);
            descriptor.Value(CountryIsoCode.LV);
            descriptor.Value(CountryIsoCode.LY);
            descriptor.Value(CountryIsoCode.MA);
            descriptor.Value(CountryIsoCode.MC);
            descriptor.Value(CountryIsoCode.MD);
            descriptor.Value(CountryIsoCode.ME);
            descriptor.Value(CountryIsoCode.MF);
            descriptor.Value(CountryIsoCode.MG);
            descriptor.Value(CountryIsoCode.MH);
            descriptor.Value(CountryIsoCode.MK);
            descriptor.Value(CountryIsoCode.ML);
            descriptor.Value(CountryIsoCode.MM);
            descriptor.Value(CountryIsoCode.MN);
            descriptor.Value(CountryIsoCode.MO);
            descriptor.Value(CountryIsoCode.MP);
            descriptor.Value(CountryIsoCode.MQ);
            descriptor.Value(CountryIsoCode.MR);
            descriptor.Value(CountryIsoCode.MS);
            descriptor.Value(CountryIsoCode.MT);
            descriptor.Value(CountryIsoCode.MU);
            descriptor.Value(CountryIsoCode.MV);
            descriptor.Value(CountryIsoCode.MW);
            descriptor.Value(CountryIsoCode.MX);
            descriptor.Value(CountryIsoCode.MY);
            descriptor.Value(CountryIsoCode.MZ);
            descriptor.Value(CountryIsoCode.NA);
            descriptor.Value(CountryIsoCode.NC);
            descriptor.Value(CountryIsoCode.NE);
            descriptor.Value(CountryIsoCode.NF);
            descriptor.Value(CountryIsoCode.NG);
            descriptor.Value(CountryIsoCode.NI);
            descriptor.Value(CountryIsoCode.NL);
            descriptor.Value(CountryIsoCode.NO);
            descriptor.Value(CountryIsoCode.NP);
            descriptor.Value(CountryIsoCode.NR);
            descriptor.Value(CountryIsoCode.NU);
            descriptor.Value(CountryIsoCode.NZ);
            descriptor.Value(CountryIsoCode.OM);
            descriptor.Value(CountryIsoCode.PA);
            descriptor.Value(CountryIsoCode.PE);
            descriptor.Value(CountryIsoCode.PF);
            descriptor.Value(CountryIsoCode.PG);
            descriptor.Value(CountryIsoCode.PH);
            descriptor.Value(CountryIsoCode.PK);
            descriptor.Value(CountryIsoCode.PL);
            descriptor.Value(CountryIsoCode.PM);
            descriptor.Value(CountryIsoCode.PN);
            descriptor.Value(CountryIsoCode.PR);
            descriptor.Value(CountryIsoCode.PS);
            descriptor.Value(CountryIsoCode.PT);
            descriptor.Value(CountryIsoCode.PW);
            descriptor.Value(CountryIsoCode.PY);
            descriptor.Value(CountryIsoCode.QA);
            descriptor.Value(CountryIsoCode.RE);
            descriptor.Value(CountryIsoCode.RO);
            descriptor.Value(CountryIsoCode.RS);
            descriptor.Value(CountryIsoCode.RU);
            descriptor.Value(CountryIsoCode.RW);
            descriptor.Value(CountryIsoCode.SA);
            descriptor.Value(CountryIsoCode.SB);
            descriptor.Value(CountryIsoCode.SC);
            descriptor.Value(CountryIsoCode.SD);
            descriptor.Value(CountryIsoCode.SE);
            descriptor.Value(CountryIsoCode.SG);
            descriptor.Value(CountryIsoCode.SH);
            descriptor.Value(CountryIsoCode.SI);
            descriptor.Value(CountryIsoCode.SJ);
            descriptor.Value(CountryIsoCode.SK);
            descriptor.Value(CountryIsoCode.SL);
            descriptor.Value(CountryIsoCode.SM);
            descriptor.Value(CountryIsoCode.SN);
            descriptor.Value(CountryIsoCode.SO);
            descriptor.Value(CountryIsoCode.SR);
            descriptor.Value(CountryIsoCode.SS);
            descriptor.Value(CountryIsoCode.ST);
            descriptor.Value(CountryIsoCode.SV);
            descriptor.Value(CountryIsoCode.SX);
            descriptor.Value(CountryIsoCode.SY);
            descriptor.Value(CountryIsoCode.SZ);
            descriptor.Value(CountryIsoCode.TC);
            descriptor.Value(CountryIsoCode.TD);
            descriptor.Value(CountryIsoCode.TF);
            descriptor.Value(CountryIsoCode.TG);
            descriptor.Value(CountryIsoCode.TH);
            descriptor.Value(CountryIsoCode.TJ);
            descriptor.Value(CountryIsoCode.TK);
            descriptor.Value(CountryIsoCode.TL);
            descriptor.Value(CountryIsoCode.TM);
            descriptor.Value(CountryIsoCode.TN);
            descriptor.Value(CountryIsoCode.TO);
            descriptor.Value(CountryIsoCode.TR);
            descriptor.Value(CountryIsoCode.TT);
            descriptor.Value(CountryIsoCode.TV);
            descriptor.Value(CountryIsoCode.TW);
            descriptor.Value(CountryIsoCode.TZ);
            descriptor.Value(CountryIsoCode.UA);
            descriptor.Value(CountryIsoCode.UG);
            descriptor.Value(CountryIsoCode.UM);
            descriptor.Value(CountryIsoCode.US);
            descriptor.Value(CountryIsoCode.UY);
            descriptor.Value(CountryIsoCode.UZ);
            descriptor.Value(CountryIsoCode.VA);
            descriptor.Value(CountryIsoCode.VC);
            descriptor.Value(CountryIsoCode.VE);
            descriptor.Value(CountryIsoCode.VG);
            descriptor.Value(CountryIsoCode.VI);
            descriptor.Value(CountryIsoCode.VN);
            descriptor.Value(CountryIsoCode.VU);
            descriptor.Value(CountryIsoCode.WF);
            descriptor.Value(CountryIsoCode.WS);
            descriptor.Value(CountryIsoCode.XK);
            descriptor.Value(CountryIsoCode.YE);
            descriptor.Value(CountryIsoCode.YT);
            descriptor.Value(CountryIsoCode.ZA);
            descriptor.Value(CountryIsoCode.ZM);
            descriptor.Value(CountryIsoCode.ZW);
        }
    }
}
