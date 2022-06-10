<script lang='ts'>
  import Text from '$components/Inputs/Text.svelte'
  import Siret from '$components/Inputs/Siret.svelte'
  import Address from '$components/Inputs/Address.svelte'
  import { createForm } from 'felte'
  import FormFooter from '$components/Form/FormFooter.svelte'
  import Button from '$components/Buttons/Button.svelte'

  export let initialValues
  export let onSubmit
  export let onBack

  const { form, data, isSubmitting } = createForm({ initialValues, onSubmit })

</script>

<h2>Information légales</h2>
<form use:form>
  <Text
    id='corporateName'
    isLoading='{$isSubmitting}'
    bind:value='{$data.corporateName}'
    placeholder='Nom légal'
  />
  <Siret
    id='siret'
    isLoading='{$isSubmitting}'
    bind:value='{$data.siret}'
    placeholder='Votre numéro de SIRET'
  />
  <Address 
    id='address'
    isLoading='{$isSubmitting}' 
    bind:value='{$data.legalAddress}' />
  
  <FormFooter>
    <Button
      class='back w-full mx-8'
      disabled='{$isSubmitting}'
      type="button" on:click="{() => onBack($data)}">
      Précédent
    </Button>
    <Button
      class='accent w-full mx-8'
      disabled='{$isSubmitting}'
      type="submit">Suivant</Button>
  </FormFooter>
</form>
