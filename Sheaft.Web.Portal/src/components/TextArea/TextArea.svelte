<script lang="ts">
  import { nanoid } from "nanoid";
  import "./textarea.scss";

  export let id: string | undefined;
  export let name: string | undefined;
  export let value: string | undefined;
  export let label: string | undefined;
  export let placeholder: string | undefined;
  export let required: boolean = true;
  export let disabled: boolean = false;

  if (!id) id = nanoid(10);
  else if (!name) name = id;
</script>

{#if label}
  <label for="{id}">{label} {required ? "*" : ""}</label>
{/if}
<textarea
  id="{id}"
  name="{name}"
  bind:value
  disabled="{disabled}"
  placeholder="{placeholder}"
  class="{$$props.class}"
  required="{required}"
  aria-describedby="{id}-validation"></textarea>
<div id="{id}-validation" class="validation-reporter" data-felte-reporter-dom-for="{id}">
</div>
<div
  id="{id}-warning"
  class="warning-reporter"
  data-felte-reporter-dom-for="{id}"
  data-felte-reporter-dom-level="warning">
</div>
