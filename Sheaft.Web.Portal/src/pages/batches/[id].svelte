<script lang="ts">
  import { page, goto } from "@roxi/routify";
  import { getContext, onMount } from "svelte";
  import FormFooter from "$components/Form/FormFooter.svelte";
  import { createForm } from "felte";
  import Button from "$components/Button/Button.svelte";
  import PageHeader from "$components/Page/PageHeader.svelte";
  import { mediator } from "$components/mediator";
  import { UpdateBatchCommand } from "$components/Batches/commands/updateBatch";
  import ConfirmRemoveBatch from "$components/Batches/Modals/ConfirmRemoveBatch.svelte";
  import { GetBatchQuery } from "$components/Batches/queries/getBatch";
  import { getBatchModule } from "$components/Batches/module";
  import { getFormValidators } from "$components/validate";
  import type { BatchForm } from "$components/Batches/types";
  import Batch from "$components/Batches/Batch.svelte";
  import { suite } from "$components/Batches/validators";
  import { dateStr } from '$utils/dates'

  export let id: string;

  const module = getBatchModule($goto);
  const { open } = getContext("simple-modal");

  let isLoading = true;

  const onSubmit = async (values: BatchForm): Promise<void> => {
    return await mediator.send(
      new UpdateBatchCommand(id, values.number, values.kind, values.expirationDate, values.productionDate)
    );
  };

  const onSuccess = (): void => {
    module.goToList();
  };

  const { form, data, isSubmitting, setData } = createForm<BatchForm>({
    onSubmit,
    onSuccess,
    extend: getFormValidators(suite)
  });

  const onClose = () => {
    module.goToList();
  };

  const confirmModal = () => {
    open(
      ConfirmRemoveBatch,
      {
        batchId: id,
        onClose
      },
      {
        closeButton: false,
        closeOnEsc: true,
        closeOnOuterClick: true
      }
    );
  };

  onMount(async () => {
    try {
      isLoading = true;
      const batch = await mediator.send(new GetBatchQuery(id));
      const batchForm = <BatchForm>batch;
      batchForm.expirationDate = dateStr(new Date(batch.expirationDate), "yyyy-MM-dd");
      batchForm.productionDate = batch.productionDate ? dateStr(new Date(batch.productionDate), "yyyy-MM-dd") : null;
      setData(batchForm);
      isLoading = false;
    } catch (exc) {
      console.error(exc);
      module.goToList();
    }
  });

  $: actions = [
    {
      name: "Supprimer",
      disabled: controlsAreDisabled,
      visible: true,
      color: "danger",
      action: () => confirmModal()
    }
  ];

  $: controlsAreDisabled = isLoading || $isSubmitting;
</script>

<!-- routify:options index=true -->
<!-- routify:options title="Details du lot" -->

<PageHeader
  title="{$page.title}"
  actions="{actions}"
  previous="{() => module.goToList()}"
  class='max-w-xl'/>

<form use:form>
  <Batch data="{data}" disabled="{controlsAreDisabled}" />
  <FormFooter>
    <Button
      type="button"
      disabled="{controlsAreDisabled}"
      class="back w-full mx-8"
      on:click="{module.goToList}"
      >Annuler
    </Button>
    <Button
      type="submit"
      disabled="{controlsAreDisabled}"
      isLoading="{$isSubmitting}"
      class="accent w-full mx-8"
      >Sauvegarder
    </Button>
  </FormFooter>
</form>
