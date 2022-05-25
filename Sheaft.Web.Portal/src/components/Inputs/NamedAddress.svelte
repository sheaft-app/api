<script lang='ts'>
  import './inputs.scss'
  import type { INamedAddress } from '$types/address'

  export let value: INamedAddress = { name: '', email: '', street: '', complement: '', postcode: '', city: '' }
  export let label: string = ''
  export let isLoading: boolean = false
  export let showName: boolean = true
  export let showEmail: boolean = true
</script>

{#if showName}
  <div class='f-input'>
    <input
      type='text'
      disabled='{isLoading}'
      placeholder='Nom'
      bind:value='{value.name}'
    />
  </div>
{/if}
{#if showEmail}
  <div class='f-input'>
    <input
      type='email'
      disabled='{isLoading}'
      placeholder='Email'
      bind:value='{value.email}'
    />
  </div>
{/if}
<div class='f-input'>
  <input
    type='text'
    disabled='{isLoading}'
    placeholder='Adresse'
    bind:value='{value.street}'
  />
</div>
<div class='f-input'>
  <input
    type='text'
    disabled='{isLoading}'
    placeholder="Complément d'adresse"
    bind:value='{value.complement}'
  />
</div>
<div class='f-input'>
  <input
    type='text'
    disabled='{isLoading}'
    placeholder='Code postal'
    bind:value='{value.postcode}'
  />
</div>
<div class='f-input'>
  <input
    type='text'
    disabled='{isLoading}'
    placeholder='Ville'
    bind:value='{value.city}'
  />
</div>
