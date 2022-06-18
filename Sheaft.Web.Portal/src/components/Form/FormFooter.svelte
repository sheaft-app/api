<script lang="ts">
  export let showAsterisk:boolean = true;
</script>

<div class="{$$props.class}">
  {#if showAsterisk}
    <small>Veuillez remplir tous les champs requis <b>(*)</b></small>
  {/if}
  <div class="flex my-6">
    <slot />
  </div>
</div>
