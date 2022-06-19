<script lang='ts'>
  import { getContext, onMount } from 'svelte'
  import Button from '$components/Button/Button.svelte'
  import type { IModalResult } from '$components/Modal/modal'
  import { ModalResult } from '$components/Modal/modal'
  import type { Components } from '$types/api'
  import { mediator } from '$components/mediator'
  import { RefuseAgreementCommand } from '$components/Agreements/commands/refuseAgreement'
  import TextArea from '$components/TextArea/TextArea.svelte'
  import { AgreementOwner, AgreementStatus } from '$components/Agreements/enums'
  import { CancelAgreementCommand } from '$components/Agreements/commands/cancelAgreement'

  export let agreement: Components.Schemas.AgreementDetailsDto
  export let onClose: (result: IModalResult<string>) => {}

  const { close } = getContext('simple-modal')
  
  let title:string;
  let proposedBy:string;
  let actionStr:string;
  let shortActionStr:string;
  let reason:string;

  const validate = async () => {
    try {
      const result = await mediator.send(new CancelAgreementCommand(agreement.id))
      close()
      await onClose(ModalResult.Success(result))
    } catch (exc) {
      console.error(exc)
    }
  }
  
  onMount(() => {
    if(agreement.status == AgreementStatus.Pending){
      title = "Annuler votre demande";
    }
    else{
      title = "Annuler cet accord commercial";      
    }
    
    if(agreement.owner == AgreementOwner.Customer) {
      proposedBy = agreement.status == AgreementStatus.Pending ? `envoyé à ${agreement.customer.name}` : `avec ${agreement.customer.name}`;
      actionStr = agreement.status == AgreementStatus.Pending ? "Il ne pourra donc pas commander vos produits" : "Il ne pourra donc plus commander vos produits";
      shortActionStr = "lui vendre vos";
    }
    else{
      proposedBy = agreement.status == AgreementStatus.Pending ? `envoyé à ${agreement.supplier.name}` : `avec ${agreement.supplier.name}`;
      actionStr = agreement.status == AgreementStatus.Pending ? "Vous ne pourrez donc pas commander ses produits" : "Vous ne pourrez donc plus commander ses produits";
      shortActionStr = "lui acheter des";
    }
  })
</script>

<h2 class='mb-4'>{title}</h2>
<hr />
<div class='my-6'>
  <p class='my-4'>Vous vous apprêtez à annuler la demande d'accord commercial {proposedBy}. {actionStr}.</p>
  <p class='my-4'>Veuillez préciser la raison de cette annulation pour permettre à {proposedBy} de comprendre ce choix.</p>
  <TextArea bind:value={reason}/>
  <p class='my-4'>Malgré tout, si vous souhaitez finalement {shortActionStr} produits à nouveau, vous pourrez lui renvoyer une demande.</p>
</div>
<hr />
<div class='flex items-center justify-evenly pt-4'>
  <Button class='bg-default-600' on:click='{close}'>Fermer</Button>
  <Button class='bg-warning-600' on:click='{validate}' disabled='{!reason || reason.length < 1}'>Revoquer</Button>
</div>
