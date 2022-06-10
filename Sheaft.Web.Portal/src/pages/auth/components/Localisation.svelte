<script lang='ts'>
  import Checkbox from '$components/Inputs/Checkbox.svelte'
  import NamedAddress from '$components/Inputs/NamedAddress.svelte'
  import { createForm } from 'felte'
  import FormFooter from '$components/Form/FormFooter.svelte'
  import Button from '$components/Buttons/Button.svelte'
  import { ProfileKind } from '$enums/profile'

  export let initialValues
  export let onSubmit
  export let onBack
  export let accountType:ProfileKind|null = null;

  const { form, data, isSubmitting } = createForm({ initialValues, onSubmit })

  let hasDifferentBillingAddress = false;
  let hasDifferentSecondaryAddress = false;
</script>

<h2>Localisation</h2>
<form use:form>
  <Checkbox
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
      required='{hasDifferentBillingAddress}'/>
  {/if}
  <Checkbox
    isLoading='{$isSubmitting}'
    bind:value='{hasDifferentSecondaryAddress}'
    label="{accountType === ProfileKind.Customer ? 'Mon adresse de livraison est différente' : 'Mon adresse d\'expedition est différente'}"
    required='{false}'
  />
  {#if hasDifferentSecondaryAddress && accountType === ProfileKind.Customer}
    <NamedAddress 
      id='deliveryAddress'
      isLoading='{$isSubmitting}' 
      bind:value='{$data.deliveryAddress}'
      required='{hasDifferentSecondaryAddress}' />
  {/if}
  {#if hasDifferentSecondaryAddress && accountType === ProfileKind.Supplier}
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
      type="button" on:click="{() => onBack($data)}">
      Précédent
    </Button>
    <Button
      class='primary w-full mx-8'
      disabled='{$isSubmitting}'
      isLoading='{$isSubmitting}'
      type="submit">Enregistrer</Button>
  </FormFooter>
</form>
