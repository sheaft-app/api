<script lang="ts">
  import "./inputs.scss";
  import Text from "$components/Inputs/Text.svelte";
  import Email from "$components/Inputs/Email.svelte";
  import type { INamedAddress } from "/types/address";
  import { nanoid } from "nanoid";
  import { onMount } from "svelte";

  export let id: string | null = null;
  export let value: INamedAddress | undefined;
  export let label: string = "";
  export let disabled: boolean = false;
  export let showName: boolean = true;
  export let showEmail: boolean = true;
  export let required: boolean = true;

  if (!id) id = nanoid(10);

  onMount(() => {
    checkValue();
  });

  const checkValue = () => {
    if (value) return;

    value = { name: null, email: null, street: null, postcode: null, city: null };
  };
</script>

{#if value}
  <fieldset class="my-4">
    {#if label}
      <label class="block mb-2 text-sm font-medium text-gray-700 "
        >{label} {required ? "*" : ""}</label
      >
    {/if}
    <fieldset class:mb-3="{showName || showEmail}">
      {#if showName}
        <Text
          id="{id}.name"
          disabled="{disabled}"
          placeholder="Destinataire/Emetteur"
          bind:value="{value.name}"
          required="{required}"
        />
      {/if}
      {#if showEmail}
        <Email
          id="{id}.email"
          disabled="{disabled}"
          placeholder="Mail de contact"
          bind:value="{value.email}"
        />
      {/if}
    </fieldset>
    <fieldset>
      <Text
        id="{id}.street"
        disabled="{disabled}"
        placeholder="Adresse"
        bind:value="{value.street}"
        required="{required}"
      />
      <Text
        id="{id}.complement"
        disabled="{disabled}"
        placeholder="Complément d'adresse"
        bind:value="{value.complement}"
        required="{false}"
      />
      <Text
        id="{id}.postcode"
        disabled="{disabled}"
        placeholder="Code postal"
        bind:value="{value.postcode}"
        required="{required}"
      />
      <Text
        id="{id}.city"
        disabled="{disabled}"
        placeholder="Ville"
        bind:value="{value.city}"
        required="{required}"
      />
    </fieldset>
  </fieldset>
{/if}
