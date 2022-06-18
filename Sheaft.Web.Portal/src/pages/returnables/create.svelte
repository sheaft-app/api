<script lang='ts'>
  import { page, goto } from '@roxi/routify'
  import FormFooter from '$components/Form/FormFooter.svelte'
  import { createForm } from 'felte'
  import Button from '$components/Button/Button.svelte'
  import PageHeader from '$components/Page/PageHeader.svelte'
  import { calculateOnSalePrice } from '$utils/money'
  import { validator } from '@felte/validator-vest'
  import { suite } from '$pages/returnables/validators'
  import reporterDom from '@felte/reporter-dom'
  import type { Components } from '$types/api'
  import { mediator } from '$components/mediator'
  import { CreateReturnableCommand } from '$components/Returnables/commands/createReturnable'
  import Input from '$components/Input/Input.svelte'
  import Checkbox from '$components/Checkbox/Checkbox.svelte'
  import Vat from '$components/Vat/Vat.svelte'
  import { getReturnableModule } from '$components/Returnables/module'

  const module = getReturnableModule($goto)
  let hasVat = true;

  const { form, data, isSubmitting, isValid } =
    createForm<Components.Schemas.CreateReturnableRequest>({
      initialValues: {
        vat: 0
      },
      onSubmit: async values => {
        return await mediator.send(
          new CreateReturnableCommand(
            values.name,
            values.unitPrice,
            values.vat,
            values.code
          )
        )
      },
      onSuccess: (id: string) => {
        module.goToDetails(id)
      },
      extend: [
        <any>validator({ suite }),
        reporterDom({ single: true })
      ]
    })

  $: onSalePrice = calculateOnSalePrice($data.unitPrice, $data.vat)
  $: if(hasVat && !$data.vat){
    $data.vat = 5.5;
  }else if(!hasVat && $data.vat){
    $data.vat = undefined;
  }
</script>

<!-- routify:options index=true -->
<!-- routify:options title="Ajouter une nouvelle consigne" -->

<PageHeader
  title={$page.title}
  previous='{() => module.goToList()}'
/>

<form use:form>
  <Input
    id='name'
    label='Nom'
    bind:value='{$data.name}'
    placeholder='Le nom de votre consigne'
    disabled='{$isSubmitting}'
  />
  <Input
    id='unitPrice'
    label='Prix HT'
    bind:value='{$data.unitPrice}'
    placeholder='Prix HT de votre consigne en €'
    disabled='{$isSubmitting}'
  />
  <Checkbox
    id='hasVat'
    label='Je facture la TVA pour cette consigne'
    disabled='{$isSubmitting}'
    bind:value='{hasVat}'
    class='my-3'
  />
  {#if hasVat}
    <Vat
      id='vat'
      label='TVA'
      bind:value='{$data.vat}'
      disabled='{$isSubmitting}'
    />
    <Input
      id='onSalePrice'
      type='number'
      label='Prix TTC (calculé)'
      value='{onSalePrice}'
      disabled='{true}'
      required='{false}'
    />
  {/if}
  <FormFooter>
    <Button
      type='button'
      disabled='{$isSubmitting}'
      class='back w-full mx-8'
      on:click='{module.goToList}'
    >Revenir à la liste
    </Button>
    <Button
      type='submit'
      disabled='{$isSubmitting}'
      isLoading='{$isSubmitting}'
      class='accent w-full mx-8'
    >Créer
    </Button>
  </FormFooter>
</form>
