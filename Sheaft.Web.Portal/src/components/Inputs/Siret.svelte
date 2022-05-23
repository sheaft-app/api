<script lang="ts">
  import "./inputs.scss";

  export let value: string = "";
  export let label: string = "";
  export let placeholder: string = "";
  export let isLoading: boolean = false
</script>

<div class="f-input">
  <label>{label}</label>
  <input type="text" disabled={isLoading} placeholder="{placeholder}" bind:value class={$$props.class} pattern='[0-9]' maxlength='14' minlength='14'/>
</div>
