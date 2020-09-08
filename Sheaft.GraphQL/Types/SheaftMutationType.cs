﻿using HotChocolate.Types;

namespace Sheaft.GraphQL
{
    public class SheaftMutationType : ObjectType<SheaftMutation>
    {
        protected override void Configure(IObjectTypeDescriptor<SheaftMutation> descriptor)
        {
            descriptor.Field(c => c.AcceptAgreementAsync(default, default));
            descriptor.Field(c => c.AcceptPurchaseOrdersAsync(default, default));
            descriptor.Field(c => c.ArchiveJobsAsync(default, default));
            descriptor.Field(c => c.CancelAgreementsAsync(default, default));
            descriptor.Field(c => c.CancelJobsAsync(default, default));
            descriptor.Field(c => c.CancelPurchaseOrdersAsync(default, default));
            descriptor.Field(c => c.CompletePurchaseOrdersAsync(default, default));
            descriptor.Field(c => c.CreateAgreementAsync(default, default));
            descriptor.Field(c => c.CreateDeliveryModeAsync(default, default));
            descriptor.Field(c => c.CreatePackagingAsync(default, default));
            descriptor.Field(c => c.CreateProductAsync(default, default));
            descriptor.Field(c => c.CreatePurchaseOrdersAsync(default, default));
            descriptor.Field(c => c.CreateQuickOrderAsync(default, default));
            descriptor.Field(c => c.DeleteCompanyAsync(default));
            descriptor.Field(c => c.DeleteDeliveryModeAsync(default));
            descriptor.Field(c => c.DeletePackagingAsync(default));
            descriptor.Field(c => c.DeleteProductsAsync(default));
            descriptor.Field(c => c.DeletePurchaseOrdersAsync(default));
            descriptor.Field(c => c.DeleteQuickOrdersAsync(default));
            descriptor.Field(c => c.DeleteUserAsync(default));
            descriptor.Field(c => c.DeliverPurchaseOrdersAsync(default, default));
            descriptor.Field(c => c.ExportUserDataAsync(default, default));
            descriptor.Field(c => c.ExportPickingOrdersAsync(default, default));
            descriptor.Field(c => c.GenerateUserSponsoringCodeAsync(default));
            descriptor.Field(c => c.MarkMyNotificationsAsReadAsync());
            descriptor.Field(c => c.MarkNotificationAsReadAsync(default, default));
            descriptor.Field(c => c.PauseJobsAsync(default, default));
            descriptor.Field(c => c.ProcessPurchaseOrdersAsync(default, default));
            descriptor.Field(c => c.RateProductAsync(default, default));
            descriptor.Field(c => c.RefuseAgreementsAsync(default, default));
            descriptor.Field(c => c.RefusePurchaseOrdersAsync(default, default));
            descriptor.Field(c => c.RegisterProducerAsync(default, default));
            descriptor.Field(c => c.RegisterStoreAsync(default, default));
            descriptor.Field(c => c.RegisterConsumerAsync(default, default));
            descriptor.Field(c => c.ResumeJobsAsync(default, default));
            descriptor.Field(c => c.RetryJobsAsync(default, default));
            descriptor.Field(c => c.SetDefaultQuickOrderAsync(default, default));
            descriptor.Field(c => c.SetProductsAvailabilityAsync(default, default));
            descriptor.Field(c => c.ShipPurchaseOrdersAsync(default, default));
            descriptor.Field(c => c.UpdateProducerAsync(default, default));
            descriptor.Field(c => c.UpdateStoreAsync(default, default));
            descriptor.Field(c => c.UpdateCompanyPictureAsync(default));
            descriptor.Field(c => c.UpdateDeliveryModeAsync(default, default));
            descriptor.Field(c => c.UpdatePackagingAsync(default, default));
            descriptor.Field(c => c.UpdateProductAsync(default, default));
            descriptor.Field(c => c.UpdateProductPictureAsync(default, default));
            descriptor.Field(c => c.UpdatePurchaseOrderProductsAsync(default, default));
            descriptor.Field(c => c.UpdateQuickOrderAsync(default, default));
            descriptor.Field(c => c.UpdateQuickOrderProductsAsync(default, default));
            descriptor.Field(c => c.UpdateConsumerAsync(default, default));
            descriptor.Field(c => c.UpdateUserPictureAsync(default, default));
        }
    }
}
