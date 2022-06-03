<script lang="ts">
  import { page, goto } from "@roxi/routify";
  import Text from "$components/Inputs/Text.svelte";
  import Price from "$components/Inputs/Price.svelte";
  import Vat from "$components/Inputs/Vat.svelte";
  import { round } from "$utils/number";
  import FormFooter from "$components/Form/FormFooter.svelte";
  import Form from "$components/Form/Form.svelte";
  import { create } from "./service";
  import type { Paths } from '$types/api'

  let isLoading = false;
  const returnable: Paths.CreateReturnable.RequestBody = {
    unitPrice: 0,
    name: "",
    vat: 0.0550,
  };

  const cancelCreate = () => {
    $goto("/returnables/");
  };

  const createReturnable = async () => {
    isLoading = true;
    const res = await create(returnable);
    if (res.success) {
      $goto(`/returnables/${res.data}`);
      return;
    }

    isLoading = false;
  };

  $: fullPrice = round(returnable.unitPrice * (1 + returnable.vat / 100));
</script>

<!-- routify:options menu="Créer" -->
<!-- routify:options index=2 -->
<!-- routify:options title="Ajouter une nouvelle consigne" -->
<!-- routify:options icon="fas#coffee" -->

<svelte:head>
  <title>{$page.title}</title>
</svelte:head>

<h1>{$page.title}</h1>

<Form class="mt-4 ">
  <Text
    label="Code"
    bind:value="{returnable.code}"
    required="{false}"
    maxLength="{30}"
    placeholder="Le code de votre consigne (sera autogénéré si non renseigné)"
    isLoading="{isLoading}"
  />
  <Text
    label="Nom"
    bind:value="{returnable.name}"
    placeholder="Le nom de votre consigne"
    isLoading="{isLoading}"
  />
  <Price
    label="Prix HT"
    bind:value="{returnable.unitPrice}"
    placeholder="Prix HT de votre consigne en €"
    isLoading="{isLoading}"
  />
  <Vat label="TVA" bind:value="{returnable.vat}" isLoading="{isLoading}" rates='{[0, 0.055, 0.10, 0.20]}' />
  <Price
    label="Prix TTC (calculé)"
    value="{fullPrice}"
    disabled="{true}"
    isLoading="{isLoading}"
    required="{false}"
  />
  <FormFooter
    submit="{createReturnable}"
    submitText="Créer"
    cancel="{cancelCreate}"
    isLoading="{isLoading}"
  />
</Form>
