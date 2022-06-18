<script lang="ts">
  import Input from "$components/Input/Input.svelte";
  import Checkbox from "$components/Checkbox/Checkbox.svelte";
  import Vat from "$components/Vat/Vat.svelte";
  import { calculateOnSalePrice } from "$utils/money";
  import type { KeyedWritable } from "@felte/common";
  import type { CreateProductForm, UpdateProductForm } from "$components/Products/types";
  import Select from "$components/Select/Select.svelte";
  import TextArea from "$components/TextArea/TextArea.svelte";

  export let data: KeyedWritable<CreateProductForm | UpdateProductForm>;
  export let disabled: boolean;

  $: onSalePrice = calculateOnSalePrice($data.unitPrice, $data.vat);
</script>

<Input
  id="code"
  label="Code"
  bind:value="{$data.code}"
  required="{false}"
  maxLength="{30}"
  placeholder="Le code de votre produit (sera autogénéré si non renseigné)"
  disabled="{disabled}"
/>
<Input
  id="name"
  label="Nom"
  bind:value="{$data.name}"
  placeholder="Le nom de votre produit"
  disabled="{disabled}"
/>
<Input
  id="unitPrice"
  label="Prix HT"
  bind:value="{$data.unitPrice}"
  placeholder="Prix HT de votre produit en €"
  disabled="{disabled}"
/>
<Vat id="vat" label="TVA" bind:value="{$data.vat}" disabled="{disabled}" />
<Input
  type="number"
  label="Prix TTC (calculé)"
  value="{onSalePrice}"
  disabled="{true}"
  required="{false}"
/>
<TextArea
  id="description"
  label="Description"
  bind:value="{$data.description}"
  placeholder="Les ingrédients, la méthode de préparation, tout ce que vous pouvez juger utile de préciser"
  disabled="{disabled}"
/>
<Checkbox
  id="hasReturnable"
  label="Ce produit est consigné"
  disabled="{disabled}"
  bind:value="{$data.hasReturnable}"
  class="mt-3 mb-6"
/>
{#if $data.hasReturnable}
  <Select
    id="returnableId"
    label="Consigne"
    options="{returnablesOptions}"
    disabled="{disabled}"
    bind:value="{$data.returnableId}"
  />
{/if}
