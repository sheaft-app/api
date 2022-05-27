<script lang="ts">
  import { page, goto, params } from "@roxi/routify";
  import { configureCustomer, configureSupplier, refreshToken } from "$stores/auth";
  import type { IConfigureCustomer, IConfigureSupplier } from "$types/auth";
  import type { IAddress, INamedAddress } from "$types/address";
  import Select from "$components/Inputs/Select.svelte";
  import Text from "$components/Inputs/Text.svelte";
  import Siret from "$components/Inputs/Siret.svelte";
  import Email from "$components/Inputs/Email.svelte";
  import Phone from "$components/Inputs/Phone.svelte";
  import Stepper from "$components/Stepper/Stepper.svelte";
  import Checkbox from "$components/Inputs/Checkbox.svelte";
  import Address from "$components/Inputs/Address.svelte";
  import { ProfileKind } from "$enums/profile";
  import NamedAddress from "$components/Inputs/NamedAddress.svelte";

  export let account: IConfigureSupplier | IConfigureCustomer = {
    email: "",
    phone: "",
    tradeName: "",
    corporateName: "",
    siret: "",
    legalAddress: {
      street: "",
      postcode: "",
      city: ""
    }
  };

  let billingAddress: INamedAddress = {
    name: "",
    email: "",
    street: "",
    postcode: "",
    city: ""
  };

  let shippingAddress: INamedAddress = {
    name: "",
    email: "",
    street: "",
    postcode: "",
    city: ""
  };

  let deliveryAddress: INamedAddress = {
    name: "",
    email: "",
    street: "",
    postcode: "",
    city: ""
  };

  let isLoading: boolean = false;
  let accountType: ProfileKind;

  let accountTypeOptions = [
    { label: "Producteur", value: ProfileKind.Supplier },
    { label: "Commerçant", value: ProfileKind.Customer }
  ];

  let hasDifferentBillingAddress: boolean = false;
  let hasDifferentShippingAddress: boolean = false;
  let hasDifferentDeliveryAddress: boolean = false;

  let steps = [
    {
      name: "Informations",
      icon: "fas#AddressCard"
    },
    {
      name: "Légal",
      icon: "fas#Gavel"
    },
    {
      name: "Localisation",
      icon: "fas#Compass"
    }
  ];

  const configureAccount = async () => {
    try {
      if (!accountType) return;

      isLoading = true;

      if (hasDifferentBillingAddress) account.billingAddress = billingAddress;

      let result = null;
      switch (accountType) {
        case ProfileKind.Customer:
          if (hasDifferentDeliveryAddress)
            (<IConfigureCustomer>account).deliveryAddress = deliveryAddress;

          result = await configureCustomer(account);
          break;
        case ProfileKind.Supplier:
          if (hasDifferentShippingAddress)
            (<IConfigureSupplier>account).shippingAddress = shippingAddress;

          result = await configureSupplier(account);
          break;
      }

      if (!result) {
        isLoading = false;
        return;
      }

      await refreshToken();

      if ($params.returnUrl && $params.returnUrl.length > 1) $goto($params.returnUrl);
      else $goto("/");
    } catch (e) {
      isLoading = false;
    }
  };

  $: if (!hasDifferentBillingAddress) {
    account.billingAddress = null;
  }

  $: if (!hasDifferentShippingAddress) {
    (<IConfigureSupplier>account).shippingAddress = null;
  }

  $: if (!hasDifferentDeliveryAddress) {
    (<IConfigureCustomer>account).deliveryAddress = null;
  }
</script>

<!-- routify:options title="Renseigner vos informations" -->

<div class="flex justify-center items-center flex-wrap h-full g-6 text-gray-800 mb-6">
  <div class="md:w-8/12 lg:w-6/12 mb-12 md:mb-0">
    <img data-ujs-name="Sign in" />
  </div>
  <div class="md:w-8/12 lg:w-5/12 lg:ml-20">
    <h1>{$page.title}</h1>
    <form>
      <Stepper
        steps="{steps}"
        let:currentPosition
        submit="{configureAccount}"
        isLoading="{isLoading}"
      >
        {#if currentPosition == 0}
          <Select
            options="{accountTypeOptions}"
            isLoading="{isLoading}"
            bind:value="{accountType}"
          />
          <Text
            isLoading="{isLoading}"
            bind:value="{account.tradeName}"
            placeholder="Nom commercial"
          />
          <Email
            isLoading="{isLoading}"
            bind:value="{account.email}"
            placeholder="Adresse mail de contact"
          />
          <Phone
            isLoading="{isLoading}"
            bind:value="{account.phone}"
            placeholder="Numéro de téléphone de contact"
          />
        {:else if currentPosition == 1}
          <Text
            isLoading="{isLoading}"
            bind:value="{account.corporateName}"
            placeholder="Nom légal"
          />
          <Siret
            isLoading="{isLoading}"
            bind:value="{account.siret}"
            placeholder="Votre numéro de SIRET"
          />
          <Address isLoading="{isLoading}" bind:value="{account.legalAddress}" />
        {:else if currentPosition == 2 && accountType == ProfileKind.Customer}
          <Checkbox
            isLoading="{isLoading}"
            bind:value="{hasDifferentBillingAddress}"
            label="Mon adresse de facturation est différente"
          />
          {#if hasDifferentBillingAddress}
            <NamedAddress isLoading="{isLoading}" bind:value="{billingAddress}" />
          {/if}
          <Checkbox
            isLoading="{isLoading}"
            bind:value="{hasDifferentDeliveryAddress}"
            label="Mon adresse de livraison est différente"
          />
          {#if hasDifferentDeliveryAddress}
            <NamedAddress isLoading="{isLoading}" bind:value="{deliveryAddress}" />
          {/if}
        {:else if currentPosition == 2 && accountType == ProfileKind.Supplier}
          <Checkbox
            isLoading="{isLoading}"
            bind:value="{hasDifferentBillingAddress}"
            label="Mon adresse de facturation est différente"
          />
          {#if hasDifferentBillingAddress}
            <NamedAddress
              isLoading="{isLoading}"
              bind:value="{billingAddress}"
              showName="{true}"
              showEmail="{false}"
            />
          {/if}
          <Checkbox
            isLoading="{isLoading}"
            bind:value="{hasDifferentShippingAddress}"
            label="Mon adresse d'expedition est différente"
          />
          {#if hasDifferentShippingAddress}
            <NamedAddress
              isLoading="{isLoading}"
              bind:value="{shippingAddress}"
              showName="{true}"
              showEmail="{false}"
            />
          {/if}
        {/if}
      </Stepper>
    </form>
  </div>
</div>
