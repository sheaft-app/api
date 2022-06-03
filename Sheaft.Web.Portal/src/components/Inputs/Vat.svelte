<script lang="ts">
  import "./inputs.scss";
  import { format, percent } from '$utils/format'

  export let value: number = 5.5;
  export let label: string | null = null;
  export let isLoading: boolean = false;
  export let required: boolean = true;
  export let rates: number[] = [0.055, 0.10, 0.20];
  
  const getRadioId = (rate:number) : string => {
    return (rate * 1000).toString();
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
            type='radio'
            bind:group='{value}'
            name='{getRadioId(rate)}'
            id='{getRadioId(rate)}'
            class='peer'
            value='{rate * 100}'
          />
          <label
            for='{getRadioId(rate)}'
            class='peer-checked:bg-primary-500 peer-checked:font-bold peer-checked:text-white hover:peer-checked:bg-primary-600'
          use:format={percent}>{rate}</label
          >
        </div>
      {/each}
  </div>
</div>
