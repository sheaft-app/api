import type {
  OpenAPIClient,
  Parameters,
  UnknownParamsObject,
  OperationResponse,
  AxiosRequestConfig,
} from 'openapi-client-axios'; 

declare namespace Components {
    namespace Schemas {
        export interface AcceptAgreementRequest {
            deliveryDays?: DayOfWeek /* int32 */[] | null;
            orderDelayInHoursBeforeDeliveryDay?: null | number; // int32
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
        export interface CreateOrderDraftRequest {
            supplierIdentifier?: string | null;
        }
        export interface CreatePreparationDocumentRequest {
            orderIdentifiers?: string[] | null;
            autoAcceptPendingOrders?: boolean | null;
        }
        export interface CreateProductRequest {
            name?: string | null;
            code?: string | null;
            price?: number; // double
            vat?: number; // double
            description?: string | null;
            returnableIdentifier?: string | null;
        }
        export interface CreateReturnableRequest {
            name?: string | null;
            code?: string | null;
            price?: number; // double
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
        export type PaymentKind = 0 | 1; // int32
        export interface ProblemDetails {
            [name: string]: any;
            type?: string | null;
            title?: string | null;
            status?: null | number; // int32
            detail?: string | null;
            instance?: string | null;
        }
        export interface ProductDto {
            id?: string | null;
            name?: string | null;
            reference?: string | null;
            description?: string | null;
            unitPrice?: number; // double
            vat?: number; // double
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
            price?: number; // double
            vat?: number; // double
            description?: string | null;
            returnableIdentifier?: string | null;
        }
        export interface UpdateReturnableRequest {
            name?: string | null;
            code?: string | null;
            price?: number; // double
            vat?: number; // double
        }
    }
}
declare namespace Paths {
    namespace AcceptAgreement {
        namespace Parameters {
            export type Id = string;
        }
        export interface PathParameters {
            id: Parameters.Id;
        }
        export type RequestBody = Components.Schemas.AcceptAgreementRequest;
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
        export type RequestBody = Components.Schemas.CreateOrderDraftRequest;
        namespace Responses {
            export type $201 = string;
            export type $400 = Components.Schemas.ProblemDetails;
        }
    }
    namespace CreatePreparationDocument {
        export type RequestBody = Components.Schemas.CreatePreparationDocumentRequest;
        namespace Responses {
            export type $201 = string;
            export type $400 = Components.Schemas.ProblemDetails;
        }
    }
    namespace CreateProduct {
        export type RequestBody = Components.Schemas.CreateProductRequest;
        namespace Responses {
            export type $201 = string;
            export type $400 = Components.Schemas.ProblemDetails;
        }
    }
    namespace CreateReturnable {
        export type RequestBody = Components.Schemas.CreateReturnableRequest;
        namespace Responses {
            export type $201 = string;
            export type $400 = Components.Schemas.ProblemDetails;
        }
    }
    namespace DeleteBatch {
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
    namespace DeleteProduct {
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
    namespace DeleteReturnable {
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
    namespace GetProduct {
        namespace Parameters {
            export type Id = string;
        }
        export interface PathParameters {
            id: Parameters.Id;
        }
        namespace Responses {
            export type $200 = Components.Schemas.ProductDto;
            export type $400 = Components.Schemas.ProblemDetails;
        }
    }
    namespace ListProducts {
        namespace Parameters {
            export type Page = number; // int32
            export type Take = number; // int32
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
    namespace UpdateBatch {
        namespace Parameters {
            export type Id = string;
        }
        export interface PathParameters {
            id: Parameters.Id;
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
            export type Id = string;
        }
        export interface PathParameters {
            id: Parameters.Id;
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
            export type Id = string;
        }
        export interface PathParameters {
            id: Parameters.Id;
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
   * AcceptAgreement - Accept agreeement
   */
  'AcceptAgreement'(
    parameters?: Parameters<Paths.AcceptAgreement.PathParameters> | null,
    data?: Paths.AcceptAgreement.RequestBody,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.AcceptAgreement.Responses.$204>
  /**
   * AcceptOrder - Accept order with id
   */
  'AcceptOrder'(
    parameters?: Parameters<Paths.AcceptOrder.PathParameters> | null,
    data?: Paths.AcceptOrder.RequestBody,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.AcceptOrder.Responses.$204>
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
    parameters?: Parameters<UnknownParamsObject> | null,
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
    parameters?: Parameters<UnknownParamsObject> | null,
    data?: Paths.CreateOrderDraft.RequestBody,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.CreateOrderDraft.Responses.$201>
  /**
   * CreatePreparationDocument - Create a preparation document for specfified orders
   */
  'CreatePreparationDocument'(
    parameters?: Parameters<UnknownParamsObject> | null,
    data?: Paths.CreatePreparationDocument.RequestBody,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.CreatePreparationDocument.Responses.$201>
  /**
   * ListProducts - List available products for supplier
   */
  'ListProducts'(
    parameters?: Parameters<Paths.ListProducts.QueryParameters> | null,
    data?: any,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.ListProducts.Responses.$200>
  /**
   * CreateProduct - Create a product
   */
  'CreateProduct'(
    parameters?: Parameters<UnknownParamsObject> | null,
    data?: Paths.CreateProduct.RequestBody,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.CreateProduct.Responses.$201>
  /**
   * CreateReturnable - Create a returnable to be used with products
   */
  'CreateReturnable'(
    parameters?: Parameters<UnknownParamsObject> | null,
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
   * SendInvoice - Send invoice with id to customer
   */
  'SendInvoice'(
    parameters?: Parameters<Paths.SendInvoice.PathParameters> | null,
    data?: any,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.SendInvoice.Responses.$204>
  /**
   * UpdateCustomer - Update current user customer profile
   */
  'UpdateCustomer'(
    parameters?: Parameters<Paths.UpdateCustomer.PathParameters> | null,
    data?: Paths.UpdateCustomer.RequestBody,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.UpdateCustomer.Responses.$200>
  /**
   * UpdateOrderDraftProducts - Update order with id products
   */
  'UpdateOrderDraftProducts'(
    parameters?: Parameters<Paths.UpdateOrderDraftProducts.PathParameters> | null,
    data?: Paths.UpdateOrderDraftProducts.RequestBody,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.UpdateOrderDraftProducts.Responses.$204>
  /**
   * UpdateSupplier - Update current user supplier profile
   */
  'UpdateSupplier'(
    parameters?: Parameters<Paths.UpdateSupplier.PathParameters> | null,
    data?: Paths.UpdateSupplier.RequestBody,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.UpdateSupplier.Responses.$200>
}

export interface PathsDictionary {
  ['/api/agreements/{id}/accept']: {
    /**
     * AcceptAgreement - Accept agreeement
     */
    'put'(
      parameters?: Parameters<Paths.AcceptAgreement.PathParameters> | null,
      data?: Paths.AcceptAgreement.RequestBody,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.AcceptAgreement.Responses.$204>
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
  ['/api/batches']: {
    /**
     * CreateBatch - Create a batch with specified info
     */
    'post'(
      parameters?: Parameters<UnknownParamsObject> | null,
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
  ['/api/orders/drafts']: {
    /**
     * CreateOrderDraft - Create a new order draft
     */
    'post'(
      parameters?: Parameters<UnknownParamsObject> | null,
      data?: Paths.CreateOrderDraft.RequestBody,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.CreateOrderDraft.Responses.$201>
  }
  ['/api/documents/preparation']: {
    /**
     * CreatePreparationDocument - Create a preparation document for specfified orders
     */
    'post'(
      parameters?: Parameters<UnknownParamsObject> | null,
      data?: Paths.CreatePreparationDocument.RequestBody,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.CreatePreparationDocument.Responses.$201>
  }
  ['/api/products']: {
    /**
     * CreateProduct - Create a product
     */
    'post'(
      parameters?: Parameters<UnknownParamsObject> | null,
      data?: Paths.CreateProduct.RequestBody,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.CreateProduct.Responses.$201>
    /**
     * ListProducts - List available products for supplier
     */
    'get'(
      parameters?: Parameters<Paths.ListProducts.QueryParameters> | null,
      data?: any,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.ListProducts.Responses.$200>
  }
  ['/api/returnables']: {
    /**
     * CreateReturnable - Create a returnable to be used with products
     */
    'post'(
      parameters?: Parameters<UnknownParamsObject> | null,
      data?: Paths.CreateReturnable.RequestBody,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.CreateReturnable.Responses.$201>
  }
  ['/api/batches/{id}']: {
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
  ['/api/products/{id}']: {
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
  ['/api/returnables/{id}']: {
    /**
     * DeleteReturnable - Remove returnable with id
     */
    'delete'(
      parameters?: Parameters<Paths.DeleteReturnable.PathParameters> | null,
      data?: any,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.DeleteReturnable.Responses.$204>
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
  ['/api/suppliers/{id}/agreement']: {
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
  ['/api/customers/{id}']: {
    /**
     * UpdateCustomer - Update current user customer profile
     */
    'put'(
      parameters?: Parameters<Paths.UpdateCustomer.PathParameters> | null,
      data?: Paths.UpdateCustomer.RequestBody,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.UpdateCustomer.Responses.$200>
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
  ['/api/suppliers/{id}']: {
    /**
     * UpdateSupplier - Update current user supplier profile
     */
    'put'(
      parameters?: Parameters<Paths.UpdateSupplier.PathParameters> | null,
      data?: Paths.UpdateSupplier.RequestBody,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.UpdateSupplier.Responses.$200>
  }
}

export type Client = OpenAPIClient<OperationMethods, PathsDictionary>