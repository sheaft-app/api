<script lang="ts">
  import "./inputs.scss";

  export let value: string = "";
  export let label: string = "";
  export let placeholder: string = "";
  export let isLoading: boolean = false
</script>

<div class="f-input">
  <input type="text" disabled={isLoading} placeholder="{placeholder}" bind:value class={$$props.class} />
  <label>{label}</label>
</div>
