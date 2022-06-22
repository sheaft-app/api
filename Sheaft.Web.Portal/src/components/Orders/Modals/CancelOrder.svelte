<script lang="ts">
  import { getContext } from "svelte";
  import Button from "$components/Button/Button.svelte";
  import { type IModalResult, ModalResult } from "$components/Modal/modal";
  import type { Components } from "$types/api";
  import { mediator } from "$components/mediator";
  import { CancelOrderCommand } from "$components/Orders/commands/cancelOrder";
  import TextArea from '$components/TextArea/TextArea.svelte'
  import { authStore } from '$components/Account/store'
  import { ProfileKind } from '$components/Account/enums'

  export let order: Components.Schemas.OrderDetailsDto;
  export let onClose: (result: IModalResult<string>) => {};

  const { close } = getContext("simple-modal");
  const targetName = $authStore.account?.profile?.kind == ProfileKind.Supplier ? order.customer.name : order.supplier.name;

  let reason: string;

  const validate = async () => {
    try {
      const result = await mediator.send(new CancelOrderCommand(order.id, reason));
      close();
      await onClose(ModalResult.Success(result));
    } catch (exc) {
      console.error(exc);
    }
  };
</script>

<h2 class="mb-4">Annuler la commande n°{order.code}</h2>
<hr />
<div class="my-6">
  <p class="my-4">
    Vous vous apprêtez à annuler la commande de {targetName}.
  </p>
  <p class="my-4">
    Veuillez préciser la raison de cette annulation pour permettre à {targetName} de comprendre votre choix.
  </p>
  <TextArea bind:value="{reason}" />
</div>
<hr />
<div class="flex items-center justify-evenly pt-4">
  <Button class="bg-default-600" on:click="{close}">Fermer</Button>
  <Button class="bg-warning-600" on:click="{validate}">Valider</Button>
</div>
