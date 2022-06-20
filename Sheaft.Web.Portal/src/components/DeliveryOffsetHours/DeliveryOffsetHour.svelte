<script lang='ts'>
  import Checkbox from '$components/Checkbox/Checkbox.svelte'
  import Input from '$components/Input/Input.svelte'
  import { nanoid } from 'nanoid'

  export let id:string|undefined;
  export let toggleId:string|undefined;
  export let name:string|undefined;
  export let label: string = 'Verrouiller la prise de commandes x heures avant le jour de livraison'
  export let offset: number = 0;
  export let offsetEnabled: boolean = false;
  export let disabled: boolean = false;
  
  if (!id) id = nanoid(10);
  else if (!name) name = id;
  
  if(!toggleId)
    toggleId = `${id}Enabled`
    
</script>

<div class="mt-4">
  <Checkbox
    id='{toggleId}'
    bind:value="{offsetEnabled}"
    disabled='{disabled}'
    {label} />
  {#if offsetEnabled}
    <Input
      id={id}
      name={name}
      type="number"
      label="Nombre d'heures"
      disabled='{disabled}'
      bind:value="{offset}"
      class="mt-2" />
  {/if}
</div>
