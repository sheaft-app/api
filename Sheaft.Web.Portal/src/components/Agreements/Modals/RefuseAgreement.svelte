<script lang="ts">
  import { getContext } from "svelte";
  import Button from "$components/Button/Button.svelte";
  import { type IModalResult, ModalResult } from "$components/Modal/modal";
  import type { Components } from "$types/api";
  import { mediator } from "$components/mediator";
  import { RefuseAgreementCommand } from "$components/Agreements/commands/refuseAgreement";
  import TextArea from "$components/TextArea/TextArea.svelte";

  export let agreement: Components.Schemas.AgreementDetailsDto;
  export let onClose: (result: IModalResult<string>) => {};

  const { close } = getContext("simple-modal");

  let reason: string;

  const validate = async () => {
    try {
      const result = await mediator.send(
        new RefuseAgreementCommand(agreement.id, reason)
      );
      close();
      await onClose(ModalResult.Success(result));
    } catch (exc) {
      console.error(exc);
    }
  };
</script>

<h2 class="mb-4">Refuser cette demande</h2>
<hr />
<div class="my-6">
  <p class="my-4">
    Vous vous apprêtez à refuser la demande d'accord commercial envoyée par {agreement
      .target.name}.
  </p>
  <p class="my-4">
    Veuillez préciser la raison de ce refus pour leur permettre de comprendre votre choix.
  </p>
  <TextArea bind:value="{reason}" />
  <p class="my-4">
    Si vous souhaitez finalement commercer avec eux, vous pourrez renvoyer une demande
    d'accord commercial.
  </p>
</div>
<hr />
<div class="flex items-center justify-evenly pt-4">
  <Button class="bg-default-600" on:click="{close}">Fermer</Button>
  <Button
    class="bg-danger-600"
    on:click="{validate}"
    disabled="{!reason || reason.length < 1}">Valider</Button>
</div>
