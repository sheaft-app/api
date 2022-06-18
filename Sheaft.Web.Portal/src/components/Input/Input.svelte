<script lang="ts">
  import "./input.scss";
  import { nanoid } from "nanoid";
  import {setType} from './actions'

  export let id: string | undefined;
  export let name: string | undefined;
  export let type: string = "text";
  export let value: string | undefined;
  export let label: string | undefined;
  export let placeholder: string | undefined;
  export let required: boolean = true;
  export let disabled: boolean = false;

  if (!id) id = nanoid(10);
  else if(!name) name = id;
</script>

<div class="f-input">
  {#if label}
    <label for="{id}">{label} {required ? "*" : ""}</label>
  {/if}
  <input
    id="{id}"
    name="{id}"
    use:setType={type}
    disabled="{disabled}"
    placeholder="{placeholder}"
    bind:value
    class="{$$props.class}"
    aria-describedby="{id}-validation"/>
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
</div>
