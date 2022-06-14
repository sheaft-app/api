<script lang="ts">
  import "./inputs.scss";
  import { percent } from '$utils/percent'
  import {formatInnerHtml} from '$directives/format'

  export let id: string | null = null;
  export let value: number | null = null;
  export let label: string | null = null;
  export let disabled: boolean = false;
  export let required: boolean = true;
  export let rates: number[] = [0.055, 0.1, 0.2];

  $: getValue = rate => {
    return rate * 100;
  };
</script>

<fieldset class="my-4">
  {#if label}
    <label class="block mb-2 text-sm font-medium text-gray-700 "
      >{label} {required ? "*" : ""}</label
    >
  {/if}
  <div class="flex">
    {#each rates as rate}
      <div class="f-radio">
        <input
          class="peer"
          type="radio"
          bind:group="{value}"
          id="{id}-{rate}"
          value="{getValue(rate)}"
          disabled='{disabled}'
        />
        <label
          class="peer-checked:bg-primary-500 peer-checked:font-bold peer-checked:text-white hover:peer-checked:bg-primary-600"
          for="{id}-{rate}"
          use:formatInnerHtml="{percent}">{rate}</label
        >
      </div>
    {/each}
  </div>
</fieldset>
