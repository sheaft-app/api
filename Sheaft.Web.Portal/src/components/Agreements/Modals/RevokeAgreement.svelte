<script lang="ts">
  import { getContext, onMount } from "svelte";
  import Button from "$components/Button/Button.svelte";
  import type { IModalResult } from "$components/Modal/modal";
  import { ModalResult } from "$components/Modal/modal";
  import type { Components } from "$types/api";
  import { mediator } from "$components/mediator";
  import TextArea from "$components/TextArea/TextArea.svelte";
  import { AgreementOwner, AgreementStatus } from "$components/Agreements/enums";
  import { RevokeAgreementCommand } from "$components/Agreements/commands/revokeAgreement";

  export let agreement: Components.Schemas.AgreementDetailsDto;
  export let onClose: (result: IModalResult<string>) => {};

  const { close } = getContext("simple-modal");

  let title: string;
  let proposedBy: string;
  let actionStr: string;
  let shortActionStr: string;
  let reason: string;

  const validate = async () => {
    try {
      const result = await mediator.send(
        new RevokeAgreementCommand(agreement.id, reason)
      );
      close();
      await onClose(ModalResult.Success(result));
    } catch (exc) {
      console.error(exc);
    }
  };
</script>

<h2 class="mb-4">Révoquer l'accord commercial</h2>
<hr />
<div class="my-6">
  <p class="my-4">
    Vous vous apprêtez à révoquer l'accord commercial avec {agreement.target.name}.
  </p>
  <p class="my-4">
    Veuillez préciser la raison de cette annulation pour leur permettre de comprendre
    votre choix.
  </p>
  <TextArea bind:value="{reason}" />
  <p class="my-4">
    Si plus tard, vous souhaiterez commercer avec {agreement.target.name} à nouveau, vous pourrez
    renvoyer une demande d'accord commercial.
  </p>
</div>
<hr />
<div class="flex items-center justify-evenly pt-4">
  <Button class="bg-default-600" on:click="{close}">Fermer</Button>
  <Button
    class="bg-warning-600"
    on:click="{validate}"
    disabled="{!reason || reason.length < 1}">Valider</Button
  >
</div>
