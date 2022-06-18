<script lang='ts'>
  import { getContext } from 'svelte'
  import Button from '$components/Button/Button.svelte'
  import { address } from '$utils/address'
  import type { IModalResult } from '$components/Modal/modal'
  import { ModalResult } from '$components/Modal/modal'
  import type { Components } from '$types/api'
  import { mediator } from '$components/mediator'
  import { ProposeAgreementToSupplierCommand } from '$components/Suppliers/commands/proposeAgreementToSupplier'

  export let supplier: Components.Schemas.AvailableSupplierDto
  export let onClose: (result: IModalResult<string>) => {};

  const { close } = getContext('simple-modal')

  const validate = async () => {
    try {
      const result = await mediator.send(new ProposeAgreementToSupplierCommand(supplier.id))
      close();
      await onClose(ModalResult.Success(result));
    } catch (exc) {
      console.error(exc)
    }
  }
</script>
<h2 class='mb-4'>Nouvelle relation commerciale</h2>
<hr/>
<div class='my-6'>
  <p>Vous allez envoyer une demande de mise en relation commerciale avec le producteur suivant :</p>
  <div class='mx-3 my-5'>
    <p><b>{supplier.name}</b></p>
    <p>{@html address(supplier.shippingAddress)}</p></div>
    <p class='mt-4'>Ce producteur assignera des jours de livraison en fonction de ses disponibilités au moment d'accepter votre demande</p>
</div>
<hr/>
<div class='flex items-center justify-evenly pt-4'>
  <Button class='bg-back-600 mx-2' on:click={close}>Annuler</Button>
  <Button class='bg-accent-600 mx-2' on:click={validate}>Ajouter</Button>
</div>
