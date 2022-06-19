<script lang='ts'>
  import { getContext, onMount } from 'svelte'
  import Button from '$components/Button/Button.svelte'
  import type { IModalResult } from '$components/Modal/modal'
  import { ModalResult } from '$components/Modal/modal'
  import type { Components } from '$types/api'
  import { mediator } from '$components/mediator'
  import { RefuseAgreementCommand } from '$components/Agreements/commands/refuseAgreement'
  import TextArea from '$components/TextArea/TextArea.svelte'
  import { AgreementOwner } from '$components/Agreements/enums'

  export let agreement: Components.Schemas.AgreementDetailsDto
  export let onClose: (result: IModalResult<string>) => {}

  const { close } = getContext('simple-modal')
  
  let proposedBy:string;
  let actionStr:string;
  let shortActionStr:string;
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
  
  onMount(() => {
    if(agreement.owner == AgreementOwner.Customer) {
      proposedBy = agreement.customer.name;
      actionStr = "Il ne pourra donc pas commander vos produits";
      shortActionStr = "lui vendre vos";
    }
    else{
      proposedBy = agreement.supplier.name;
      actionStr = "Vous ne pourrez donc pas commander ses produits";
      shortActionStr = "lui acheter des";
    }
  })
</script>

<h2 class='mb-4'>Refuser cette demande</h2>
<hr />
<div class='my-6'>
  <p>Vous vous apprêtez à refuser la demande d'accord commercial proposé par {proposedBy}. {actionStr}.</p>
  <p>Veuillez préciser la raison de ce refus pour permettre à {proposedBy} de comprendre ce choix.</p>
  <TextArea bind:value={reason}/>
  <p>Malgré tout, si vous souhaitez finalement {shortActionStr} produits plus tard, vous pourrez lui renvoyer une demande.</p>
</div>
<hr />
<div class='flex items-center justify-evenly pt-4'>
  <Button class='bg-default-600' on:click='{close}'>Fermer</Button>
  <Button class='bg-danger-600' on:click='{validate}' disabled='{!reason || reason.length < 1}'>Refuser</Button>
</div>
