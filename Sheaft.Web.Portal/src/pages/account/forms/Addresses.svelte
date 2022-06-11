<script lang='ts'>
  import Checkbox from '$components/Inputs/Checkbox.svelte'
  import NamedAddress from '$components/Inputs/NamedAddress.svelte'
  import { createForm } from 'felte'
  import FormFooter from '$components/Form/FormFooter.svelte'
  import Button from '$components/Buttons/Button.svelte'
  import { ProfileKind } from '$enums/profile'
  import type { IAccountAddresses } from '$commands/account/configureAccount'
  import Address from '$components/Inputs/Address.svelte'
  import type { IAccountConfigurationResults } from '$pages/account/account'

  export let initialValues: IAccountAddresses | null
  export let onSubmit
  export let onBack
  export let state:IAccountConfigurationResults

  const { form, data, isSubmitting } = createForm<IAccountAddresses>({ initialValues, onSubmit })

  let hasDifferentBillingAddress = false
  let hasDifferentSecondaryAddress = false
</script>

<form use:form>
  <Address
    id='legalAddress'
    isLoading='{$isSubmitting}'
    bind:value='{$data.legalAddress}' />
  <Checkbox
    id='billingAddressIsDifferent'
    class='block'
    isLoading='{$isSubmitting}'
    bind:value='{hasDifferentBillingAddress}'
    label='Mon adresse de facturation est différente'
    required='{false}'
  />
  {#if hasDifferentBillingAddress}
    <NamedAddress
      id='billingAddress'
      isLoading='{$isSubmitting}'
      bind:value='{$data.billingAddress}'
      required='{hasDifferentBillingAddress}' />
  {/if}
  <Checkbox
    id='secondaryAddressIsDifferent'
    class='block'
    isLoading='{$isSubmitting}'
    bind:value='{hasDifferentSecondaryAddress}'
    label="{state.information.values.accountType === ProfileKind.Customer ? 'Mon adresse de livraison est différente' : 'Mon adresse d\'expédition est différente'}"
    required='{false}'
  />
  {#if hasDifferentSecondaryAddress && state.information.values.accountType === ProfileKind.Customer}
    <NamedAddress
      id='deliveryAddress'
      isLoading='{$isSubmitting}'
      bind:value='{$data.deliveryAddress}'
      required='{hasDifferentSecondaryAddress}' />
  {/if}
  {#if hasDifferentSecondaryAddress && state.information.values.accountType === ProfileKind.Supplier}
    <NamedAddress
      id='shippingAddress'
      isLoading='{$isSubmitting}'
      bind:value='{$data.shippingAddress}'
      required='{hasDifferentSecondaryAddress}' />
  {/if}
  <FormFooter>
    <Button
      class='back w-full mx-8'
      disabled='{$isSubmitting}'
      type='button' on:click='{() => onBack($data)}'>
      Précédent
    </Button>
    <Button
      class='primary w-full mx-8'
      disabled='{$isSubmitting}'
      isLoading='{$isSubmitting}'
      type='submit'>Enregistrer
    </Button>
  </FormFooter>
</form>
