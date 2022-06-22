import type {
  OpenAPIClient,
  Parameters,
  UnknownParamsObject,
  OperationResponse,
  AxiosRequestConfig,
} from 'openapi-client-axios'; 

declare namespace Components {
    namespace Schemas {
        export interface AcceptCustomerAgreementRequest {
            deliveryDays?: DayOfWeek /* int32 */[] | null;
            limitOrderHourOffset?: number; // int32
        }
        export interface AcceptOrderRequest {
            newDeliveryDate?: string | null; // date-time
        }
        export interface AddressDto {
            street?: string | null;
            complement?: string | null;
            postcode?: string | null;
            city?: string | null;
        }
        export interface AgreementCatalogDto {
            id?: string | null;
            name?: string | null;
        }
        export interface AgreementDetailsDto {
            id?: string | null;
            status?: AgreementStatus /* int32 */;
            createdOn?: string; // date-time
            updatedOn?: string; // date-time
            target?: AgreementProfileDto;
            catalog?: AgreementCatalogDto;
            deliveryDays?: DayOfWeek /* int32 */[] | null;
            limitOrderHourOffset?: null | number; // int32
            deliveryAddress?: AddressDto;
            ownerId?: string | null;
            owner?: AgreementOwner /* int32 */;
            canBeAcceptedOrRefused?: boolean;
            canBeCancelled?: boolean;
            canBeRevoked?: boolean;
        }
        export interface AgreementDto {
            id?: string | null;
            status?: AgreementStatus /* int32 */;
            createdOn?: string; // date-time
            updatedOn?: string; // date-time
            targetName?: string | null;
            targetId?: string | null;
            ownerId?: string | null;
            owner?: AgreementOwner /* int32 */;
            deliveryDays?: DayOfWeek /* int32 */[] | null;
            limitOrderHourOffset?: null | number; // int32
        }
        export interface AgreementDtoPaginatedResults {
            items?: AgreementDto[] | null;
            next?: string | null;
            previous?: string | null;
            pageNumber?: number; // int32
            itemsPerPage?: number; // int32
            totalItems?: number; // int32
            totalPages?: number; // int32
        }
        export type AgreementOwner = 0 | 1; // int32
        export interface AgreementProfileDto {
            id?: string | null;
            name?: string | null;
            email?: string | null;
            phone?: string | null;
        }
        export type AgreementStatus = 0 | 1 | 2 | 3 | 4; // int32
        export interface AvailableCustomerDto {
            id?: string | null;
            name?: string | null;
            email?: string | null;
            phone?: string | null;
            deliveryAddress?: AddressDto;
        }
        export interface AvailableCustomerDtoPaginatedResults {
            items?: AvailableCustomerDto[] | null;
            next?: string | null;
            previous?: string | null;
            pageNumber?: number; // int32
            itemsPerPage?: number; // int32
            totalItems?: number; // int32
            totalPages?: number; // int32
        }
        export interface AvailableSupplierDto {
            id?: string | null;
            name?: string | null;
            email?: string | null;
            phone?: string | null;
            shippingAddress?: AddressDto;
        }
        export interface AvailableSupplierDtoPaginatedResults {
            items?: AvailableSupplierDto[] | null;
            next?: string | null;
            previous?: string | null;
            pageNumber?: number; // int32
            itemsPerPage?: number; // int32
            totalItems?: number; // int32
            totalPages?: number; // int32
        }
        export type BatchDateKind = 0 | 1 | 2; // int32
        export interface CancelInvoiceRequest {
            reason?: string | null;
        }
        export interface CancelOrderRequest {
            cancellationReason?: string | null;
        }
        export interface CreateBatchRequest {
            number?: string | null;
            dateKind?: BatchDateKind /* int32 */;
            date?: DateOnly;
        }
        export interface CreatePreparationDocumentRequest {
            orderIdentifiers?: string[] | null;
            autoAcceptPendingOrders?: boolean | null;
        }
        export interface CreateProductRequest {
            name?: string | null;
            code?: string | null;
            unitPrice?: number; // double
            vat?: number; // double
            description?: string | null;
            returnableId?: string | null;
        }
        export interface CreateReturnableRequest {
            name?: string | null;
            code?: string | null;
            unitPrice?: number; // double
            vat?: number; // double
        }
        export interface CustomerInfoRequest {
            tradeName?: string | null;
            corporateName?: string | null;
            siret?: string | null;
            email?: string | null;
            phone?: string | null;
            legalAddress?: AddressDto;
            deliveryAddress?: NamedAddressDto;
            billingAddress?: NamedAddressDto;
        }
        export interface DateOnly {
            year?: number; // int32
            month?: number; // int32
            day?: number; // int32
            dayOfWeek?: DayOfWeek /* int32 */;
            dayOfYear?: number; // int32
            dayNumber?: number; // int32
        }
        export type DayOfWeek = 0 | 1 | 2 | 3 | 4 | 5 | 6; // int32
        export interface DeliverOrdersRequest {
            productsAdjustments?: LineAdjustmentRequest[] | null;
            returnedReturnables?: LineAdjustmentRequest[] | null;
        }
        export interface DeliveryLineRequest {
            productIdentifier?: string | null;
            quantity?: number; // int32
            batchIdentifiers?: string[] | null;
        }
        export type DeliveryStatus = 0 | 1 | 2; // int32
        export interface ForgotPasswordRequest {
            email?: string | null;
        }
        export interface FulfillOrderRequest {
            deliveryLines?: DeliveryLineRequest[] | null;
            newDeliveryDate?: string | null; // date-time
        }
        export interface LineAdjustmentRequest {
            identifier?: string | null;
            quantity?: number; // int32
        }
        export interface LoginRequest {
            username?: string | null;
            password?: string | null;
        }
        export interface MarkInvoiceAsPayedRequest {
            reference?: string | null;
            payedOn?: string; // date-time
            paymentKind?: PaymentKind /* int32 */;
        }
        export interface NamedAddressDto {
            name?: string | null;
            email?: string | null;
            street?: string | null;
            complement?: string | null;
            postcode?: string | null;
            city?: string | null;
        }
        export interface OrderDeliveryDto {
            id?: string | null;
            scheduledAt?: string; // date-time
            status?: DeliveryStatus /* int32 */;
            address?: NamedAddressDto;
        }
        export interface OrderDetailsDto {
            id?: string | null;
            code?: string | null;
            status?: OrderStatus /* int32 */;
            totalWholeSalePrice?: number; // double
            totalOnSalePrice?: number; // double
            totalVatPrice?: number; // double
            createdOn?: string; // date-time
            updatedOn?: string; // date-time
            publishedOn?: string | null; // date-time
            acceptedOn?: string | null; // date-time
            completedOn?: string | null; // date-time
            abortedOn?: string | null; // date-time
            fulfilledOn?: string | null; // date-time
            supplier?: OrderUserDto;
            customer?: OrderUserDto;
            lines?: OrderLineDto[] | null;
            delivery?: OrderDeliveryDto;
        }
        export interface OrderDraftDto {
            id?: string | null;
            createdOn?: string; // date-time
            updatedOn?: string; // date-time
            supplier?: OrderUserDto;
            customer?: OrderUserDto;
            lines?: OrderDraftLineDto[] | null;
        }
        export interface OrderDraftLineDto {
            kind?: OrderLineKind /* int32 */;
            identifier?: string | null;
            name?: string | null;
            code?: string | null;
            quantity?: number; // int32
            vat?: number; // double
            unitPrice?: number; // double
        }
        export interface OrderDto {
            id?: string | null;
            code?: string | null;
            status?: OrderStatus /* int32 */;
            totalWholeSalePrice?: number; // double
            totalOnSalePrice?: number; // double
            totalVatPrice?: number; // double
            createdOn?: string; // date-time
            updatedOn?: string; // date-time
            publishedOn?: string | null; // date-time
            acceptedOn?: string | null; // date-time
            completedOn?: string | null; // date-time
            abortedOn?: string | null; // date-time
            fulfilledOn?: string | null; // date-time
            deliveryStatus?: DeliveryStatus /* int32 */;
            deliveryScheduledAt?: string | null; // date-time
            customerName?: string | null;
            supplierName?: string | null;
        }
        export interface OrderDtoPaginatedResults {
            items?: OrderDto[] | null;
            next?: string | null;
            previous?: string | null;
            pageNumber?: number; // int32
            itemsPerPage?: number; // int32
            totalItems?: number; // int32
            totalPages?: number; // int32
        }
        export interface OrderLineDto {
            kind?: OrderLineKind /* int32 */;
            identifier?: string | null;
            name?: string | null;
            code?: string | null;
            quantity?: number; // int32
            vat?: number; // double
            unitPrice?: number; // double
            totalWholeSalePrice?: number; // double
            totalVatPrice?: number; // double
            totalOnSalePrice?: number; // double
        }
        export type OrderLineKind = 0 | 1; // int32
        export type OrderStatus = 0 | 1 | 2 | 3 | 4 | 5 | 6; // int32
        export interface OrderUserDto {
            id?: string | null;
            name?: string | null;
            email?: string | null;
            phone?: string | null;
        }
        export interface OrderableProductDto {
            id?: string | null;
            name?: string | null;
            code?: string | null;
            unitPrice?: number; // double
            vat?: number; // double
            updatedOn?: string; // date-time
            supplier?: OrderableProductSupplierDto;
            returnable?: OrderableReturnableDto;
        }
        export interface OrderableProductDtoPaginatedResults {
            items?: OrderableProductDto[] | null;
            next?: string | null;
            previous?: string | null;
            pageNumber?: number; // int32
            itemsPerPage?: number; // int32
            totalItems?: number; // int32
            totalPages?: number; // int32
        }
        export interface OrderableProductSupplierDto {
            id?: string | null;
            name?: string | null;
        }
        export interface OrderableReturnableDto {
            id?: string | null;
            name?: string | null;
            unitPrice?: number; // double
            vat?: number; // double
        }
        export type PaymentKind = 0 | 1; // int32
        export interface ProblemDetails {
            [name: string]: any;
            type?: string | null;
            title?: string | null;
            status?: null | number; // int32
            detail?: string | null;
            instance?: string | null;
        }
        export interface ProductDetailsDto {
            id?: string | null;
            name?: string | null;
            code?: string | null;
            description?: string | null;
            unitPrice?: number; // double
            vat?: number; // double
            returnable?: ReturnableDto;
            returnableId?: string | null;
            createdOn?: string; // date-time
            updatedOn?: string; // date-time
        }
        export interface ProductDto {
            id?: string | null;
            name?: string | null;
            code?: string | null;
            unitPrice?: number; // double
            vat?: number; // double
            updatedOn?: string; // date-time
        }
        export interface ProductDtoPaginatedResults {
            items?: ProductDto[] | null;
            next?: string | null;
            previous?: string | null;
            pageNumber?: number; // int32
            itemsPerPage?: number; // int32
            totalItems?: number; // int32
            totalPages?: number; // int32
        }
        export interface ProductQuantityDto {
            productIdentifier?: string | null;
            quantity?: number; // int32
        }
        export interface ProposeAgreementToCustomerRequest {
            deliveryDays?: DayOfWeek /* int32 */[] | null;
            orderDelayInHoursBeforeDeliveryDay?: null | number; // int32
        }
        export interface PublishOrderDraftRequest {
            deliveryDate?: string; // date-time
            products?: ProductQuantityDto[] | null;
        }
        export interface RefreshTokenRequest {
            token?: string | null;
        }
        export interface RefuseAgreementRequest {
            reason?: string | null;
        }
        export interface RefuseOrderRequest {
            refusalReason?: string | null;
        }
        export interface RegisterRequest {
            email?: string | null;
            password?: string | null;
            confirm?: string | null;
            firstname?: string | null;
            lastname?: string | null;
        }
        export interface ResetPasswordRequest {
            resetToken?: string | null;
            password?: string | null;
            confirm?: string | null;
        }
        export interface ReturnableDto {
            id?: string | null;
            name?: string | null;
            code?: string | null;
            unitPrice?: number; // double
            vat?: number; // double
            createdOn?: string; // date-time
            updatedOn?: string; // date-time
        }
        export interface ReturnableDtoPaginatedResults {
            items?: ReturnableDto[] | null;
            next?: string | null;
            previous?: string | null;
            pageNumber?: number; // int32
            itemsPerPage?: number; // int32
            totalItems?: number; // int32
            totalPages?: number; // int32
        }
        export interface RevokeAgreementRequest {
            reason?: string | null;
        }
        export interface SupplierInfoRequest {
            tradeName?: string | null;
            corporateName?: string | null;
            siret?: string | null;
            email?: string | null;
            phone?: string | null;
            legalAddress?: AddressDto;
            shippingAddress?: NamedAddressDto;
            billingAddress?: NamedAddressDto;
        }
        export interface TokenResponse {
            access_token?: string | null;
            refresh_token?: string | null;
            token_type?: string | null;
            expires_in?: number; // int32
        }
        export interface UpdateAgreementDeliveryRequest {
            deliveryDays?: DayOfWeek /* int32 */[] | null;
            limitOrderHourOffset?: number; // int32
        }
        export interface UpdateBatchRequest {
            number?: string | null;
            dateKind?: BatchDateKind /* int32 */;
            date?: DateOnly;
        }
        export interface UpdateDraftProductsRequest {
            products?: ProductQuantityDto[] | null;
        }
        export interface UpdateProductRequest {
            name?: string | null;
            code?: string | null;
            unitPrice?: number; // double
            vat?: number; // double
            description?: string | null;
            returnableId?: string | null;
        }
        export interface UpdateReturnableRequest {
            name?: string | null;
            code?: string | null;
            unitPrice?: number; // double
            vat?: number; // double
        }
    }
}
declare namespace Paths {
    namespace AcceptCustomerAgreement {
        namespace Parameters {
            export type Id = string;
        }
        export interface PathParameters {
            id: Parameters.Id;
        }
        export type RequestBody = Components.Schemas.AcceptCustomerAgreementRequest;
        namespace Responses {
            export interface $204 {
            }
            export type $400 = Components.Schemas.ProblemDetails;
        }
    }
    namespace AcceptOrder {
        namespace Parameters {
            export type Id = string;
        }
        export interface PathParameters {
            id: Parameters.Id;
        }
        export type RequestBody = Components.Schemas.AcceptOrderRequest;
        namespace Responses {
            export interface $204 {
            }
            export type $400 = Components.Schemas.ProblemDetails;
        }
    }
    namespace AcceptSupplierAgreement {
        namespace Parameters {
            export type Id = string;
        }
        export interface PathParameters {
            id: Parameters.Id;
        }
        namespace Responses {
            export interface $204 {
            }
            export type $400 = Components.Schemas.ProblemDetails;
        }
    }
    namespace CancelAgreement {
        namespace Parameters {
            export type Id = string;
        }
        export interface PathParameters {
            id: Parameters.Id;
        }
        namespace Responses {
            export interface $204 {
            }
            export type $400 = Components.Schemas.ProblemDetails;
        }
    }
    namespace CancelInvoice {
        namespace Parameters {
            export type Id = string;
        }
        export interface PathParameters {
            id: Parameters.Id;
        }
        export type RequestBody = Components.Schemas.CancelInvoiceRequest;
        namespace Responses {
            export type $200 = string;
            export type $400 = Components.Schemas.ProblemDetails;
        }
    }
    namespace CancelOrder {
        namespace Parameters {
            export type Id = string;
        }
        export interface PathParameters {
            id: Parameters.Id;
        }
        export type RequestBody = Components.Schemas.CancelOrderRequest;
        namespace Responses {
            export interface $204 {
            }
            export type $400 = Components.Schemas.ProblemDetails;
        }
    }
    namespace ConfigureAccountAsCustomer {
        export type RequestBody = Components.Schemas.CustomerInfoRequest;
        namespace Responses {
            export type $200 = string;
            export type $400 = Components.Schemas.ProblemDetails;
        }
    }
    namespace ConfigureAccountAsSupplier {
        export type RequestBody = Components.Schemas.SupplierInfoRequest;
        namespace Responses {
            export type $200 = string;
            export type $400 = Components.Schemas.ProblemDetails;
        }
    }
    namespace CreateBatch {
        namespace Parameters {
            export type SupplierId = string;
        }
        export interface PathParameters {
            supplierId: Parameters.SupplierId;
        }
        export type RequestBody = Components.Schemas.CreateBatchRequest;
        namespace Responses {
            export type $201 = string;
            export type $400 = Components.Schemas.ProblemDetails;
        }
    }
    namespace CreateCreditNoteDraft {
        namespace Parameters {
            export type Id = string;
        }
        export interface PathParameters {
            id: Parameters.Id;
        }
        namespace Responses {
            export type $201 = string;
            export type $400 = Components.Schemas.ProblemDetails;
        }
    }
    namespace CreateInvoiceForDelivery {
        namespace Parameters {
            export type Id = string;
        }
        export interface PathParameters {
            id: Parameters.Id;
        }
        namespace Responses {
            export type $201 = string;
            export type $400 = Components.Schemas.ProblemDetails;
        }
    }
    namespace CreateOrderDraft {
        namespace Parameters {
            export type SupplierId = string;
        }
        export interface PathParameters {
            supplierId: Parameters.SupplierId;
        }
        namespace Responses {
            export type $201 = string;
            export type $400 = Components.Schemas.ProblemDetails;
        }
    }
    namespace CreatePreparationDocument {
        namespace Parameters {
            export type SupplierId = string;
        }
        export interface PathParameters {
            supplierId: Parameters.SupplierId;
        }
        export type RequestBody = Components.Schemas.CreatePreparationDocumentRequest;
        namespace Responses {
            export type $201 = string;
            export type $400 = Components.Schemas.ProblemDetails;
        }
    }
    namespace CreateProduct {
        namespace Parameters {
            export type SupplierId = string;
        }
        export interface PathParameters {
            supplierId: Parameters.SupplierId;
        }
        export type RequestBody = Components.Schemas.CreateProductRequest;
        namespace Responses {
            export type $201 = string;
            export type $400 = Components.Schemas.ProblemDetails;
        }
    }
    namespace CreateReturnable {
        namespace Parameters {
            export type SupplierId = string;
        }
        export interface PathParameters {
            supplierId: Parameters.SupplierId;
        }
        export type RequestBody = Components.Schemas.CreateReturnableRequest;
        namespace Responses {
            export type $201 = string;
            export type $400 = Components.Schemas.ProblemDetails;
        }
    }
    namespace DeleteBatch {
        namespace Parameters {
            export type BatchId = string;
            export type SupplierId = string;
        }
        export interface PathParameters {
            supplierId: Parameters.SupplierId;
            batchId: Parameters.BatchId;
        }
        namespace Responses {
            export interface $204 {
            }
            export type $400 = Components.Schemas.ProblemDetails;
        }
    }
    namespace DeleteProduct {
        namespace Parameters {
            export type ProductId = string;
            export type SupplierId = string;
        }
        export interface PathParameters {
            supplierId: Parameters.SupplierId;
            productId: Parameters.ProductId;
        }
        namespace Responses {
            export interface $204 {
            }
            export type $400 = Components.Schemas.ProblemDetails;
        }
    }
    namespace DeleteReturnable {
        namespace Parameters {
            export type ReturnableId = string;
            export type SupplierId = string;
        }
        export interface PathParameters {
            supplierId: Parameters.SupplierId;
            returnableId: Parameters.ReturnableId;
        }
        namespace Responses {
            export interface $204 {
            }
            export type $400 = Components.Schemas.ProblemDetails;
        }
    }
    namespace DeliverOrder {
        namespace Parameters {
            export type Id = string;
        }
        export interface PathParameters {
            id: Parameters.Id;
        }
        export type RequestBody = Components.Schemas.DeliverOrdersRequest;
        namespace Responses {
            export interface $204 {
            }
            export type $400 = Components.Schemas.ProblemDetails;
        }
    }
    namespace DownloadDocument {
        namespace Parameters {
            export type Id = string;
        }
        export interface PathParameters {
            id: Parameters.Id;
        }
        namespace Responses {
            export type $200 = string;
            export type $400 = Components.Schemas.ProblemDetails;
        }
    }
    namespace ForgotPassword {
        export type RequestBody = Components.Schemas.ForgotPasswordRequest;
        namespace Responses {
            export interface $204 {
            }
            export type $400 = Components.Schemas.ProblemDetails;
        }
    }
    namespace FulfillOrder {
        namespace Parameters {
            export type Id = string;
        }
        export interface PathParameters {
            id: Parameters.Id;
        }
        export type RequestBody = Components.Schemas.FulfillOrderRequest;
        namespace Responses {
            export interface $204 {
            }
            export type $400 = Components.Schemas.ProblemDetails;
        }
    }
    namespace GetAgreement {
        namespace Parameters {
            export type Id = string;
        }
        export interface PathParameters {
            id: Parameters.Id;
        }
        namespace Responses {
            export type $200 = Components.Schemas.AgreementDetailsDto;
            export type $400 = Components.Schemas.ProblemDetails;
        }
    }
    namespace GetAvailableCustomer {
        namespace Parameters {
            export type Id = string;
        }
        export interface PathParameters {
            id: Parameters.Id;
        }
        namespace Responses {
            export type $200 = Components.Schemas.AvailableCustomerDto;
            export type $400 = Components.Schemas.ProblemDetails;
        }
    }
    namespace GetAvailableSupplier {
        namespace Parameters {
            export type Id = string;
        }
        export interface PathParameters {
            id: Parameters.Id;
        }
        namespace Responses {
            export type $200 = Components.Schemas.AvailableSupplierDto;
            export type $400 = Components.Schemas.ProblemDetails;
        }
    }
    namespace GetNextSupplierDeliveryDates {
        namespace Parameters {
            export type SupplierId = string;
        }
        export interface PathParameters {
            supplierId: Parameters.SupplierId;
        }
        namespace Responses {
            export type $200 = string /* date-time */[];
            export type $400 = Components.Schemas.ProblemDetails;
        }
    }
    namespace GetOrder {
        namespace Parameters {
            export type OrderId = string;
        }
        export interface PathParameters {
            orderId: Parameters.OrderId;
        }
        namespace Responses {
            export type $200 = Components.Schemas.OrderDetailsDto;
            export type $400 = Components.Schemas.ProblemDetails;
        }
    }
    namespace GetOrderDraft {
        namespace Parameters {
            export type OrderId = string;
        }
        export interface PathParameters {
            orderId: Parameters.OrderId;
        }
        namespace Responses {
            export type $200 = Components.Schemas.OrderDraftDto;
            export type $400 = Components.Schemas.ProblemDetails;
        }
    }
    namespace GetProduct {
        namespace Parameters {
            export type ProductId = string;
            export type SupplierId = string;
        }
        export interface PathParameters {
            supplierId: Parameters.SupplierId;
            productId: Parameters.ProductId;
        }
        namespace Responses {
            export type $200 = Components.Schemas.ProductDetailsDto;
            export type $400 = Components.Schemas.ProblemDetails;
        }
    }
    namespace GetReturnable {
        namespace Parameters {
            export type ReturnableId = string;
            export type SupplierId = string;
        }
        export interface PathParameters {
            supplierId: Parameters.SupplierId;
            returnableId: Parameters.ReturnableId;
        }
        namespace Responses {
            export type $200 = Components.Schemas.ReturnableDto;
            export type $400 = Components.Schemas.ProblemDetails;
        }
    }
    namespace ListActiveAgreements {
        namespace Parameters {
            export type Page = number; // int32
            export type Search = string;
            export type Take = number; // int32
        }
        export interface QueryParameters {
            search?: Parameters.Search;
            page?: Parameters.Page /* int32 */;
            take?: Parameters.Take /* int32 */;
        }
        namespace Responses {
            export type $200 = Components.Schemas.AgreementDtoPaginatedResults;
            export type $400 = Components.Schemas.ProblemDetails;
        }
    }
    namespace ListAvailableCustomers {
        namespace Parameters {
            export type Page = number; // int32
            export type Search = string;
            export type Take = number; // int32
        }
        export interface QueryParameters {
            search?: Parameters.Search;
            page?: Parameters.Page /* int32 */;
            take?: Parameters.Take /* int32 */;
        }
        namespace Responses {
            export type $200 = Components.Schemas.AvailableCustomerDtoPaginatedResults;
            export type $400 = Components.Schemas.ProblemDetails;
        }
    }
    namespace ListAvailableSuppliers {
        namespace Parameters {
            export type Page = number; // int32
            export type Search = string;
            export type Take = number; // int32
        }
        export interface QueryParameters {
            search?: Parameters.Search;
            page?: Parameters.Page /* int32 */;
            take?: Parameters.Take /* int32 */;
        }
        namespace Responses {
            export type $200 = Components.Schemas.AvailableSupplierDtoPaginatedResults;
            export type $400 = Components.Schemas.ProblemDetails;
        }
    }
    namespace ListOrderableProducts {
        namespace Parameters {
            export type Page = number; // int32
            export type SupplierId = string;
            export type Take = number; // int32
        }
        export interface PathParameters {
            supplierId: Parameters.SupplierId;
        }
        export interface QueryParameters {
            page?: Parameters.Page /* int32 */;
            take?: Parameters.Take /* int32 */;
        }
        namespace Responses {
            export type $200 = Components.Schemas.OrderableProductDtoPaginatedResults;
            export type $400 = Components.Schemas.ProblemDetails;
        }
    }
    namespace ListOrders {
        namespace Parameters {
            export type Page = number; // int32
            export type Statuses = Components.Schemas.OrderStatus /* int32 */[];
            export type Take = number; // int32
        }
        export interface QueryParameters {
            statuses?: Parameters.Statuses;
            page?: Parameters.Page /* int32 */;
            take?: Parameters.Take /* int32 */;
        }
        namespace Responses {
            export type $200 = Components.Schemas.OrderDtoPaginatedResults;
            export type $400 = Components.Schemas.ProblemDetails;
        }
    }
    namespace ListProducts {
        namespace Parameters {
            export type Page = number; // int32
            export type SupplierId = string;
            export type Take = number; // int32
        }
        export interface PathParameters {
            supplierId: Parameters.SupplierId;
        }
        export interface QueryParameters {
            page?: Parameters.Page /* int32 */;
            take?: Parameters.Take /* int32 */;
        }
        namespace Responses {
            export type $200 = Components.Schemas.ProductDtoPaginatedResults;
            export type $400 = Components.Schemas.ProblemDetails;
        }
    }
    namespace ListReceivedAgreements {
        namespace Parameters {
            export type Page = number; // int32
            export type Search = string;
            export type Take = number; // int32
        }
        export interface QueryParameters {
            search?: Parameters.Search;
            page?: Parameters.Page /* int32 */;
            take?: Parameters.Take /* int32 */;
        }
        namespace Responses {
            export type $200 = Components.Schemas.AgreementDtoPaginatedResults;
            export type $400 = Components.Schemas.ProblemDetails;
        }
    }
    namespace ListReturnables {
        namespace Parameters {
            export type Page = number; // int32
            export type SupplierId = string;
            export type Take = number; // int32
        }
        export interface PathParameters {
            supplierId: Parameters.SupplierId;
        }
        export interface QueryParameters {
            page?: Parameters.Page /* int32 */;
            take?: Parameters.Take /* int32 */;
        }
        namespace Responses {
            export type $200 = Components.Schemas.ReturnableDtoPaginatedResults;
            export type $400 = Components.Schemas.ProblemDetails;
        }
    }
    namespace ListSentAgreements {
        namespace Parameters {
            export type Page = number; // int32
            export type Search = string;
            export type Take = number; // int32
        }
        export interface QueryParameters {
            search?: Parameters.Search;
            page?: Parameters.Page /* int32 */;
            take?: Parameters.Take /* int32 */;
        }
        namespace Responses {
            export type $200 = Components.Schemas.AgreementDtoPaginatedResults;
            export type $400 = Components.Schemas.ProblemDetails;
        }
    }
    namespace LoginUser {
        export type RequestBody = Components.Schemas.LoginRequest;
        namespace Responses {
            export type $200 = Components.Schemas.TokenResponse;
            export type $400 = Components.Schemas.ProblemDetails;
        }
    }
    namespace MarkInvoiceAsPayed {
        namespace Parameters {
            export type Id = string;
        }
        export interface PathParameters {
            id: Parameters.Id;
        }
        export type RequestBody = Components.Schemas.MarkInvoiceAsPayedRequest;
        namespace Responses {
            export interface $204 {
            }
            export type $400 = Components.Schemas.ProblemDetails;
        }
    }
    namespace ProposeAgreementToCustomer {
        namespace Parameters {
            export type Id = string;
        }
        export interface PathParameters {
            id: Parameters.Id;
        }
        export type RequestBody = Components.Schemas.ProposeAgreementToCustomerRequest;
        namespace Responses {
            export type $201 = string;
            export type $400 = Components.Schemas.ProblemDetails;
        }
    }
    namespace ProposeAgreementToSupplier {
        namespace Parameters {
            export type SupplierId = string;
        }
        export interface PathParameters {
            supplierId: Parameters.SupplierId;
        }
        namespace Responses {
            export type $201 = string;
            export type $400 = Components.Schemas.ProblemDetails;
        }
    }
    namespace PublishInvoiceDraft {
        namespace Parameters {
            export type Id = string;
        }
        export interface PathParameters {
            id: Parameters.Id;
        }
        namespace Responses {
            export interface $204 {
            }
            export type $400 = Components.Schemas.ProblemDetails;
        }
    }
    namespace PublishOrderDraft {
        namespace Parameters {
            export type Id = string;
        }
        export interface PathParameters {
            id: Parameters.Id;
        }
        export type RequestBody = Components.Schemas.PublishOrderDraftRequest;
        namespace Responses {
            export interface $204 {
            }
            export type $400 = Components.Schemas.ProblemDetails;
        }
    }
    namespace RefreshAccessToken {
        export type RequestBody = Components.Schemas.RefreshTokenRequest;
        namespace Responses {
            export type $200 = Components.Schemas.TokenResponse;
            export type $400 = Components.Schemas.ProblemDetails;
        }
    }
    namespace RefuseAgreement {
        namespace Parameters {
            export type Id = string;
        }
        export interface PathParameters {
            id: Parameters.Id;
        }
        export type RequestBody = Components.Schemas.RefuseAgreementRequest;
        namespace Responses {
            export interface $204 {
            }
            export type $400 = Components.Schemas.ProblemDetails;
        }
    }
    namespace RefuseOrder {
        namespace Parameters {
            export type Id = string;
        }
        export interface PathParameters {
            id: Parameters.Id;
        }
        export type RequestBody = Components.Schemas.RefuseOrderRequest;
        namespace Responses {
            export interface $204 {
            }
            export type $400 = Components.Schemas.ProblemDetails;
        }
    }
    namespace RegisterAccount {
        export type RequestBody = Components.Schemas.RegisterRequest;
        namespace Responses {
            export type $201 = string;
            export type $400 = Components.Schemas.ProblemDetails;
        }
    }
    namespace RemoveDocument {
        namespace Parameters {
            export type Id = string;
        }
        export interface PathParameters {
            id: Parameters.Id;
        }
        namespace Responses {
            export interface $204 {
            }
            export type $400 = Components.Schemas.ProblemDetails;
        }
    }
    namespace RemoveInvoiceDraft {
        namespace Parameters {
            export type Id = string;
        }
        export interface PathParameters {
            id: Parameters.Id;
        }
        namespace Responses {
            export interface $204 {
            }
            export type $400 = Components.Schemas.ProblemDetails;
        }
    }
    namespace ResetPassword {
        export type RequestBody = Components.Schemas.ResetPasswordRequest;
        namespace Responses {
            export interface $204 {
            }
            export type $400 = Components.Schemas.ProblemDetails;
        }
    }
    namespace RevokeAgreement {
        namespace Parameters {
            export type Id = string;
        }
        export interface PathParameters {
            id: Parameters.Id;
        }
        export type RequestBody = Components.Schemas.RevokeAgreementRequest;
        namespace Responses {
            export interface $204 {
            }
            export type $400 = Components.Schemas.ProblemDetails;
        }
    }
    namespace SendInvoice {
        namespace Parameters {
            export type Id = string;
        }
        export interface PathParameters {
            id: Parameters.Id;
        }
        namespace Responses {
            export interface $204 {
            }
            export type $400 = Components.Schemas.ProblemDetails;
        }
    }
    namespace UpdateAgreementDelivery {
        namespace Parameters {
            export type Id = string;
        }
        export interface PathParameters {
            id: Parameters.Id;
        }
        export type RequestBody = Components.Schemas.UpdateAgreementDeliveryRequest;
        namespace Responses {
            export interface $204 {
            }
            export type $400 = Components.Schemas.ProblemDetails;
        }
    }
    namespace UpdateBatch {
        namespace Parameters {
            export type BatchId = string;
            export type SupplierId = string;
        }
        export interface PathParameters {
            supplierId: Parameters.SupplierId;
            batchId: Parameters.BatchId;
        }
        export type RequestBody = Components.Schemas.UpdateBatchRequest;
        namespace Responses {
            export interface $200 {
            }
            export type $400 = Components.Schemas.ProblemDetails;
        }
    }
    namespace UpdateCustomer {
        namespace Parameters {
            export type Id = string;
        }
        export interface PathParameters {
            id: Parameters.Id;
        }
        export type RequestBody = Components.Schemas.CustomerInfoRequest;
        namespace Responses {
            export interface $200 {
            }
            export type $400 = Components.Schemas.ProblemDetails;
        }
    }
    namespace UpdateOrderDraftProducts {
        namespace Parameters {
            export type Id = string;
        }
        export interface PathParameters {
            id: Parameters.Id;
        }
        export type RequestBody = Components.Schemas.UpdateDraftProductsRequest;
        namespace Responses {
            export interface $204 {
            }
            export type $400 = Components.Schemas.ProblemDetails;
        }
    }
    namespace UpdateProduct {
        namespace Parameters {
            export type ProductId = string;
            export type SupplierId = string;
        }
        export interface PathParameters {
            supplierId: Parameters.SupplierId;
            productId: Parameters.ProductId;
        }
        export type RequestBody = Components.Schemas.UpdateProductRequest;
        namespace Responses {
            export interface $204 {
            }
            export type $400 = Components.Schemas.ProblemDetails;
        }
    }
    namespace UpdateReturnable {
        namespace Parameters {
            export type ReturnableId = string;
            export type SupplierId = string;
        }
        export interface PathParameters {
            supplierId: Parameters.SupplierId;
            returnableId: Parameters.ReturnableId;
        }
        export type RequestBody = Components.Schemas.UpdateReturnableRequest;
        namespace Responses {
            export interface $204 {
            }
            export type $400 = Components.Schemas.ProblemDetails;
        }
    }
    namespace UpdateSupplier {
        namespace Parameters {
            export type Id = string;
        }
        export interface PathParameters {
            id: Parameters.Id;
        }
        export type RequestBody = Components.Schemas.SupplierInfoRequest;
        namespace Responses {
            export interface $200 {
            }
            export type $400 = Components.Schemas.ProblemDetails;
        }
    }
}

export interface OperationMethods {
  /**
   * AcceptCustomerAgreement - Accept customer agreement
   */
  'AcceptCustomerAgreement'(
    parameters?: Parameters<Paths.AcceptCustomerAgreement.PathParameters> | null,
    data?: Paths.AcceptCustomerAgreement.RequestBody,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.AcceptCustomerAgreement.Responses.$204>
  /**
   * AcceptOrder - Accept order with id
   */
  'AcceptOrder'(
    parameters?: Parameters<Paths.AcceptOrder.PathParameters> | null,
    data?: Paths.AcceptOrder.RequestBody,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.AcceptOrder.Responses.$204>
  /**
   * AcceptSupplierAgreement - Accept supplier agreement
   */
  'AcceptSupplierAgreement'(
    parameters?: Parameters<Paths.AcceptSupplierAgreement.PathParameters> | null,
    data?: any,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.AcceptSupplierAgreement.Responses.$204>
  /**
   * CancelAgreement - Cancel agreement
   */
  'CancelAgreement'(
    parameters?: Parameters<Paths.CancelAgreement.PathParameters> | null,
    data?: any,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.CancelAgreement.Responses.$204>
  /**
   * CancelInvoice - Cancel invoice with id
   */
  'CancelInvoice'(
    parameters?: Parameters<Paths.CancelInvoice.PathParameters> | null,
    data?: Paths.CancelInvoice.RequestBody,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.CancelInvoice.Responses.$200>
  /**
   * CancelOrder - Cancel order with id
   */
  'CancelOrder'(
    parameters?: Parameters<Paths.CancelOrder.PathParameters> | null,
    data?: Paths.CancelOrder.RequestBody,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.CancelOrder.Responses.$204>
  /**
   * ConfigureAccountAsCustomer - Configure current user account as customer profile
   */
  'ConfigureAccountAsCustomer'(
    parameters?: Parameters<UnknownParamsObject> | null,
    data?: Paths.ConfigureAccountAsCustomer.RequestBody,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.ConfigureAccountAsCustomer.Responses.$200>
  /**
   * ConfigureAccountAsSupplier - Configure user account as supplier profile
   */
  'ConfigureAccountAsSupplier'(
    parameters?: Parameters<UnknownParamsObject> | null,
    data?: Paths.ConfigureAccountAsSupplier.RequestBody,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.ConfigureAccountAsSupplier.Responses.$200>
  /**
   * CreateBatch - Create a batch with specified info
   */
  'CreateBatch'(
    parameters?: Parameters<Paths.CreateBatch.PathParameters> | null,
    data?: Paths.CreateBatch.RequestBody,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.CreateBatch.Responses.$201>
  /**
   * CreateCreditNoteDraft - Create a credit note for invoice with id
   */
  'CreateCreditNoteDraft'(
    parameters?: Parameters<Paths.CreateCreditNoteDraft.PathParameters> | null,
    data?: any,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.CreateCreditNoteDraft.Responses.$201>
  /**
   * CreateInvoiceForDelivery - Create an invoice for delivery with id
   */
  'CreateInvoiceForDelivery'(
    parameters?: Parameters<Paths.CreateInvoiceForDelivery.PathParameters> | null,
    data?: any,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.CreateInvoiceForDelivery.Responses.$201>
  /**
   * CreateOrderDraft - Create a new order draft
   */
  'CreateOrderDraft'(
    parameters?: Parameters<Paths.CreateOrderDraft.PathParameters> | null,
    data?: any,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.CreateOrderDraft.Responses.$201>
  /**
   * CreatePreparationDocument - Create a preparation document for specified orders
   */
  'CreatePreparationDocument'(
    parameters?: Parameters<Paths.CreatePreparationDocument.PathParameters> | null,
    data?: Paths.CreatePreparationDocument.RequestBody,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.CreatePreparationDocument.Responses.$201>
  /**
   * ListProducts - List available products for supplier
   */
  'ListProducts'(
    parameters?: Parameters<Paths.ListProducts.PathParameters & Paths.ListProducts.QueryParameters> | null,
    data?: any,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.ListProducts.Responses.$200>
  /**
   * CreateProduct - Create a product
   */
  'CreateProduct'(
    parameters?: Parameters<Paths.CreateProduct.PathParameters> | null,
    data?: Paths.CreateProduct.RequestBody,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.CreateProduct.Responses.$201>
  /**
   * ListReturnables - List available returnables for supplier
   */
  'ListReturnables'(
    parameters?: Parameters<Paths.ListReturnables.PathParameters & Paths.ListReturnables.QueryParameters> | null,
    data?: any,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.ListReturnables.Responses.$200>
  /**
   * CreateReturnable - Create a returnable to be used with products
   */
  'CreateReturnable'(
    parameters?: Parameters<Paths.CreateReturnable.PathParameters> | null,
    data?: Paths.CreateReturnable.RequestBody,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.CreateReturnable.Responses.$201>
  /**
   * UpdateBatch - Update a batch
   */
  'UpdateBatch'(
    parameters?: Parameters<Paths.UpdateBatch.PathParameters> | null,
    data?: Paths.UpdateBatch.RequestBody,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.UpdateBatch.Responses.$200>
  /**
   * DeleteBatch - Remove a batch
   */
  'DeleteBatch'(
    parameters?: Parameters<Paths.DeleteBatch.PathParameters> | null,
    data?: any,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.DeleteBatch.Responses.$204>
  /**
   * GetProduct - Retrieve product with id
   */
  'GetProduct'(
    parameters?: Parameters<Paths.GetProduct.PathParameters> | null,
    data?: any,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.GetProduct.Responses.$200>
  /**
   * UpdateProduct - Update product with id info
   */
  'UpdateProduct'(
    parameters?: Parameters<Paths.UpdateProduct.PathParameters> | null,
    data?: Paths.UpdateProduct.RequestBody,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.UpdateProduct.Responses.$204>
  /**
   * DeleteProduct - Remove product with id
   */
  'DeleteProduct'(
    parameters?: Parameters<Paths.DeleteProduct.PathParameters> | null,
    data?: any,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.DeleteProduct.Responses.$204>
  /**
   * GetReturnable - Retrieve returnable with id
   */
  'GetReturnable'(
    parameters?: Parameters<Paths.GetReturnable.PathParameters> | null,
    data?: any,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.GetReturnable.Responses.$200>
  /**
   * UpdateReturnable - Remove returnable with id info
   */
  'UpdateReturnable'(
    parameters?: Parameters<Paths.UpdateReturnable.PathParameters> | null,
    data?: Paths.UpdateReturnable.RequestBody,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.UpdateReturnable.Responses.$204>
  /**
   * DeleteReturnable - Remove returnable with id
   */
  'DeleteReturnable'(
    parameters?: Parameters<Paths.DeleteReturnable.PathParameters> | null,
    data?: any,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.DeleteReturnable.Responses.$204>
  /**
   * DeliverOrder - Delivery order with id
   */
  'DeliverOrder'(
    parameters?: Parameters<Paths.DeliverOrder.PathParameters> | null,
    data?: Paths.DeliverOrder.RequestBody,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.DeliverOrder.Responses.$204>
  /**
   * DownloadDocument - Generate a secured download link for document
   */
  'DownloadDocument'(
    parameters?: Parameters<Paths.DownloadDocument.PathParameters> | null,
    data?: any,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.DownloadDocument.Responses.$200>
  /**
   * RemoveDocument - Remove document with id
   */
  'RemoveDocument'(
    parameters?: Parameters<Paths.RemoveDocument.PathParameters> | null,
    data?: any,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.RemoveDocument.Responses.$204>
  /**
   * ForgotPassword - Generate and send a reset link on user email
   */
  'ForgotPassword'(
    parameters?: Parameters<UnknownParamsObject> | null,
    data?: Paths.ForgotPassword.RequestBody,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.ForgotPassword.Responses.$204>
  /**
   * FulfillOrder - Complete order with id
   */
  'FulfillOrder'(
    parameters?: Parameters<Paths.FulfillOrder.PathParameters> | null,
    data?: Paths.FulfillOrder.RequestBody,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.FulfillOrder.Responses.$204>
  /**
   * GetAgreement - Retrieve agreement with id
   */
  'GetAgreement'(
    parameters?: Parameters<Paths.GetAgreement.PathParameters> | null,
    data?: any,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.GetAgreement.Responses.$200>
  /**
   * UpdateAgreementDelivery - update agreement delivery
   */
  'UpdateAgreementDelivery'(
    parameters?: Parameters<Paths.UpdateAgreementDelivery.PathParameters> | null,
    data?: Paths.UpdateAgreementDelivery.RequestBody,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.UpdateAgreementDelivery.Responses.$204>
  /**
   * GetAvailableCustomer - Retrieve customer with id
   */
  'GetAvailableCustomer'(
    parameters?: Parameters<Paths.GetAvailableCustomer.PathParameters> | null,
    data?: any,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.GetAvailableCustomer.Responses.$200>
  /**
   * UpdateCustomer - Update current user customer profile
   */
  'UpdateCustomer'(
    parameters?: Parameters<Paths.UpdateCustomer.PathParameters> | null,
    data?: Paths.UpdateCustomer.RequestBody,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.UpdateCustomer.Responses.$200>
  /**
   * GetAvailableSupplier - Retrieve supplier with id
   */
  'GetAvailableSupplier'(
    parameters?: Parameters<Paths.GetAvailableSupplier.PathParameters> | null,
    data?: any,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.GetAvailableSupplier.Responses.$200>
  /**
   * UpdateSupplier - Update current user supplier profile
   */
  'UpdateSupplier'(
    parameters?: Parameters<Paths.UpdateSupplier.PathParameters> | null,
    data?: Paths.UpdateSupplier.RequestBody,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.UpdateSupplier.Responses.$200>
  /**
   * GetNextSupplierDeliveryDates - Get supplier next delivery dates for current user
   */
  'GetNextSupplierDeliveryDates'(
    parameters?: Parameters<Paths.GetNextSupplierDeliveryDates.PathParameters> | null,
    data?: any,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.GetNextSupplierDeliveryDates.Responses.$200>
  /**
   * GetOrder - Retrieve order with id
   */
  'GetOrder'(
    parameters?: Parameters<Paths.GetOrder.PathParameters> | null,
    data?: any,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.GetOrder.Responses.$200>
  /**
   * GetOrderDraft - Retrieve order draft with id
   */
  'GetOrderDraft'(
    parameters?: Parameters<Paths.GetOrderDraft.PathParameters> | null,
    data?: any,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.GetOrderDraft.Responses.$200>
  /**
   * ListActiveAgreements - List active agreements for current user
   */
  'ListActiveAgreements'(
    parameters?: Parameters<Paths.ListActiveAgreements.QueryParameters> | null,
    data?: any,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.ListActiveAgreements.Responses.$200>
  /**
   * ListAvailableCustomers - List available customers for current user
   */
  'ListAvailableCustomers'(
    parameters?: Parameters<Paths.ListAvailableCustomers.QueryParameters> | null,
    data?: any,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.ListAvailableCustomers.Responses.$200>
  /**
   * ListAvailableSuppliers - List available suppliers for current user
   */
  'ListAvailableSuppliers'(
    parameters?: Parameters<Paths.ListAvailableSuppliers.QueryParameters> | null,
    data?: any,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.ListAvailableSuppliers.Responses.$200>
  /**
   * ListOrderableProducts - List supplier orderable products for current user
   */
  'ListOrderableProducts'(
    parameters?: Parameters<Paths.ListOrderableProducts.PathParameters & Paths.ListOrderableProducts.QueryParameters> | null,
    data?: any,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.ListOrderableProducts.Responses.$200>
  /**
   * ListOrders - List available orders for current user
   */
  'ListOrders'(
    parameters?: Parameters<Paths.ListOrders.QueryParameters> | null,
    data?: any,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.ListOrders.Responses.$200>
  /**
   * ListReceivedAgreements - List received agreements for current user
   */
  'ListReceivedAgreements'(
    parameters?: Parameters<Paths.ListReceivedAgreements.QueryParameters> | null,
    data?: any,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.ListReceivedAgreements.Responses.$200>
  /**
   * ListSentAgreements - List sent agreements for current user
   */
  'ListSentAgreements'(
    parameters?: Parameters<Paths.ListSentAgreements.QueryParameters> | null,
    data?: any,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.ListSentAgreements.Responses.$200>
  /**
   * LoginUser - Log the user in and generate access token / refresh token
   */
  'LoginUser'(
    parameters?: Parameters<UnknownParamsObject> | null,
    data?: Paths.LoginUser.RequestBody,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.LoginUser.Responses.$200>
  /**
   * MarkInvoiceAsPayed - Mark invoice with id as payed
   */
  'MarkInvoiceAsPayed'(
    parameters?: Parameters<Paths.MarkInvoiceAsPayed.PathParameters> | null,
    data?: Paths.MarkInvoiceAsPayed.RequestBody,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.MarkInvoiceAsPayed.Responses.$204>
  /**
   * ProposeAgreementToCustomer - Send an agreement to customer
   */
  'ProposeAgreementToCustomer'(
    parameters?: Parameters<Paths.ProposeAgreementToCustomer.PathParameters> | null,
    data?: Paths.ProposeAgreementToCustomer.RequestBody,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.ProposeAgreementToCustomer.Responses.$201>
  /**
   * ProposeAgreementToSupplier - Send an agreement to supplier
   */
  'ProposeAgreementToSupplier'(
    parameters?: Parameters<Paths.ProposeAgreementToSupplier.PathParameters> | null,
    data?: any,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.ProposeAgreementToSupplier.Responses.$201>
  /**
   * PublishInvoiceDraft - Publish invoice with id
   */
  'PublishInvoiceDraft'(
    parameters?: Parameters<Paths.PublishInvoiceDraft.PathParameters> | null,
    data?: any,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.PublishInvoiceDraft.Responses.$204>
  /**
   * PublishOrderDraft - Publish (and send to supplier) order with id
   */
  'PublishOrderDraft'(
    parameters?: Parameters<Paths.PublishOrderDraft.PathParameters> | null,
    data?: Paths.PublishOrderDraft.RequestBody,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.PublishOrderDraft.Responses.$204>
  /**
   * RefreshAccessToken - Regenerate an access_token for current user
   */
  'RefreshAccessToken'(
    parameters?: Parameters<UnknownParamsObject> | null,
    data?: Paths.RefreshAccessToken.RequestBody,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.RefreshAccessToken.Responses.$200>
  /**
   * RefuseAgreement - Refuse agreement
   */
  'RefuseAgreement'(
    parameters?: Parameters<Paths.RefuseAgreement.PathParameters> | null,
    data?: Paths.RefuseAgreement.RequestBody,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.RefuseAgreement.Responses.$204>
  /**
   * RefuseOrder - Refuse customer order with id
   */
  'RefuseOrder'(
    parameters?: Parameters<Paths.RefuseOrder.PathParameters> | null,
    data?: Paths.RefuseOrder.RequestBody,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.RefuseOrder.Responses.$204>
  /**
   * RegisterAccount - Create an account with specified email/password
   */
  'RegisterAccount'(
    parameters?: Parameters<UnknownParamsObject> | null,
    data?: Paths.RegisterAccount.RequestBody,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.RegisterAccount.Responses.$201>
  /**
   * RemoveInvoiceDraft - Remove invoice draft with id
   */
  'RemoveInvoiceDraft'(
    parameters?: Parameters<Paths.RemoveInvoiceDraft.PathParameters> | null,
    data?: any,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.RemoveInvoiceDraft.Responses.$204>
  /**
   * ResetPassword - Reset the password with the token retrieved from email link
   */
  'ResetPassword'(
    parameters?: Parameters<UnknownParamsObject> | null,
    data?: Paths.ResetPassword.RequestBody,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.ResetPassword.Responses.$204>
  /**
   * RevokeAgreement - Revoke agreement
   */
  'RevokeAgreement'(
    parameters?: Parameters<Paths.RevokeAgreement.PathParameters> | null,
    data?: Paths.RevokeAgreement.RequestBody,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.RevokeAgreement.Responses.$204>
  /**
   * SendInvoice - Send invoice with id to customer
   */
  'SendInvoice'(
    parameters?: Parameters<Paths.SendInvoice.PathParameters> | null,
    data?: any,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.SendInvoice.Responses.$204>
  /**
   * UpdateOrderDraftProducts - Update order with id products
   */
  'UpdateOrderDraftProducts'(
    parameters?: Parameters<Paths.UpdateOrderDraftProducts.PathParameters> | null,
    data?: Paths.UpdateOrderDraftProducts.RequestBody,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.UpdateOrderDraftProducts.Responses.$204>
}

export interface PathsDictionary {
  ['/api/agreements/{id}/accept/customer']: {
    /**
     * AcceptCustomerAgreement - Accept customer agreement
     */
    'put'(
      parameters?: Parameters<Paths.AcceptCustomerAgreement.PathParameters> | null,
      data?: Paths.AcceptCustomerAgreement.RequestBody,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.AcceptCustomerAgreement.Responses.$204>
  }
  ['/api/orders/{id}/accept']: {
    /**
     * AcceptOrder - Accept order with id
     */
    'post'(
      parameters?: Parameters<Paths.AcceptOrder.PathParameters> | null,
      data?: Paths.AcceptOrder.RequestBody,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.AcceptOrder.Responses.$204>
  }
  ['/api/agreements/{id}/accept/supplier']: {
    /**
     * AcceptSupplierAgreement - Accept supplier agreement
     */
    'put'(
      parameters?: Parameters<Paths.AcceptSupplierAgreement.PathParameters> | null,
      data?: any,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.AcceptSupplierAgreement.Responses.$204>
  }
  ['/api/agreements/{id}/cancel']: {
    /**
     * CancelAgreement - Cancel agreement
     */
    'put'(
      parameters?: Parameters<Paths.CancelAgreement.PathParameters> | null,
      data?: any,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.CancelAgreement.Responses.$204>
  }
  ['/api/invoices/{id}/cancel']: {
    /**
     * CancelInvoice - Cancel invoice with id
     */
    'post'(
      parameters?: Parameters<Paths.CancelInvoice.PathParameters> | null,
      data?: Paths.CancelInvoice.RequestBody,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.CancelInvoice.Responses.$200>
  }
  ['/api/orders/{id}/cancel']: {
    /**
     * CancelOrder - Cancel order with id
     */
    'post'(
      parameters?: Parameters<Paths.CancelOrder.PathParameters> | null,
      data?: Paths.CancelOrder.RequestBody,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.CancelOrder.Responses.$204>
  }
  ['/api/account/configure/customer']: {
    /**
     * ConfigureAccountAsCustomer - Configure current user account as customer profile
     */
    'post'(
      parameters?: Parameters<UnknownParamsObject> | null,
      data?: Paths.ConfigureAccountAsCustomer.RequestBody,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.ConfigureAccountAsCustomer.Responses.$200>
  }
  ['/api/account/configure/supplier']: {
    /**
     * ConfigureAccountAsSupplier - Configure user account as supplier profile
     */
    'post'(
      parameters?: Parameters<UnknownParamsObject> | null,
      data?: Paths.ConfigureAccountAsSupplier.RequestBody,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.ConfigureAccountAsSupplier.Responses.$200>
  }
  ['/api/suppliers/{supplierId}/batches']: {
    /**
     * CreateBatch - Create a batch with specified info
     */
    'post'(
      parameters?: Parameters<Paths.CreateBatch.PathParameters> | null,
      data?: Paths.CreateBatch.RequestBody,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.CreateBatch.Responses.$201>
  }
  ['/api/invoices/{id}/credit']: {
    /**
     * CreateCreditNoteDraft - Create a credit note for invoice with id
     */
    'post'(
      parameters?: Parameters<Paths.CreateCreditNoteDraft.PathParameters> | null,
      data?: any,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.CreateCreditNoteDraft.Responses.$201>
  }
  ['/api/deliveries/{id}/invoice']: {
    /**
     * CreateInvoiceForDelivery - Create an invoice for delivery with id
     */
    'post'(
      parameters?: Parameters<Paths.CreateInvoiceForDelivery.PathParameters> | null,
      data?: any,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.CreateInvoiceForDelivery.Responses.$201>
  }
  ['/api/suppliers/{supplierId}/orders']: {
    /**
     * CreateOrderDraft - Create a new order draft
     */
    'post'(
      parameters?: Parameters<Paths.CreateOrderDraft.PathParameters> | null,
      data?: any,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.CreateOrderDraft.Responses.$201>
  }
  ['/api/documents/{supplierId}/preparation']: {
    /**
     * CreatePreparationDocument - Create a preparation document for specified orders
     */
    'post'(
      parameters?: Parameters<Paths.CreatePreparationDocument.PathParameters> | null,
      data?: Paths.CreatePreparationDocument.RequestBody,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.CreatePreparationDocument.Responses.$201>
  }
  ['/api/suppliers/{supplierId}/products']: {
    /**
     * CreateProduct - Create a product
     */
    'post'(
      parameters?: Parameters<Paths.CreateProduct.PathParameters> | null,
      data?: Paths.CreateProduct.RequestBody,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.CreateProduct.Responses.$201>
    /**
     * ListProducts - List available products for supplier
     */
    'get'(
      parameters?: Parameters<Paths.ListProducts.PathParameters & Paths.ListProducts.QueryParameters> | null,
      data?: any,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.ListProducts.Responses.$200>
  }
  ['/api/suppliers/{supplierId}/returnables']: {
    /**
     * CreateReturnable - Create a returnable to be used with products
     */
    'post'(
      parameters?: Parameters<Paths.CreateReturnable.PathParameters> | null,
      data?: Paths.CreateReturnable.RequestBody,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.CreateReturnable.Responses.$201>
    /**
     * ListReturnables - List available returnables for supplier
     */
    'get'(
      parameters?: Parameters<Paths.ListReturnables.PathParameters & Paths.ListReturnables.QueryParameters> | null,
      data?: any,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.ListReturnables.Responses.$200>
  }
  ['/api/suppliers/{supplierId}/batches/{batchId}']: {
    /**
     * DeleteBatch - Remove a batch
     */
    'delete'(
      parameters?: Parameters<Paths.DeleteBatch.PathParameters> | null,
      data?: any,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.DeleteBatch.Responses.$204>
    /**
     * UpdateBatch - Update a batch
     */
    'put'(
      parameters?: Parameters<Paths.UpdateBatch.PathParameters> | null,
      data?: Paths.UpdateBatch.RequestBody,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.UpdateBatch.Responses.$200>
  }
  ['/api/suppliers/{supplierId}/products/{productId}']: {
    /**
     * DeleteProduct - Remove product with id
     */
    'delete'(
      parameters?: Parameters<Paths.DeleteProduct.PathParameters> | null,
      data?: any,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.DeleteProduct.Responses.$204>
    /**
     * GetProduct - Retrieve product with id
     */
    'get'(
      parameters?: Parameters<Paths.GetProduct.PathParameters> | null,
      data?: any,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.GetProduct.Responses.$200>
    /**
     * UpdateProduct - Update product with id info
     */
    'put'(
      parameters?: Parameters<Paths.UpdateProduct.PathParameters> | null,
      data?: Paths.UpdateProduct.RequestBody,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.UpdateProduct.Responses.$204>
  }
  ['/api/suppliers/{supplierId}/returnables/{returnableId}']: {
    /**
     * DeleteReturnable - Remove returnable with id
     */
    'delete'(
      parameters?: Parameters<Paths.DeleteReturnable.PathParameters> | null,
      data?: any,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.DeleteReturnable.Responses.$204>
    /**
     * GetReturnable - Retrieve returnable with id
     */
    'get'(
      parameters?: Parameters<Paths.GetReturnable.PathParameters> | null,
      data?: any,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.GetReturnable.Responses.$200>
    /**
     * UpdateReturnable - Remove returnable with id info
     */
    'put'(
      parameters?: Parameters<Paths.UpdateReturnable.PathParameters> | null,
      data?: Paths.UpdateReturnable.RequestBody,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.UpdateReturnable.Responses.$204>
  }
  ['/api/deliveries/{id}/deliver']: {
    /**
     * DeliverOrder - Delivery order with id
     */
    'post'(
      parameters?: Parameters<Paths.DeliverOrder.PathParameters> | null,
      data?: Paths.DeliverOrder.RequestBody,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.DeliverOrder.Responses.$204>
  }
  ['/api/documents/{id}']: {
    /**
     * DownloadDocument - Generate a secured download link for document
     */
    'post'(
      parameters?: Parameters<Paths.DownloadDocument.PathParameters> | null,
      data?: any,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.DownloadDocument.Responses.$200>
    /**
     * RemoveDocument - Remove document with id
     */
    'delete'(
      parameters?: Parameters<Paths.RemoveDocument.PathParameters> | null,
      data?: any,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.RemoveDocument.Responses.$204>
  }
  ['/api/password/forgot']: {
    /**
     * ForgotPassword - Generate and send a reset link on user email
     */
    'post'(
      parameters?: Parameters<UnknownParamsObject> | null,
      data?: Paths.ForgotPassword.RequestBody,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.ForgotPassword.Responses.$204>
  }
  ['/api/orders/{id}/fulfill']: {
    /**
     * FulfillOrder - Complete order with id
     */
    'post'(
      parameters?: Parameters<Paths.FulfillOrder.PathParameters> | null,
      data?: Paths.FulfillOrder.RequestBody,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.FulfillOrder.Responses.$204>
  }
  ['/api/agreements/{id}']: {
    /**
     * GetAgreement - Retrieve agreement with id
     */
    'get'(
      parameters?: Parameters<Paths.GetAgreement.PathParameters> | null,
      data?: any,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.GetAgreement.Responses.$200>
    /**
     * UpdateAgreementDelivery - update agreement delivery
     */
    'put'(
      parameters?: Parameters<Paths.UpdateAgreementDelivery.PathParameters> | null,
      data?: Paths.UpdateAgreementDelivery.RequestBody,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.UpdateAgreementDelivery.Responses.$204>
  }
  ['/api/customers/{id}']: {
    /**
     * GetAvailableCustomer - Retrieve customer with id
     */
    'get'(
      parameters?: Parameters<Paths.GetAvailableCustomer.PathParameters> | null,
      data?: any,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.GetAvailableCustomer.Responses.$200>
    /**
     * UpdateCustomer - Update current user customer profile
     */
    'put'(
      parameters?: Parameters<Paths.UpdateCustomer.PathParameters> | null,
      data?: Paths.UpdateCustomer.RequestBody,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.UpdateCustomer.Responses.$200>
  }
  ['/api/suppliers/{id}']: {
    /**
     * GetAvailableSupplier - Retrieve supplier with id
     */
    'get'(
      parameters?: Parameters<Paths.GetAvailableSupplier.PathParameters> | null,
      data?: any,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.GetAvailableSupplier.Responses.$200>
    /**
     * UpdateSupplier - Update current user supplier profile
     */
    'put'(
      parameters?: Parameters<Paths.UpdateSupplier.PathParameters> | null,
      data?: Paths.UpdateSupplier.RequestBody,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.UpdateSupplier.Responses.$200>
  }
  ['/api/suppliers/{supplierId}/next-delivery-dates']: {
    /**
     * GetNextSupplierDeliveryDates - Get supplier next delivery dates for current user
     */
    'get'(
      parameters?: Parameters<Paths.GetNextSupplierDeliveryDates.PathParameters> | null,
      data?: any,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.GetNextSupplierDeliveryDates.Responses.$200>
  }
  ['/api/orders/{orderId}']: {
    /**
     * GetOrder - Retrieve order with id
     */
    'get'(
      parameters?: Parameters<Paths.GetOrder.PathParameters> | null,
      data?: any,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.GetOrder.Responses.$200>
  }
  ['/api/orders/draft/{orderId}']: {
    /**
     * GetOrderDraft - Retrieve order draft with id
     */
    'get'(
      parameters?: Parameters<Paths.GetOrderDraft.PathParameters> | null,
      data?: any,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.GetOrderDraft.Responses.$200>
  }
  ['/api/agreements']: {
    /**
     * ListActiveAgreements - List active agreements for current user
     */
    'get'(
      parameters?: Parameters<Paths.ListActiveAgreements.QueryParameters> | null,
      data?: any,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.ListActiveAgreements.Responses.$200>
  }
  ['/api/customers']: {
    /**
     * ListAvailableCustomers - List available customers for current user
     */
    'get'(
      parameters?: Parameters<Paths.ListAvailableCustomers.QueryParameters> | null,
      data?: any,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.ListAvailableCustomers.Responses.$200>
  }
  ['/api/suppliers']: {
    /**
     * ListAvailableSuppliers - List available suppliers for current user
     */
    'get'(
      parameters?: Parameters<Paths.ListAvailableSuppliers.QueryParameters> | null,
      data?: any,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.ListAvailableSuppliers.Responses.$200>
  }
  ['/api/suppliers/{supplierId}/products/orderable']: {
    /**
     * ListOrderableProducts - List supplier orderable products for current user
     */
    'get'(
      parameters?: Parameters<Paths.ListOrderableProducts.PathParameters & Paths.ListOrderableProducts.QueryParameters> | null,
      data?: any,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.ListOrderableProducts.Responses.$200>
  }
  ['/api/orders']: {
    /**
     * ListOrders - List available orders for current user
     */
    'get'(
      parameters?: Parameters<Paths.ListOrders.QueryParameters> | null,
      data?: any,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.ListOrders.Responses.$200>
  }
  ['/api/agreements/received']: {
    /**
     * ListReceivedAgreements - List received agreements for current user
     */
    'get'(
      parameters?: Parameters<Paths.ListReceivedAgreements.QueryParameters> | null,
      data?: any,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.ListReceivedAgreements.Responses.$200>
  }
  ['/api/agreements/sent']: {
    /**
     * ListSentAgreements - List sent agreements for current user
     */
    'get'(
      parameters?: Parameters<Paths.ListSentAgreements.QueryParameters> | null,
      data?: any,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.ListSentAgreements.Responses.$200>
  }
  ['/api/token/login']: {
    /**
     * LoginUser - Log the user in and generate access token / refresh token
     */
    'post'(
      parameters?: Parameters<UnknownParamsObject> | null,
      data?: Paths.LoginUser.RequestBody,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.LoginUser.Responses.$200>
  }
  ['/api/invoices/{id}/payed']: {
    /**
     * MarkInvoiceAsPayed - Mark invoice with id as payed
     */
    'post'(
      parameters?: Parameters<Paths.MarkInvoiceAsPayed.PathParameters> | null,
      data?: Paths.MarkInvoiceAsPayed.RequestBody,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.MarkInvoiceAsPayed.Responses.$204>
  }
  ['/api/customers/{id}/agreement']: {
    /**
     * ProposeAgreementToCustomer - Send an agreement to customer
     */
    'post'(
      parameters?: Parameters<Paths.ProposeAgreementToCustomer.PathParameters> | null,
      data?: Paths.ProposeAgreementToCustomer.RequestBody,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.ProposeAgreementToCustomer.Responses.$201>
  }
  ['/api/suppliers/{supplierId}/agreement']: {
    /**
     * ProposeAgreementToSupplier - Send an agreement to supplier
     */
    'post'(
      parameters?: Parameters<Paths.ProposeAgreementToSupplier.PathParameters> | null,
      data?: any,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.ProposeAgreementToSupplier.Responses.$201>
  }
  ['/api/invoices/{id}/publish']: {
    /**
     * PublishInvoiceDraft - Publish invoice with id
     */
    'post'(
      parameters?: Parameters<Paths.PublishInvoiceDraft.PathParameters> | null,
      data?: any,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.PublishInvoiceDraft.Responses.$204>
  }
  ['/api/orders/{id}/publish']: {
    /**
     * PublishOrderDraft - Publish (and send to supplier) order with id
     */
    'post'(
      parameters?: Parameters<Paths.PublishOrderDraft.PathParameters> | null,
      data?: Paths.PublishOrderDraft.RequestBody,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.PublishOrderDraft.Responses.$204>
  }
  ['/api/token/refresh']: {
    /**
     * RefreshAccessToken - Regenerate an access_token for current user
     */
    'post'(
      parameters?: Parameters<UnknownParamsObject> | null,
      data?: Paths.RefreshAccessToken.RequestBody,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.RefreshAccessToken.Responses.$200>
  }
  ['/api/agreements/{id}/refuse']: {
    /**
     * RefuseAgreement - Refuse agreement
     */
    'put'(
      parameters?: Parameters<Paths.RefuseAgreement.PathParameters> | null,
      data?: Paths.RefuseAgreement.RequestBody,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.RefuseAgreement.Responses.$204>
  }
  ['/api/orders/{id}/refuse']: {
    /**
     * RefuseOrder - Refuse customer order with id
     */
    'post'(
      parameters?: Parameters<Paths.RefuseOrder.PathParameters> | null,
      data?: Paths.RefuseOrder.RequestBody,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.RefuseOrder.Responses.$204>
  }
  ['/api/register']: {
    /**
     * RegisterAccount - Create an account with specified email/password
     */
    'post'(
      parameters?: Parameters<UnknownParamsObject> | null,
      data?: Paths.RegisterAccount.RequestBody,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.RegisterAccount.Responses.$201>
  }
  ['/api/invoices/{id}']: {
    /**
     * RemoveInvoiceDraft - Remove invoice draft with id
     */
    'delete'(
      parameters?: Parameters<Paths.RemoveInvoiceDraft.PathParameters> | null,
      data?: any,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.RemoveInvoiceDraft.Responses.$204>
  }
  ['/api/password/reset']: {
    /**
     * ResetPassword - Reset the password with the token retrieved from email link
     */
    'post'(
      parameters?: Parameters<UnknownParamsObject> | null,
      data?: Paths.ResetPassword.RequestBody,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.ResetPassword.Responses.$204>
  }
  ['/api/agreements/{id}/revoke']: {
    /**
     * RevokeAgreement - Revoke agreement
     */
    'put'(
      parameters?: Parameters<Paths.RevokeAgreement.PathParameters> | null,
      data?: Paths.RevokeAgreement.RequestBody,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.RevokeAgreement.Responses.$204>
  }
  ['/api/invoices/{id}/send']: {
    /**
     * SendInvoice - Send invoice with id to customer
     */
    'post'(
      parameters?: Parameters<Paths.SendInvoice.PathParameters> | null,
      data?: any,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.SendInvoice.Responses.$204>
  }
  ['/api/orders/{id}/draft']: {
    /**
     * UpdateOrderDraftProducts - Update order with id products
     */
    'put'(
      parameters?: Parameters<Paths.UpdateOrderDraftProducts.PathParameters> | null,
      data?: Paths.UpdateOrderDraftProducts.RequestBody,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.UpdateOrderDraftProducts.Responses.$204>
  }
}

export type Client = OpenAPIClient<OperationMethods, PathsDictionary>
