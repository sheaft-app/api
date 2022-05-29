<script lang="ts">
  import "./inputs.scss";
  import type { IAddress } from "$types/address";
  import Text from "$components/Inputs/Text.svelte";

  export let value: IAddress = { street: "", complement: "", postcode: "", city: "" };
  export let label: string = "";
  export let isLoading: boolean = false;
  export let required: boolean = true;
</script>

<Text
  disabled="{isLoading}"
  placeholder="Adresse"
  bind:value="{value.street}"
  required="{required}"
/>
<Text
  type="text"
  disabled="{isLoading}"
  placeholder="Complément d'adresse"
  bind:value="{value.complement}"
/>
<Text
  type="text"
  disabled="{isLoading}"
  placeholder="Code postal"
  bind:value="{value.postcode}"
  required="{required}"
/>
<Text
  type="text"
  disabled="{isLoading}"
  placeholder="Ville"
  bind:value="{value.city}"
  required="{required}"
/>
