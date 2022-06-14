<script lang="ts">
  import Checkbox from "$components/Inputs/Checkbox.svelte";
  import NamedAddress from "$components/Addresses/NamedAddress.svelte";
  import { createForm } from "felte";
  import FormFooter from "$components/Form/FormFooter.svelte";
  import Button from "$components/Buttons/Button.svelte";
  import { ProfileKind } from "$enums/profile";
  import Address from "$components/Addresses/Address.svelte";
  import { onMount } from "svelte";
  import type { IAccountAddresses, IAccountConfigurationResults } from '$features/account/types'

  export let initialValues: IAccountAddresses | null;
  export let onSubmit;
  export let onBack;
  export let state: IAccountConfigurationResults;

  let hasDifferentBillingAddress = false;
  let hasDifferentSecondaryAddress = false;

  const { form, data, isSubmitting, setData } = createForm<IAccountAddresses>({
    initialValues,
    onSubmit
  });

  $: if (hasDifferentBillingAddress) {
    setData(<any>"billingAddress.name", value => state.information.tradeName);
    setData(<any>"billingAddress.email", value => state.information.email);
  } else {
    setData(<any>"billingAddress", value => undefined);
  }

  $: if (
    hasDifferentSecondaryAddress &&
    state.information.accountType === ProfileKind.Customer
  ) {
    setData(<any>"deliveryAddress.name", value => state.information.tradeName);
    setData(<any>"deliveryAddress.email", value => state.information.email);
  } else if (
    hasDifferentSecondaryAddress &&
    state.information.accountType === ProfileKind.Supplier
  ) {
    setData(<any>"shippingAddress.name", value => state.information.tradeName);
    setData(<any>"shippingAddress.email", value => state.information.email);
  } else {
    setData(<any>"shippingAddress", value => undefined);
    setData(<any>"deliveryAddress", value => undefined);
  }

  onMount(() => {
    hasDifferentBillingAddress = !!state.addresses?.billingAddress;
    hasDifferentSecondaryAddress =
      !!state.addresses?.deliveryAddress || !!state.addresses?.shippingAddress;
  });
</script>

<form use:form>
  <Address
    id="legalAddress"
    label="Siège social"
    disabled="{$isSubmitting}"
    bind:value="{$data.legalAddress}"
  />
  <Checkbox
    id="hasDifferentBillingAddress"
    disabled="{$isSubmitting}"
    bind:value="{hasDifferentBillingAddress}"
    label="Mon adresse de facturation est différente"
    required="{false}"
  />
  {#if hasDifferentBillingAddress}
    <NamedAddress
      id="billingAddress"
      label="Adresse de facturation"
      disabled="{$isSubmitting}"
      bind:value="{$data.billingAddress}"
      required="{hasDifferentBillingAddress}"
    />
  {/if}
  <Checkbox
    id="hasDifferentSecondaryAddress"
    disabled="{$isSubmitting}"
    bind:value="{hasDifferentSecondaryAddress}"
    label="{state.information.accountType === ProfileKind.Customer
      ? 'Mon adresse de livraison est différente'
      : 'Mon adresse d\'expédition est différente'}"
    required="{false}"
  />
  {#if hasDifferentSecondaryAddress && state.information.accountType === ProfileKind.Customer}
    <NamedAddress
      id="deliveryAddress"
      label="Adresse de livraison"
      disabled="{$isSubmitting}"
      bind:value="{$data.deliveryAddress}"
      required="{hasDifferentSecondaryAddress}"
    />
  {/if}
  {#if hasDifferentSecondaryAddress && state.information.accountType === ProfileKind.Supplier}
    <NamedAddress
      id="shippingAddress"
      label="Adresse d'expédition"
      disabled="{$isSubmitting}"
      bind:value="{$data.shippingAddress}"
      required="{hasDifferentSecondaryAddress}"
    />
  {/if}
  <FormFooter class="mt-2">
    <Button
      class="back w-full mx-8"
      disabled="{$isSubmitting}"
      type="button"
      on:click="{() => onBack($data)}"
    >
      Précédent
    </Button>
    <Button
      class="primary w-full mx-8"
      disabled="{$isSubmitting}"
      isLoading="{$isSubmitting}"
      type="submit"
      >Enregistrer
    </Button>
  </FormFooter>
</form>
