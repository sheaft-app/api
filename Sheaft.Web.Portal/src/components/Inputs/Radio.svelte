<script lang="ts">
  import "./inputs.scss";
  import { nanoid } from 'nanoid'

  export let id: string = nanoid(10);
  export let group: any = null;
  export let value: number|null = null;
  export let label: string | null = null;
  export let isLoading: boolean = false;
  export let required: boolean = true; 
  
</script>

<div class='f-radio'>
  <input
    class='peer'
    type='radio'
    bind:group={group}
    id="{id}"
    value={value}
  />
  <label
    class='peer-checked:bg-primary-500 peer-checked:font-bold peer-checked:text-white hover:peer-checked:bg-primary-600'
    for="{id}">{label}</label
  >
</div>
