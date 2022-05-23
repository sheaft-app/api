<script lang="ts">
  import "./inputs.scss";

  export let value: string = "";
  export let label: string = "";
  export let placeholder: string = "Votre mot de passe";
  export let isLoading: boolean = false
</script>

<div class="f-input">
  <label>{label}</label>
  <input type="password" disabled={isLoading} placeholder="{placeholder}" bind:value class={$$props.class} />
</div>
