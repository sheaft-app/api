<script lang="ts">
  import "./inputs.scss"
  
  export let value: string = "";
  export let label: string = "";
  export let placeholder: string = "Votre adresse mail";
  export let className: string = "";
</script>

<div class="f-input">
  <input type="email" placeholder="{placeholder}" bind:value class="{className}" />
  <label>{label}</label>
</div>
