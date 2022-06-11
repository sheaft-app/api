<script lang='ts'>
  import './inputs.scss'
  import Text from '$components/Inputs/Text.svelte'
  import type { IAddress } from '/types/address'
  import { nanoid } from 'nanoid'

  export let id: string|null = null;
  export let value: IAddress | null = null
  export let label: string = ''
  export let isLoading: boolean = false
  export let required: boolean = true
  
  if(!id)
    id = nanoid(10);

  $:if(!value){ value = {street: null, postcode:null, city: null}; }
  
</script>

  <Text
    id='{id}_street'
    disabled='{isLoading}'
    placeholder='Adresse'
    bind:value='{value.street}'
    required='{required}'
  />
  <Text
    id='{id}_complement'
    type='text'
    disabled='{isLoading}'
    required='{false}'
    placeholder="Complément d'adresse"
    bind:value='{value.complement}'
  />
  <Text
    id='{id}_postcode'
    type='text'
    disabled='{isLoading}'
    placeholder='Code postal'
    bind:value='{value.postcode}'
    required='{required}'
  />
  <Text
    id='{id}_city'
    type='text'
    disabled='{isLoading}'
    placeholder='Ville'
    bind:value='{value.city}'
    required='{required}'
  />
