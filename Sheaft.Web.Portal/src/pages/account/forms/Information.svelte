<script lang='ts'>
  import Select from '$components/Inputs/Select.svelte'
  import Text from '$components/Inputs/Text.svelte'
  import Email from '$components/Inputs/Email.svelte'
  import Phone from '$components/Inputs/Phone.svelte'
  import { ProfileKind } from '$enums/profile'
  import { createForm } from 'felte'
  import FormFooter from '$components/Form/FormFooter.svelte'
  import Button from '$components/Buttons/Button.svelte'
  import type { IAccountInformation } from '$commands/account/configureAccount'
  import type { ISelectOption } from '$components/Inputs/types/select'
  import Siret from '$components/Inputs/Siret.svelte'

  export let initialValues: IAccountInformation | null
  export let onSubmit
  export let onBack

  const { form, data, isSubmitting } = createForm<IAccountInformation>({ initialValues, onSubmit })

  const accountTypeOptions: ISelectOption[] = [
    { label: 'Producteur', value: ProfileKind.Supplier },
    { label: 'Commerçant', value: ProfileKind.Customer }
  ]

</script>

<h2>Informations générales</h2>
<form use:form>
  <Select
    id='accountType'
    options='{accountTypeOptions}'
    isLoading='{$isSubmitting}'
    bind:value='{$data.accountType}'
  />
  <Text
    id='tradeName'
    isLoading='{$isSubmitting}'
    bind:value='{$data.tradeName}'
    placeholder='Nom commercial'
  />
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
  <Email
    id='email'
    isLoading='{$isSubmitting}'
    bind:value='{$data.email}'
    placeholder='Adresse mail de contact'
  />
  <Phone
    id='phone'
    isLoading='{$isSubmitting}'
    bind:value='{$data.phone}'
    placeholder='Numéro de téléphone de contact'
  />
  <FormFooter>
    <Button
      class='back w-full mx-8'
      disabled='{$isSubmitting}'
      type='button' on:click='{() => onBack($data)}'>
      Annuler
    </Button>
    <Button
      class='accent w-full mx-8'
      disabled='{$isSubmitting}'
      type='submit'>Suivant
    </Button>
  </FormFooter>
</form>
