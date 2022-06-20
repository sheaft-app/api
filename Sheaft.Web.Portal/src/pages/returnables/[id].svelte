<script lang="ts">
  import { page, goto } from "@roxi/routify";
  import { getContext, onMount } from "svelte";
  import FormFooter from "$components/Form/FormFooter.svelte";
  import { createForm } from "felte";
  import Button from "$components/Button/Button.svelte";
  import PageHeader from "$components/Page/PageHeader.svelte";
  import { mediator } from "$components/mediator";
  import { UpdateReturnableCommand } from "$components/Returnables/commands/updateReturnable";
  import ConfirmRemoveReturnable from "$components/Returnables/Modals/ConfirmRemoveReturnable.svelte";
  import { GetReturnableQuery } from "$components/Returnables/queries/getReturnable";
  import { getReturnableModule } from "$components/Returnables/module";
  import { getFormValidators } from "$components/validate";
  import type { UpdateReturnableForm } from "$components/Returnables/types";
  import Returnable from "$components/Returnables/Returnable.svelte";
  import { suite } from "$components/Returnables/validators";

  export let id: string;

  const module = getReturnableModule($goto);
  const { open } = getContext("simple-modal");

  let isLoading = true;

  const onSubmit = async (values: UpdateReturnableForm): Promise<void> => {
    return await mediator.send(
      new UpdateReturnableCommand(id, values.name, values.unitPrice, values.vat)
    );
  };

  const onSuccess = (): void => {
    module.goToList();
  };

  const { form, data, isSubmitting, setData } = createForm<UpdateReturnableForm>({
    onSubmit,
    onSuccess,
    extend: getFormValidators(suite)
  });

  const onClose = () => {
    module.goToList();
  };

  const confirmModal = () => {
    open(
      ConfirmRemoveReturnable,
      {
        returnableId: id,
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
      const returnable = await mediator.send(new GetReturnableQuery(id));
      const returnableForm = <UpdateReturnableForm>returnable;
      returnableForm.hasVat = returnable.vat > 0;
      setData(returnableForm);
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
<!-- routify:options title="Details de la consigne" -->

<PageHeader
  title="{$page.title}"
  actions="{actions}"
  previous="{() => module.goToList()}"
  class='max-w-xl'/>

<form use:form>
  <Returnable data="{data}" disabled="{controlsAreDisabled}" />
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
