<script lang="ts">
  import { page, goto } from "@roxi/routify";
  import FormFooter from "$components/Form/FormFooter.svelte";
  import { createForm } from "felte";
  import Button from "$components/Button/Button.svelte";
  import PageHeader from "$components/Page/PageHeader.svelte";
  import { mediator } from "$components/mediator";
  import { CreateReturnableCommand } from "$components/Returnables/commands/createReturnable";
  import { getReturnableModule } from "$components/Returnables/module";
  import type { CreateReturnableForm } from "$components/Returnables/types";
  import { getFormValidators } from "$components/validate";
  import Returnable from "$components/Returnables/Returnable.svelte";
  import { suite } from "$components/Returnables/validators";

  const module = getReturnableModule($goto);

  const onSubmit = async (values: CreateReturnableForm): Promise<string> => {
    return await mediator.send(
      new CreateReturnableCommand(values.name, values.unitPrice, values.vat, values.code)
    );
  };

  const onSuccess = (id: string): void => {
    module.goToDetails(id);
  };

  const { form, data, isSubmitting } = createForm<CreateReturnableForm>({
    initialValues: {
      vat: 0
    },
    onSubmit,
    onSuccess,
    extend: getFormValidators(suite)
  });
</script>

<!-- routify:options index=true -->
<!-- routify:options title="Ajouter une nouvelle consigne" -->

<PageHeader title="{$page.title}" previous="{() => module.goToList()}" />

<form use:form>
  <Returnable data="{data}" disabled="{$isSubmitting}" />
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
