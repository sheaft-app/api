<script lang="ts">
  import { getContext } from "svelte";
  import Button from "$components/Button/Button.svelte";
  import { type IModalResult, ModalResult } from "$components/Modal/modal";
  import { mediator } from "$components/mediator";
  import { RemoveBatchCommand } from '$components/Batches/commands/removeBatch'

  export let batchId: string;
  export let onClose: (result: IModalResult<string>) => {};

  const { close } = getContext("simple-modal");

  const validate = async () => {
    try {
      const result = await mediator.send(new RemoveBatchCommand(batchId));
      close();
      await onClose(ModalResult.Success(result));
    } catch (exc) {
      console.error(exc);
    }
  };
</script>

<div class="text-center my-6">
  <p>Êtes-vous sûr de vouloir supprimer ce numéro de lot ?</p>
</div>
<div class="flex items-center justify-evenly pt-4">
  <Button class="bg-default-600" on:click="{close}">Non</Button>
  <Button class="bg-danger-600" on:click="{validate}">Oui</Button>
</div>
