<script lang="ts">
  import { page, goto } from "@roxi/routify";
  import { getContext, onMount } from "svelte";
  import Button from "$components/Button/Button.svelte";
  import { createForm } from "felte";
  import PageHeader from "$components/Page/PageHeader.svelte";
  import { getProductModule } from "$components/Products/module";
  import { mediator } from "$components/mediator";
  import { UpdateProductCommand } from "$components/Products/commands/updateProduct";
  import ConfirmRemoveProduct from "$components/Products/Modals/ConfirmRemoveProduct.svelte";
  import { GetProductQuery } from "$components/Products/queries/getProduct";
  import { ListReturnablesOptionsQuery } from "$components/Products/queries/listReturnablesOptions";
  import FormFooter from "$components/Form/FormFooter.svelte";
  import type { UpdateProductForm } from "$components/Products/types";
  import { getFormValidators } from "$components/validate";
  import Product from "$components/Products/Product.svelte";
  import { suite } from "$components/Products/validators";

  export let id: string;
  const module = getProductModule($goto);
  const { open } = getContext("simple-modal");

  let isLoading = false;
  let returnablesOptions: { label: string; value: string }[] = [];

  const onSubmit = async (values: UpdateProductForm): Promise<void> => {
    return await mediator.send(
      new UpdateProductCommand(
        id,
        values.name,
        values.unitPrice,
        values.vat,
        values.code,
        values.description,
        values.returnableId
      )
    );
  };

  const onSuccess = (): void => {
    module.goToList();
  };

  const { form, data, isSubmitting, setData } = createForm<UpdateProductForm>({
    onSubmit,
    onSuccess,
    extend: getFormValidators(suite)
  });

  const onClose = () => {
    module.goToList();
  };

  const confirmModal = () => {
    open(
      ConfirmRemoveProduct,
      {
        productId: id,
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

      const product = await mediator.send(new GetProductQuery(id));
      const productForm = <UpdateProductForm>product;
      productForm.hasReturnable = !!product.returnableId;
      setData(productForm);

      returnablesOptions = await mediator.send(new ListReturnablesOptionsQuery());

      isLoading = false;
    } catch (exc) {
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
<!-- routify:options title="Details du produit" -->

<PageHeader
  title="{$page.title}"
  actions="{actions}"
  previous="{() => module.goToList()}"
  class='max-w-xl'/>

<form use:form>
  <Product data="{data}" disabled="{controlsAreDisabled}" />
  <FormFooter>
    <Button
      type="button"
      disabled="{controlsAreDisabled}"
      class="back w-full mx-8"
      on:click="{module.goToList}"
      >Revenir à la liste
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
