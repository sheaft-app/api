<script lang="ts">
  import { page, goto } from "@roxi/routify";
  import { onMount } from "svelte";
  import Button from "$components/Button/Button.svelte";
  import PageHeader from "$components/Page/PageHeader.svelte";
  import { getProductModule } from "$components/Products/module";
  import { mediator } from "$components/mediator";
  import { CreateProductCommand } from "$components/Products/commands/createProduct";
  import { ListReturnablesOptionsQuery } from "$components/Products/queries/listReturnablesOptions";
  import FormFooter from "$components/Form/FormFooter.svelte";
  import type { CreateProductForm } from "$components/Products/types";
  import { createForm } from "felte";
  import { getFormValidators } from "$components/validate";
  import Product from "$components/Products/Product.svelte";
  import { suite } from "$components/Products/validators";

  const module = getProductModule($goto);

  let isLoading = false;
  let returnablesOptions: { label: string; value: string }[] = [];

  const onSubmit = async (values: CreateProductForm): Promise<string> => {
    return await mediator.send(
      new CreateProductCommand(
        values.name,
        values.unitPrice,
        values.vat,
        values.code,
        values.description,
        values.returnableId
      )
    );
  };

  const onSuccess = (id: string) => {
    module.goToDetails(id);
  };

  const { form, data, isSubmitting } = createForm<CreateProductForm>({
    initialValues: {
      vat: 5.5
    },
    onSubmit,
    onSuccess,
    extend: getFormValidators(suite)
  });

  onMount(async () => {
    try {
      isLoading = true;
      returnablesOptions = await mediator.send(new ListReturnablesOptionsQuery());
      isLoading = false;
    } catch (exc) {
      module.goToList();
    }
  });
</script>

<!-- routify:options index=true -->
<!-- routify:options title="Ajouter un nouveau produit" -->

<PageHeader title="{$page.title}" previous="{() => module.goToList()}" />

<form use:form>
  <Product data="{data}" disabled="{$isSubmitting}" />
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
