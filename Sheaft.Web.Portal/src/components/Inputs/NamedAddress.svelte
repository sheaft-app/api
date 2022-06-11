<script lang='ts'>
  import './inputs.scss'
  import Text from '$components/Inputs/Text.svelte'
  import Email from '$components/Inputs/Email.svelte'
  import type { INamedAddress } from '/types/address'
  import { nanoid } from 'nanoid'

  export let id: string|null = null;
  export let value: INamedAddress | null = null
  export let label: string = ''
  export let isLoading: boolean = false
  export let showName: boolean = true
  export let showEmail: boolean = true
  export let required: boolean = true

  if(!id)
    id = nanoid(10);
  
  $:if(!value){ value = { name:null, email:null, street: null, postcode: null, city: null }; }
</script>

{#if showName}
  <Text
    id='{id}_name'
    disabled='{isLoading}'
    placeholder='Nom'
    bind:value='{value.name}'
    required='{required}'
  />
{/if}
{#if showEmail}
  <Email
    id='{id}_email'
    disabled='{isLoading}' 
    placeholder='Email' 
    bind:value='{value.email}' />
{/if}
<Text
  id='{id}_street'
  disabled='{isLoading}'
  placeholder='Adresse'
  bind:value='{value.street}'
  required='{required}'
/>
<Text
  id='{id}_complement'
  disabled='{isLoading}'
  placeholder="Complément d'adresse"
  bind:value='{value.complement}'
  required='{false}'
/>
<Text
  id='{id}_postcode'
  disabled='{isLoading}'
  placeholder='Code postal'
  bind:value='{value.postcode}'
  required='{required}'
/>
<Text
  id='{id}_city'
  disabled='{isLoading}'
  placeholder='Ville'
  bind:value='{value.city}'
  required='{required}'
/>
