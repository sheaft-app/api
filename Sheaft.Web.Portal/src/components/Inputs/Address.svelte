<script lang="ts">
  import "./inputs.scss";
  import type { IAddress } from "$types/address";

  export let value: IAddress = { street: "", complement: "", postcode: "", city: "" };
  export let label: string = "";
  export let isLoading: boolean = false;
</script>

<div class="f-input">
  <input
    type="text"
    disabled="{isLoading}"
    placeholder="Adresse"
    bind:value="{value.street}"
  />
</div>
<div class="f-input">
  <input
    type="text"
    disabled="{isLoading}"
    placeholder="Complément d'adresse"
    bind:value="{value.complement}"
  />
</div>
<div class="f-input">
  <input
    type="text"
    disabled="{isLoading}"
    placeholder="Code postal"
    bind:value="{value.postcode}"
  />
</div>
<div class="f-input">
  <input
    type="text"
    disabled="{isLoading}"
    placeholder="Ville"
    bind:value="{value.city}"
  />
</div>
