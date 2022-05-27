<script lang='ts'>
  import Button from '$components/Buttons/Button.svelte'

  export let showAsterisk = true
  export let isLoading = false
  export let disabled = false

  export let submitText = 'Valider'
  export let submit = () => {
  }

  export let cancelText = 'Annuler'
  export let cancel = () => {
  }
</script>

{#if showAsterisk}
  <small>Veuillez remplir tous les champs requis <b>(*)</b> pour ajouter un nouveau produit à votre catalogue</small>
{/if}
<div class='flex my-6'>
  {#if cancelText}
    <Button
      type='button'
      disabled='{disabled || isLoading}'
      class='back w-full mx-8'
      on:click={cancel}
    >{cancelText}
    </Button>
  {/if}
  <Button
    type='submit'
    {disabled}
    {isLoading}
    class='accent w-full mx-8'
    on:click={submit}
  >{submitText}
  </Button>
</div>
