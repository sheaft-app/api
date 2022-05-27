<script lang='ts'>
  import './inputs.scss'
  import type { INamedAddress } from '$types/address'
  import Text from '$components/Inputs/Text.svelte'
  import Email from '$components/Inputs/Email.svelte'

  export let value: INamedAddress = {
    name: '',
    email: '',
    street: '',
    complement: '',
    postcode: '',
    city: ''
  }
  export let label: string = ''
  export let isLoading: boolean = false
  export let showName: boolean = true
  export let showEmail: boolean = true
  export let required: boolean = true

</script>

{#if showName}
  <Text
    disabled='{isLoading}'
    placeholder='Nom'
    bind:value='{value.name}'
    required='{required}'
  />
{/if}
{#if showEmail}
  <Email
    disabled='{isLoading}'
    placeholder='Email'
    bind:value='{value.email}'
  />
{/if}
<Text
  disabled='{isLoading}'
  placeholder='Adresse'
  bind:value='{value.street}'
  required='{required}'
/>
<Text
  disabled='{isLoading}'
  placeholder="Complément d'adresse"
  bind:value='{value.complement}'
/>
<Text
  disabled='{isLoading}'
  placeholder='Code postal'
  bind:value='{value.postcode}'
  required='{required}'
/>
<Text
  disabled='{isLoading}'
  placeholder='Ville'
  bind:value='{value.city}'
  required='{required}'
/>
