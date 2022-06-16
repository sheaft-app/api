<script lang="ts">
  import Text from "$components/Inputs/Text.svelte";
  import { nanoid } from "nanoid";
  import { onMount } from "svelte";
  import type { IAddress } from './types'

  export let id: string | null = null;
  export let value: IAddress | undefined;
  export let label: string = "";
  export let disabled: boolean = false;
  export let required: boolean = true;

  if (!id) id = nanoid(10);

  onMount(() => {
    checkValue();
  });

  const checkValue = () => {
    if (value) return;

    value = { street: null, postcode: null, city: null };
  };
</script>

{#if value}
  <fieldset class="my-4">
    {#if label}
      <label class="block mb-2 text-sm font-medium text-gray-700 "
        >{label} {required ? "*" : ""}</label
      >
    {/if}

    <Text
      id="{id}.street"
      disabled="{disabled}"
      placeholder="Adresse"
      bind:value="{value.street}"
      required="{required}"
    />
    <Text
      id="{id}.complement"
      type="text"
      disabled="{disabled}"
      required="{false}"
      placeholder="Complément d'adresse"
      bind:value="{value.complement}"
    />
    <Text
      id="{id}.postcode"
      type="text"
      disabled="{disabled}"
      placeholder="Code postal"
      bind:value="{value.postcode}"
      required="{required}"
    />
    <Text
      id="{id}.city"
      type="text"
      disabled="{disabled}"
      placeholder="Ville"
      bind:value="{value.city}"
      required="{required}"
    />
  </fieldset>
{/if}
