<script lang="ts">
  import { nanoid } from "nanoid";
  import { onMount } from "svelte";
  import Input from '$components/Input/Input.svelte'
  import type { NamedAddress } from '$types/address'

  export let id: string | undefined;
  export let value: NamedAddress | undefined;
  export let label: string | undefined;
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
        <Input
          id="{id}.name"
          disabled="{disabled}"
          placeholder="Destinataire/Emetteur"
          bind:value="{value.name}"
          required="{required}"
        />
      {/if}
      {#if showEmail}
        <Input
          id="{id}.email"
          type='email'
          disabled="{disabled}"
          placeholder="Mail de contact"
          bind:value="{value.email}"
        />
      {/if}
    </fieldset>
    <fieldset>
      <Input
        id="{id}.street"
        disabled="{disabled}"
        placeholder="Adresse"
        bind:value="{value.street}"
        required="{required}"
      />
      <Input
        id="{id}.complement"
        disabled="{disabled}"
        placeholder="Complément d'adresse"
        bind:value="{value.complement}"
        required="{false}"
      />
      <Input
        id="{id}.postcode"
        disabled="{disabled}"
        placeholder="Code postal"
        bind:value="{value.postcode}"
        required="{required}"
      />
      <Input
        id="{id}.city"
        disabled="{disabled}"
        placeholder="Ville"
        bind:value="{value.city}"
        required="{required}"
      />
    </fieldset>
  </fieldset>
{/if}
