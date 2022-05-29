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
   * AcceptAgreement
   */
  'AcceptAgreement'(
    parameters?: Parameters<Paths.AcceptAgreement.PathParameters> | null,
    data?: Paths.AcceptAgreement.RequestBody,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.AcceptAgreement.Responses.$204>
  /**
   * AcceptOrder
   */
  'AcceptOrder'(
    parameters?: Parameters<Paths.AcceptOrder.PathParameters> | null,
    data?: Paths.AcceptOrder.RequestBody,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.AcceptOrder.Responses.$204>
  /**
   * CancelInvoice
   */
  'CancelInvoice'(
    parameters?: Parameters<Paths.CancelInvoice.PathParameters> | null,
    data?: Paths.CancelInvoice.RequestBody,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.CancelInvoice.Responses.$200>
  /**
   * CancelOrder
   */
  'CancelOrder'(
    parameters?: Parameters<Paths.CancelOrder.PathParameters> | null,
    data?: Paths.CancelOrder.RequestBody,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.CancelOrder.Responses.$204>
  /**
   * ConfigureAccountAsCustomer
   */
  'ConfigureAccountAsCustomer'(
    parameters?: Parameters<UnknownParamsObject> | null,
    data?: Paths.ConfigureAccountAsCustomer.RequestBody,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.ConfigureAccountAsCustomer.Responses.$200>
  /**
   * ConfigureAccountAsSupplier
   */
  'ConfigureAccountAsSupplier'(
    parameters?: Parameters<UnknownParamsObject> | null,
    data?: Paths.ConfigureAccountAsSupplier.RequestBody,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.ConfigureAccountAsSupplier.Responses.$200>
  /**
   * CreateBatch
   */
  'CreateBatch'(
    parameters?: Parameters<UnknownParamsObject> | null,
    data?: Paths.CreateBatch.RequestBody,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.CreateBatch.Responses.$201>
  /**
   * CreateCreditNoteDraft
   */
  'CreateCreditNoteDraft'(
    parameters?: Parameters<Paths.CreateCreditNoteDraft.PathParameters> | null,
    data?: any,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.CreateCreditNoteDraft.Responses.$201>
  /**
   * CreateInvoiceForDelivery
   */
  'CreateInvoiceForDelivery'(
    parameters?: Parameters<Paths.CreateInvoiceForDelivery.PathParameters> | null,
    data?: any,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.CreateInvoiceForDelivery.Responses.$201>
  /**
   * CreateOrderDraft
   */
  'CreateOrderDraft'(
    parameters?: Parameters<UnknownParamsObject> | null,
    data?: Paths.CreateOrderDraft.RequestBody,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.CreateOrderDraft.Responses.$201>
  /**
   * CreatePreparationDocument
   */
  'CreatePreparationDocument'(
    parameters?: Parameters<UnknownParamsObject> | null,
    data?: Paths.CreatePreparationDocument.RequestBody,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.CreatePreparationDocument.Responses.$201>
  /**
   * ListProducts
   */
  'ListProducts'(
    parameters?: Parameters<Paths.ListProducts.QueryParameters> | null,
    data?: any,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.ListProducts.Responses.$200>
  /**
   * CreateProduct
   */
  'CreateProduct'(
    parameters?: Parameters<UnknownParamsObject> | null,
    data?: Paths.CreateProduct.RequestBody,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.CreateProduct.Responses.$201>
  /**
   * CreateReturnable
   */
  'CreateReturnable'(
    parameters?: Parameters<UnknownParamsObject> | null,
    data?: Paths.CreateReturnable.RequestBody,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.CreateReturnable.Responses.$201>
  /**
   * UpdateBatch
   */
  'UpdateBatch'(
    parameters?: Parameters<Paths.UpdateBatch.PathParameters> | null,
    data?: Paths.UpdateBatch.RequestBody,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.UpdateBatch.Responses.$200>
  /**
   * DeleteBatch
   */
  'DeleteBatch'(
    parameters?: Parameters<Paths.DeleteBatch.PathParameters> | null,
    data?: any,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.DeleteBatch.Responses.$204>
  /**
   * GetProduct
   */
  'GetProduct'(
    parameters?: Parameters<Paths.GetProduct.PathParameters> | null,
    data?: any,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.GetProduct.Responses.$200>
  /**
   * UpdateProduct
   */
  'UpdateProduct'(
    parameters?: Parameters<Paths.UpdateProduct.PathParameters> | null,
    data?: Paths.UpdateProduct.RequestBody,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.UpdateProduct.Responses.$204>
  /**
   * DeleteProduct
   */
  'DeleteProduct'(
    parameters?: Parameters<Paths.DeleteProduct.PathParameters> | null,
    data?: any,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.DeleteProduct.Responses.$204>
  /**
   * UpdateReturnable
   */
  'UpdateReturnable'(
    parameters?: Parameters<Paths.UpdateReturnable.PathParameters> | null,
    data?: Paths.UpdateReturnable.RequestBody,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.UpdateReturnable.Responses.$204>
  /**
   * DeleteReturnable
   */
  'DeleteReturnable'(
    parameters?: Parameters<Paths.DeleteReturnable.PathParameters> | null,
    data?: any,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.DeleteReturnable.Responses.$204>
  /**
   * DeliverOrder
   */
  'DeliverOrder'(
    parameters?: Parameters<Paths.DeliverOrder.PathParameters> | null,
    data?: Paths.DeliverOrder.RequestBody,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.DeliverOrder.Responses.$204>
  /**
   * DownloadDocument
   */
  'DownloadDocument'(
    parameters?: Parameters<Paths.DownloadDocument.PathParameters> | null,
    data?: any,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.DownloadDocument.Responses.$200>
  /**
   * RemoveDocument
   */
  'RemoveDocument'(
    parameters?: Parameters<Paths.RemoveDocument.PathParameters> | null,
    data?: any,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.RemoveDocument.Responses.$204>
  /**
   * ForgotPassword
   */
  'ForgotPassword'(
    parameters?: Parameters<UnknownParamsObject> | null,
    data?: Paths.ForgotPassword.RequestBody,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.ForgotPassword.Responses.$204>
  /**
   * FulfillOrder
   */
  'FulfillOrder'(
    parameters?: Parameters<Paths.FulfillOrder.PathParameters> | null,
    data?: Paths.FulfillOrder.RequestBody,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.FulfillOrder.Responses.$204>
  /**
   * LoginUser
   */
  'LoginUser'(
    parameters?: Parameters<UnknownParamsObject> | null,
    data?: Paths.LoginUser.RequestBody,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.LoginUser.Responses.$200>
  /**
   * MarkInvoiceAsPayed
   */
  'MarkInvoiceAsPayed'(
    parameters?: Parameters<Paths.MarkInvoiceAsPayed.PathParameters> | null,
    data?: Paths.MarkInvoiceAsPayed.RequestBody,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.MarkInvoiceAsPayed.Responses.$204>
  /**
   * ProposeAgreementToCustomer
   */
  'ProposeAgreementToCustomer'(
    parameters?: Parameters<Paths.ProposeAgreementToCustomer.PathParameters> | null,
    data?: Paths.ProposeAgreementToCustomer.RequestBody,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.ProposeAgreementToCustomer.Responses.$201>
  /**
   * ProposeAgreementToSupplier
   */
  'ProposeAgreementToSupplier'(
    parameters?: Parameters<Paths.ProposeAgreementToSupplier.PathParameters> | null,
    data?: any,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.ProposeAgreementToSupplier.Responses.$201>
  /**
   * PublishInvoiceDraft
   */
  'PublishInvoiceDraft'(
    parameters?: Parameters<Paths.PublishInvoiceDraft.PathParameters> | null,
    data?: any,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.PublishInvoiceDraft.Responses.$204>
  /**
   * PublishOrderDraft
   */
  'PublishOrderDraft'(
    parameters?: Parameters<Paths.PublishOrderDraft.PathParameters> | null,
    data?: Paths.PublishOrderDraft.RequestBody,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.PublishOrderDraft.Responses.$204>
  /**
   * RefreshAccessToken
   */
  'RefreshAccessToken'(
    parameters?: Parameters<UnknownParamsObject> | null,
    data?: Paths.RefreshAccessToken.RequestBody,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.RefreshAccessToken.Responses.$200>
  /**
   * RefuseOrder
   */
  'RefuseOrder'(
    parameters?: Parameters<Paths.RefuseOrder.PathParameters> | null,
    data?: Paths.RefuseOrder.RequestBody,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.RefuseOrder.Responses.$204>
  /**
   * RegisterAccount
   */
  'RegisterAccount'(
    parameters?: Parameters<UnknownParamsObject> | null,
    data?: Paths.RegisterAccount.RequestBody,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.RegisterAccount.Responses.$201>
  /**
   * RemoveInvoiceDraft
   */
  'RemoveInvoiceDraft'(
    parameters?: Parameters<Paths.RemoveInvoiceDraft.PathParameters> | null,
    data?: any,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.RemoveInvoiceDraft.Responses.$204>
  /**
   * ResetPassword
   */
  'ResetPassword'(
    parameters?: Parameters<UnknownParamsObject> | null,
    data?: Paths.ResetPassword.RequestBody,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.ResetPassword.Responses.$204>
  /**
   * SendInvoice
   */
  'SendInvoice'(
    parameters?: Parameters<Paths.SendInvoice.PathParameters> | null,
    data?: any,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.SendInvoice.Responses.$204>
  /**
   * UpdateCustomer
   */
  'UpdateCustomer'(
    parameters?: Parameters<Paths.UpdateCustomer.PathParameters> | null,
    data?: Paths.UpdateCustomer.RequestBody,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.UpdateCustomer.Responses.$200>
  /**
   * UpdateOrderDraftProducts
   */
  'UpdateOrderDraftProducts'(
    parameters?: Parameters<Paths.UpdateOrderDraftProducts.PathParameters> | null,
    data?: Paths.UpdateOrderDraftProducts.RequestBody,
    config?: AxiosRequestConfig  
  ): OperationResponse<Paths.UpdateOrderDraftProducts.Responses.$204>
  /**
   * UpdateSupplier
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
     * AcceptAgreement
     */
    'put'(
      parameters?: Parameters<Paths.AcceptAgreement.PathParameters> | null,
      data?: Paths.AcceptAgreement.RequestBody,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.AcceptAgreement.Responses.$204>
  }
  ['/api/orders/{id}/accept']: {
    /**
     * AcceptOrder
     */
    'post'(
      parameters?: Parameters<Paths.AcceptOrder.PathParameters> | null,
      data?: Paths.AcceptOrder.RequestBody,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.AcceptOrder.Responses.$204>
  }
  ['/api/invoices/{id}/cancel']: {
    /**
     * CancelInvoice
     */
    'post'(
      parameters?: Parameters<Paths.CancelInvoice.PathParameters> | null,
      data?: Paths.CancelInvoice.RequestBody,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.CancelInvoice.Responses.$200>
  }
  ['/api/orders/{id}/cancel']: {
    /**
     * CancelOrder
     */
    'post'(
      parameters?: Parameters<Paths.CancelOrder.PathParameters> | null,
      data?: Paths.CancelOrder.RequestBody,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.CancelOrder.Responses.$204>
  }
  ['/api/account/configure/customer']: {
    /**
     * ConfigureAccountAsCustomer
     */
    'post'(
      parameters?: Parameters<UnknownParamsObject> | null,
      data?: Paths.ConfigureAccountAsCustomer.RequestBody,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.ConfigureAccountAsCustomer.Responses.$200>
  }
  ['/api/account/configure/supplier']: {
    /**
     * ConfigureAccountAsSupplier
     */
    'post'(
      parameters?: Parameters<UnknownParamsObject> | null,
      data?: Paths.ConfigureAccountAsSupplier.RequestBody,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.ConfigureAccountAsSupplier.Responses.$200>
  }
  ['/api/batches']: {
    /**
     * CreateBatch
     */
    'post'(
      parameters?: Parameters<UnknownParamsObject> | null,
      data?: Paths.CreateBatch.RequestBody,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.CreateBatch.Responses.$201>
  }
  ['/api/invoices/{id}/credit']: {
    /**
     * CreateCreditNoteDraft
     */
    'post'(
      parameters?: Parameters<Paths.CreateCreditNoteDraft.PathParameters> | null,
      data?: any,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.CreateCreditNoteDraft.Responses.$201>
  }
  ['/api/deliveries/{id}/invoice']: {
    /**
     * CreateInvoiceForDelivery
     */
    'post'(
      parameters?: Parameters<Paths.CreateInvoiceForDelivery.PathParameters> | null,
      data?: any,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.CreateInvoiceForDelivery.Responses.$201>
  }
  ['/api/orders/drafts']: {
    /**
     * CreateOrderDraft
     */
    'post'(
      parameters?: Parameters<UnknownParamsObject> | null,
      data?: Paths.CreateOrderDraft.RequestBody,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.CreateOrderDraft.Responses.$201>
  }
  ['/api/documents/preparation']: {
    /**
     * CreatePreparationDocument
     */
    'post'(
      parameters?: Parameters<UnknownParamsObject> | null,
      data?: Paths.CreatePreparationDocument.RequestBody,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.CreatePreparationDocument.Responses.$201>
  }
  ['/api/products']: {
    /**
     * CreateProduct
     */
    'post'(
      parameters?: Parameters<UnknownParamsObject> | null,
      data?: Paths.CreateProduct.RequestBody,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.CreateProduct.Responses.$201>
    /**
     * ListProducts
     */
    'get'(
      parameters?: Parameters<Paths.ListProducts.QueryParameters> | null,
      data?: any,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.ListProducts.Responses.$200>
  }
  ['/api/returnables']: {
    /**
     * CreateReturnable
     */
    'post'(
      parameters?: Parameters<UnknownParamsObject> | null,
      data?: Paths.CreateReturnable.RequestBody,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.CreateReturnable.Responses.$201>
  }
  ['/api/batches/{id}']: {
    /**
     * DeleteBatch
     */
    'delete'(
      parameters?: Parameters<Paths.DeleteBatch.PathParameters> | null,
      data?: any,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.DeleteBatch.Responses.$204>
    /**
     * UpdateBatch
     */
    'put'(
      parameters?: Parameters<Paths.UpdateBatch.PathParameters> | null,
      data?: Paths.UpdateBatch.RequestBody,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.UpdateBatch.Responses.$200>
  }
  ['/api/products/{id}']: {
    /**
     * DeleteProduct
     */
    'delete'(
      parameters?: Parameters<Paths.DeleteProduct.PathParameters> | null,
      data?: any,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.DeleteProduct.Responses.$204>
    /**
     * GetProduct
     */
    'get'(
      parameters?: Parameters<Paths.GetProduct.PathParameters> | null,
      data?: any,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.GetProduct.Responses.$200>
    /**
     * UpdateProduct
     */
    'put'(
      parameters?: Parameters<Paths.UpdateProduct.PathParameters> | null,
      data?: Paths.UpdateProduct.RequestBody,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.UpdateProduct.Responses.$204>
  }
  ['/api/returnables/{id}']: {
    /**
     * DeleteReturnable
     */
    'delete'(
      parameters?: Parameters<Paths.DeleteReturnable.PathParameters> | null,
      data?: any,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.DeleteReturnable.Responses.$204>
    /**
     * UpdateReturnable
     */
    'put'(
      parameters?: Parameters<Paths.UpdateReturnable.PathParameters> | null,
      data?: Paths.UpdateReturnable.RequestBody,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.UpdateReturnable.Responses.$204>
  }
  ['/api/deliveries/{id}/deliver']: {
    /**
     * DeliverOrder
     */
    'post'(
      parameters?: Parameters<Paths.DeliverOrder.PathParameters> | null,
      data?: Paths.DeliverOrder.RequestBody,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.DeliverOrder.Responses.$204>
  }
  ['/api/documents/{id}']: {
    /**
     * DownloadDocument
     */
    'post'(
      parameters?: Parameters<Paths.DownloadDocument.PathParameters> | null,
      data?: any,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.DownloadDocument.Responses.$200>
    /**
     * RemoveDocument
     */
    'delete'(
      parameters?: Parameters<Paths.RemoveDocument.PathParameters> | null,
      data?: any,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.RemoveDocument.Responses.$204>
  }
  ['/api/password/forgot']: {
    /**
     * ForgotPassword
     */
    'post'(
      parameters?: Parameters<UnknownParamsObject> | null,
      data?: Paths.ForgotPassword.RequestBody,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.ForgotPassword.Responses.$204>
  }
  ['/api/orders/{id}/fulfill']: {
    /**
     * FulfillOrder
     */
    'post'(
      parameters?: Parameters<Paths.FulfillOrder.PathParameters> | null,
      data?: Paths.FulfillOrder.RequestBody,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.FulfillOrder.Responses.$204>
  }
  ['/api/token/login']: {
    /**
     * LoginUser
     */
    'post'(
      parameters?: Parameters<UnknownParamsObject> | null,
      data?: Paths.LoginUser.RequestBody,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.LoginUser.Responses.$200>
  }
  ['/api/invoices/{id}/payed']: {
    /**
     * MarkInvoiceAsPayed
     */
    'post'(
      parameters?: Parameters<Paths.MarkInvoiceAsPayed.PathParameters> | null,
      data?: Paths.MarkInvoiceAsPayed.RequestBody,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.MarkInvoiceAsPayed.Responses.$204>
  }
  ['/api/customers/{id}/agreement']: {
    /**
     * ProposeAgreementToCustomer
     */
    'post'(
      parameters?: Parameters<Paths.ProposeAgreementToCustomer.PathParameters> | null,
      data?: Paths.ProposeAgreementToCustomer.RequestBody,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.ProposeAgreementToCustomer.Responses.$201>
  }
  ['/api/suppliers/{id}/agreement']: {
    /**
     * ProposeAgreementToSupplier
     */
    'post'(
      parameters?: Parameters<Paths.ProposeAgreementToSupplier.PathParameters> | null,
      data?: any,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.ProposeAgreementToSupplier.Responses.$201>
  }
  ['/api/invoices/{id}/publish']: {
    /**
     * PublishInvoiceDraft
     */
    'post'(
      parameters?: Parameters<Paths.PublishInvoiceDraft.PathParameters> | null,
      data?: any,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.PublishInvoiceDraft.Responses.$204>
  }
  ['/api/orders/{id}/publish']: {
    /**
     * PublishOrderDraft
     */
    'post'(
      parameters?: Parameters<Paths.PublishOrderDraft.PathParameters> | null,
      data?: Paths.PublishOrderDraft.RequestBody,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.PublishOrderDraft.Responses.$204>
  }
  ['/api/token/refresh']: {
    /**
     * RefreshAccessToken
     */
    'post'(
      parameters?: Parameters<UnknownParamsObject> | null,
      data?: Paths.RefreshAccessToken.RequestBody,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.RefreshAccessToken.Responses.$200>
  }
  ['/api/orders/{id}/refuse']: {
    /**
     * RefuseOrder
     */
    'post'(
      parameters?: Parameters<Paths.RefuseOrder.PathParameters> | null,
      data?: Paths.RefuseOrder.RequestBody,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.RefuseOrder.Responses.$204>
  }
  ['/api/register']: {
    /**
     * RegisterAccount
     */
    'post'(
      parameters?: Parameters<UnknownParamsObject> | null,
      data?: Paths.RegisterAccount.RequestBody,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.RegisterAccount.Responses.$201>
  }
  ['/api/invoices/{id}']: {
    /**
     * RemoveInvoiceDraft
     */
    'delete'(
      parameters?: Parameters<Paths.RemoveInvoiceDraft.PathParameters> | null,
      data?: any,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.RemoveInvoiceDraft.Responses.$204>
  }
  ['/api/password/reset']: {
    /**
     * ResetPassword
     */
    'post'(
      parameters?: Parameters<UnknownParamsObject> | null,
      data?: Paths.ResetPassword.RequestBody,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.ResetPassword.Responses.$204>
  }
  ['/api/invoices/{id}/send']: {
    /**
     * SendInvoice
     */
    'post'(
      parameters?: Parameters<Paths.SendInvoice.PathParameters> | null,
      data?: any,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.SendInvoice.Responses.$204>
  }
  ['/api/customers/{id}']: {
    /**
     * UpdateCustomer
     */
    'put'(
      parameters?: Parameters<Paths.UpdateCustomer.PathParameters> | null,
      data?: Paths.UpdateCustomer.RequestBody,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.UpdateCustomer.Responses.$200>
  }
  ['/api/orders/{id}/draft']: {
    /**
     * UpdateOrderDraftProducts
     */
    'put'(
      parameters?: Parameters<Paths.UpdateOrderDraftProducts.PathParameters> | null,
      data?: Paths.UpdateOrderDraftProducts.RequestBody,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.UpdateOrderDraftProducts.Responses.$204>
  }
  ['/api/suppliers/{id}']: {
    /**
     * UpdateSupplier
     */
    'put'(
      parameters?: Parameters<Paths.UpdateSupplier.PathParameters> | null,
      data?: Paths.UpdateSupplier.RequestBody,
      config?: AxiosRequestConfig  
    ): OperationResponse<Paths.UpdateSupplier.Responses.$200>
  }
}

export type Client = OpenAPIClient<OperationMethods, PathsDictionary>
