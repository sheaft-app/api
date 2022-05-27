<script lang="ts">
  import { page, goto } from "@roxi/routify";
  import Text from "$components/Inputs/Text.svelte";
  import LongText from '$components/Inputs/LongText.svelte'
  import Price from '$components/Inputs/Price.svelte'
  import Vat from '$components/Inputs/Vat.svelte'
  import Number from '$components/Inputs/Number.svelte'
  import type { ICreateProduct } from '$types/product'
  import { round } from '$utils/number'
  import FormFooter from '$components/Form/FormFooter.svelte'
  import Form from '$components/Form/Form.svelte'
  import { create } from '$stores/products'
  
  let isLoading = false;
  const product: ICreateProduct = {
    price: 0,
    name: '',
    vat: 0.055    
  };
  
  const cancelCreate = () => {
    $goto('/products/')
  }
  
  const createProduct = async () => {
    isLoading = true;
    const res = await create(product);
    if(res.success) {
      $goto(`/products/${res.data}`);
      return;
    }

    isLoading = false;    
  }
  
  $: fullPrice = round(product.price * (1+product.vat/1000));
</script>

<!-- routify:options menu="Créer" -->
<!-- routify:options index=2 -->
<!-- routify:options title="Ajouter un nouveau produit" -->
<!-- routify:options icon="fas#coffee" -->

<h1>{$page.title}</h1>

<Form class='mt-4'>
  <Text label='Code' bind:value={product.code} required={false} maxLength={30} placeholder='Le code de votre produit (sera autogénéré si non renseigné)' {isLoading}/>
  <Text label='Nom' bind:value={product.name} placeholder='Le nom de votre produit' {isLoading}/>
  <Price label='Prix HT' bind:value={product.price} placeholder='Prix HT de votre produit en €' {isLoading}/>
  <Vat label='TVA' bind:value={product.vat} {isLoading}/>
  <Number label='Prix TTC (calculé)' value={fullPrice} disabled='{true}' {isLoading} required={false}/>
  <LongText label='Description' bind:value={product.description} required='{false}' placeholder='Les ingrédients, la méthode de préparation, tout ce que vous pouvez juger utile de préciser' {isLoading}/>
  <FormFooter submit={createProduct} submitText='Créer' cancel='{cancelCreate}' {isLoading}/>
</Form>
