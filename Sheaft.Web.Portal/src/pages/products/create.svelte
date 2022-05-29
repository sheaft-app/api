<script lang="ts">
  import { page, goto } from "@roxi/routify";
  import Text from "$components/Inputs/Text.svelte";
  import LongText from "$components/Inputs/LongText.svelte";
  import Price from "$components/Inputs/Price.svelte";
  import Vat from "$components/Inputs/Vat.svelte";
  import type { ICreateProduct } from "./types";
  import { round } from "$utils/number";
  import FormFooter from "$components/Form/FormFooter.svelte";
  import Form from "$components/Form/Form.svelte";
  import { create } from "./service";

  let isLoading = false;
  const product: ICreateProduct = {
    price: 0,
    name: "",
    vat: 0.0550
  };

  const cancelCreate = () => {
    $goto("/products/");
  };

  const createProduct = async () => {
    isLoading = true;
    const res = await create(product);
    if (res.success) {
      $goto(`/products/${res.data}`);
      return;
    }

    isLoading = false;
  };

  $: fullPrice = round(product.price * (1 + product.vat / 100));
</script>

<!-- routify:options menu="Créer" -->
<!-- routify:options index=2 -->
<!-- routify:options title="Ajouter un nouveau produit" -->
<!-- routify:options icon="fas#coffee" -->

<h1>{$page.title}</h1>

<Form class="mt-4">
  <Text
    label="Code"
    bind:value="{product.code}"
    required="{false}"
    maxLength="{30}"
    placeholder="Le code de votre produit (sera autogénéré si non renseigné)"
    isLoading="{isLoading}"
  />
  <Text
    label="Nom"
    bind:value="{product.name}"
    placeholder="Le nom de votre produit"
    isLoading="{isLoading}"
  />
  <Price
    label="Prix HT"
    bind:value="{product.price}"
    placeholder="Prix HT de votre produit en €"
    isLoading="{isLoading}"
  />
  <Vat label="TVA" bind:value="{product.vat}" isLoading="{isLoading}" />
  <Price
    label="Prix TTC (calculé)"
    value="{fullPrice}"
    disabled="{true}"
    isLoading="{isLoading}"
    required="{false}"
  />
  <LongText
    label="Description"
    bind:value="{product.description}"
    required="{false}"
    placeholder="Les ingrédients, la méthode de préparation, tout ce que vous pouvez juger utile de préciser"
    isLoading="{isLoading}"
  />
  <FormFooter
    submit="{createProduct}"
    submitText="Créer"
    cancel="{cancelCreate}"
    isLoading="{isLoading}"
  />
</Form>
