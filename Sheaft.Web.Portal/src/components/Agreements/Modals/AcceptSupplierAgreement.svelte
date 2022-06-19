<script lang='ts'>
  import { getContext } from 'svelte'
  import Button from '$components/Button/Button.svelte'
  import type { IModalResult } from '$components/Modal/modal'
  import { ModalResult } from '$components/Modal/modal'
  import type { Components } from '$types/api'
  import { mediator } from '$components/mediator'
  import { RefuseAgreementCommand } from '$components/Agreements/commands/refuseAgreement'

  export let agreement: Components.Schemas.AgreementDetailsDto
  export let onClose: (result: IModalResult<string>) => {}

  const { close } = getContext('simple-modal')
  
  let reason:string;

  const validate = async () => {
    try {
      const result = await mediator.send(new RefuseAgreementCommand(agreement.id))
      close()
      await onClose(ModalResult.Success(result))
    } catch (exc) {
      console.error(exc)
    }
  }
</script>

<h2 class='mb-4'>Accepter cette demande</h2>
<hr />
<div class='my-6'>
  <p>Vous vous apprêtez à accepter la demande d'accord commercial proposé par {agreement.supplier.name}. Vous pourrez après cette acceptation commander ses produits.</p>
</div>
<hr />
<div class='flex items-center justify-evenly pt-4'>
  <Button class='bg-default-600' on:click='{close}'>Fermer</Button>
  <Button class='bg-accent-600' on:click='{validate}'>Accepter</Button>
</div>
