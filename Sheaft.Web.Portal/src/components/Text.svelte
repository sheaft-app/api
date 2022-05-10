<script lang="ts">
  export let value: string = '';
  export let label: string = '';
  export let placeholder: string = '';
  export let className: string = '';
</script>

<div class="f-input">
  <input type="text" placeholder="{placeholder}" />
  <label>{label}</label>
</div>
