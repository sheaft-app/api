﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Models;
using Sheaft.Domain;
using Sheaft.GraphQL.Catalogs;
using Sheaft.GraphQL.PurchaseOrders;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class PurchaseOrderType : SheaftOutputType<PurchaseOrder>
    {
        protected override void Configure(IObjectTypeDescriptor<PurchaseOrder> descriptor)
        {
            base.Configure(descriptor);

            descriptor
                .ImplementsNode()
                .IdField(c => c.Id)
                .ResolveNode((ctx, id) =>
                    ctx.DataLoader<PurchaseOrdersByIdBatchDataLoader>().LoadAsync(id, ctx.RequestAborted));

            descriptor
                .Field(c => c.CreatedOn)
                .Name("createdOn");
                
            descriptor
                .Field(c => c.Status)
                .Name("status");
                
            descriptor
                .Field(c => c.UpdatedOn)
                .Name("updatedOn");
                
            descriptor
                .Field(c => c.Reason)
                .Name("reason");
                
            descriptor
                .Field(c => c.Comment)
                .Name("comment");
                
            descriptor
                .Field(c => c.TotalWeight)
                .Name("totalWeight");
                
            descriptor
                .Field(c => c.TotalWholeSalePrice)
                .Name("totalWholeSalePrice");
                
            descriptor
                .Field(c => c.TotalVatPrice)
                .Name("totalVatPrice");
                
            descriptor
                .Field(c => c.TotalOnSalePrice)
                .Name("totalOnSalePrice");
                
            descriptor
                .Field(c => c.TotalReturnableWholeSalePrice)
                .Name("totalReturnableWholeSalePrice");
                
            descriptor
                .Field(c => c.TotalReturnableVatPrice)
                .Name("totalReturnableVatPrice");
                
            descriptor
                .Field(c => c.TotalReturnableOnSalePrice)
                .Name("totalReturnableOnSalePrice");
                
            descriptor
                .Field(c => c.TotalProductWholeSalePrice)
                .Name("totalProductWholeSalePrice");
                
            descriptor
                .Field(c => c.TotalProductVatPrice)
                .Name("totalProductVatPrice");
                
            descriptor
                .Field(c => c.TotalProductOnSalePrice)
                .Name("totalProductOnSalePrice");
                
            descriptor
                .Field(c => c.ReturnablesCount)
                .Name("returnablesCount");
                
            descriptor
                .Field(c => c.LinesCount)
                .Name("linesCount");
                
            descriptor
                .Field(c => c.ProductsCount)
                .Name("productsCount");

            descriptor
                .Field(c => c.Reference)
                .Name("reference")
                .Type<NonNullType<StringType>>();

            descriptor
                .Field(c => c.ExpectedDelivery)
                .Name("expectedDelivery")
                .Type<NonNullType<ExpectedPurchaseOrderDeliveryType>>();

            descriptor
                .Field(c => c.SenderInfo)
                .Name("sender")
                .ResolveWith<PurchaseOrderResolvers>(c => c.GetSender(default))
                .Type<NonNullType<PurchaseOrderUserDtoType>>();

            descriptor
                .Field(c => c.VendorInfo)
                .Name("vendor")
                .ResolveWith<PurchaseOrderResolvers>(c => c.GetVendor(default))
                .Type<NonNullType<PurchaseOrderUserDtoType>>();

            descriptor
                .Field(c => c.Products)
                .Name("products")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<PurchaseOrderResolvers>(c => c.GetProducts(default, default, default, default))
                .Type<NonNullType<ListType<PurchaseOrderProductType>>>();
        }

        private class PurchaseOrderResolvers
        {
            public PurchaseOrderUserDto GetSender(PurchaseOrder purchaseOrder)
            {
                return new PurchaseOrderUserDto
                {
                    Id = purchaseOrder.ClientId,
                    Name = purchaseOrder.SenderInfo.Name,
                    Kind = purchaseOrder.SenderInfo.Kind,
                    Email = purchaseOrder.SenderInfo.Email,
                    Phone = purchaseOrder.SenderInfo.Phone,
                    Picture = purchaseOrder.SenderInfo.Picture,
                    Address = purchaseOrder.SenderInfo.Address,
                };
            }

            public PurchaseOrderUserDto GetVendor(PurchaseOrder purchaseOrder)
            {
                return new PurchaseOrderUserDto
                {
                    Id = purchaseOrder.ProducerId,
                    Name = purchaseOrder.VendorInfo.Name,
                    Kind = purchaseOrder.VendorInfo.Kind,
                    Email = purchaseOrder.VendorInfo.Email,
                    Phone = purchaseOrder.VendorInfo.Phone,
                    Picture = purchaseOrder.VendorInfo.Picture,
                    Address = purchaseOrder.VendorInfo.Address,
                };
            }

            public async Task<IEnumerable<PurchaseOrderProduct>> GetProducts(PurchaseOrder purchaseOrder,
                [ScopedService] QueryDbContext context,
                PurchaseOrderProductsByIdBatchDataLoader purchaseOrderProductsDataLoader, CancellationToken token)
            {
                var productsId = await context.Set<PurchaseOrderProduct>()
                    .Where(p => p.PurchaseOrderId == purchaseOrder.Id)
                    .Select(p => p.Id)
                    .ToListAsync(token);

                return await purchaseOrderProductsDataLoader.LoadAsync(productsId, token);
            }
        }
    }
}