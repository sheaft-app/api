<script lang="ts">
  import type { Components } from "$types/api";
  import Input from "$components/Input/Input.svelte";
  import Address from "$components/Address/Address.svelte";

  export let profile:
    | Components.Schemas.AvailableCustomerDto
    | Components.Schemas.AvailableSupplierDto;

  $: profileAddress =
    (<Components.Schemas.AvailableCustomerDto>profile)?.deliveryAddress ??
    (<Components.Schemas.AvailableSupplierDto>profile)?.shippingAddress;
</script>

<Input label="Nom" value="{profile.name}" disabled="{true}" />
<Input type="email" label="Adresse mail" value="{profile.email}" disabled="{true}" />
<Input type="tel" label="Numéro de téléphone" value="{profile.phone}" disabled="{true}" />
<Address label="Adresse" disabled="{true}" value="{profileAddress}" />
