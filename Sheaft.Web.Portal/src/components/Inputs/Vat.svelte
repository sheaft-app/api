<script lang="ts">
  import "./inputs.scss";
  import { format, percent } from '$utils/format'

  export let id:string|null=null;
  export let value: string|null = null;
  export let label: string|null = null;
  export let isLoading: boolean = false;
  export let required: boolean = true;
  export let rates: number[] = [0.055, 0.10, 0.20];  
      
  $: getValue = (rate) => {
    return rate * 100;
  }
  
</script>

<div class="my-4">
  <label class="block mb-2 text-sm font-medium text-gray-700 "
    >{label} {required ? "*" : ""}</label
  >
  <div class="flex">
      {#each rates as rate}
        <div class='f-radio'>
          <input
            class='peer'
            type='radio'
            bind:group={value}
            id="{id}-{rate}"
            value={getValue(rate)}
          />
          <label
            class='peer-checked:bg-primary-500 peer-checked:font-bold peer-checked:text-white hover:peer-checked:bg-primary-600'
            for="{id}-{rate}"
            use:format={percent}>{rate}</label
          >
        </div>
      {/each}
  </div>
</div>
