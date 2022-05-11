<script lang="ts">
  export let value: string = '';
  export let label: string = '';
  export let placeholder: string = 'Votre mot de passe';
  export let className: string = '';
</script>

<div class="f-input">
  <input type="password" placeholder="{placeholder}" bind:value class="{className}" />
  <label>{label}</label>
</div>
