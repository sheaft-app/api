<script lang='ts'>
  export let showAsterisk = true
</script>

<div class='{$$props.class}'>
  {#if showAsterisk}
    <small
    >Veuillez remplir tous les champs requis <b>(*)</b> pour ajouter un nouveau produit à votre
      catalogue</small
    >
  {/if}
  <div class='flex my-6'>
    <slot/>
  </div>
</div>
