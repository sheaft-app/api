<script lang="ts">
  import "./vat.scss";
  import { percent } from '$utils/percent'
  import {formatInnerHtml} from '$components/Actions/format'
  import { nanoid } from 'nanoid'

  export let id: string | undefined;
  export let value: number | undefined;
  export let label: string | undefined;
  export let disabled: boolean = false;
  export let required: boolean = true;
  export let rates: number[] = [0.055, 0.1, 0.2];

  if (!id) id = nanoid(10);
  
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
  <div 
    id='{id}'
    class="flex"
    aria-describedby="{id}-validation">
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
  <div
    id="{id}-validation"
    class='validation-reporter'
    data-felte-reporter-dom-for="{id}"
  ></div>
  <div
    id="{id}-warning"
    class='warning-reporter'
    data-felte-reporter-dom-for="{id}"
    data-felte-reporter-dom-level="warning"
  ></div>
</fieldset>
