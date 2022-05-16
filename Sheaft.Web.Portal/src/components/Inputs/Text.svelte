<script lang="ts">
  import "./inputs.scss";

  export let value: string = "";
  export let label: string = "";
  export let placeholder: string = "";
  export let className: string = "";
</script>

<div class="f-input">
  <input type="text" placeholder="{placeholder}" bind:value class="{className}" />
  <label>{label}</label>
</div>
