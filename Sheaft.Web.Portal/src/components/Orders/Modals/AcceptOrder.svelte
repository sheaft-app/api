<script lang="ts">
  import { getContext } from 'svelte'
  import Button from '$components/Button/Button.svelte'
  import { type IModalResult, ModalResult } from '$components/Modal/modal'
  import type { Components } from '$types/api'
  import { mediator } from '$components/mediator'
  import { AcceptOrderCommand } from '$components/Orders/commands/acceptOrder'
  import { authStore } from '$components/Account/store'
  import { ProfileKind } from '$components/Account/enums'

  export let order: Components.Schemas.OrderDetailsDto;
  export let onClose: (result: IModalResult<string>) => {};

  const { close } = getContext("simple-modal");
  const targetName = $authStore.account?.profile?.kind == ProfileKind.Supplier ? order.customer.name : order.supplier.name;
  
  let reason: string;

  const validate = async () => {
    try {
      const result = await mediator.send(new AcceptOrderCommand(order.id));
      close();
      await onClose(ModalResult.Success(result));
    } catch (exc) {
      console.error(exc);
    }
  };
  
</script>

<h2 class="mb-4">Accepter la commande n°{order.code}</h2>
<hr />
<div class="my-6">
  <p class="my-4">
    Vous vous apprêtez à accepter la commande de {targetName}.
  </p>
</div>
<hr />
<div class="flex items-center justify-evenly pt-4">
  <Button class="bg-default-600" on:click="{close}">Fermer</Button>
  <Button class="bg-success-600" on:click="{validate}">Valider</Button>
</div>
