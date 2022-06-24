<script lang="ts">
  import { page, goto } from "@roxi/routify";
  import FormFooter from "$components/Form/FormFooter.svelte";
  import { createForm } from "felte";
  import Button from "$components/Button/Button.svelte";
  import PageHeader from "$components/Page/PageHeader.svelte";
  import { mediator } from "$components/mediator";
  import { CreateBatchCommand } from "$components/Batches/commands/createBatch";
  import { getBatchModule } from "$components/Batches/module";
  import type { BatchForm } from "$components/Batches/types";
  import { getFormValidators } from "$components/validate";
  import Batch from "$components/Batches/Batch.svelte";
  import { suite } from "$components/Batches/validators";

  const module = getBatchModule($goto);

  const onSubmit = async (values: BatchForm): Promise<string> => {
    return await mediator.send(new CreateBatchCommand(values.number, values.kind, values.expirationDate, values.productionDate)
    );
  };

  const onSuccess = (id: string): void => {
    module.goToDetails(id);
  };

  const { form, data, isSubmitting } = createForm<BatchForm>({
    onSubmit,
    onSuccess,
    extend: getFormValidators(suite)
  });
</script>

<!-- routify:options index=true -->
<!-- routify:options title="Ajouter un nouveau lot" -->

<PageHeader 
  title="{$page.title}" 
  previous="{() => module.goToList()}"
  class='max-w-xl'/>

<form use:form>
  <Batch data="{data}" disabled="{$isSubmitting}" />
  <FormFooter>
    <Button
      type="button"
      disabled="{$isSubmitting}"
      class="back w-full mx-8"
      on:click="{module.goToList}"
      >Revenir à la liste
    </Button>
    <Button
      type="submit"
      disabled="{$isSubmitting}"
      isLoading="{$isSubmitting}"
      class="accent w-full mx-8"
      >Créer
    </Button>
  </FormFooter>
</form>
