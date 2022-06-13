<script lang="ts">
  import { page, goto } from "@roxi/routify";
  import { onMount } from "svelte";
  import Text from "$components/Inputs/Text.svelte";
  import Price from "$components/Inputs/Price.svelte";
  import FormFooter from "$components/Form/FormFooter.svelte";
  import { createForm } from "felte";
  import { calculateOnSalePrice } from "$utils/price";
  import Vat from "$components/Inputs/Vat.svelte";
  import { mediator } from "$services/mediator";
  import { UpdateReturnableRequest } from "$commands/returnables/updateReturnable";
  import { getReturnableModule } from "$pages/returnables/module";
  import { GetReturnableQuery } from "$queries/returnables/getReturnable";
  import type { Components } from "$types/api";
  import Button from "$components/Buttons/Button.svelte";
  import PageHeader from '$components/Page/PageHeader.svelte'

  export let id = "";
  let initializing = true;
  const module = getReturnableModule($goto);

  const { form, data, isSubmitting, isValid, setData } =
    createForm<Components.Schemas.UpdateReturnableRequest>({
      onSubmit: async values => {
        await mediator.send(
          new UpdateReturnableRequest(
            id,
            values.name,
            values.unitPrice,
            values.vat,
            values.code
          )
        );
      },
      onSuccess: () => {
        module.goToList();
      }
    });

  onMount(async () => {
    initializing = true;
    try {
      const returnable = await mediator.send(new GetReturnableQuery(id));
      setData(returnable);
      initializing = false;
    } catch (exc) {
      console.error(exc);
      module.goToList();
    }
  });

  const actions = [
    {
      name:'Supprimer',
      disabled:false,
      color:'danger',
      action: () => {}
    }
  ];

  $: onSalePrice = calculateOnSalePrice($data.unitPrice, $data.vat);
  $: isLoading = isSubmitting || initializing;
</script>

<!-- routify:options index=true -->
<!-- routify:options title="Details de la consigne" -->
<!-- routify:options roles=[] -->

<PageHeader
  title={$page.title}
  actions={actions}
/>

<form use:form>
  <Text
    id="code"
    label="Code"
    bind:value="{$data.code}"
    required="{false}"
    maxLength="{30}"
    placeholder="Le code de votre produit (autogénéré si non renseigné)"
    disabled="{$isLoading}"
  />
  <Text
    id="name"
    label="Nom"
    bind:value="{$data.name}"
    placeholder="Le nom de votre produit"
    disabled="{$isLoading}"
  />
  <Price
    id="unitPrice"
    label="Prix HT"
    bind:value="{$data.unitPrice}"
    placeholder="Prix HT de votre produit en €"
    disabled="{$isLoading}"
  />
  <Vat
    id="vat"
    label="TVA"
    bind:value="{$data.vat}"
    disabled="{$isLoading}"
    rates="{[0, 0.055, 0.1, 0.2]}"
  />
  <Price
    id="onSalePrice"
    label="Prix TTC (calculé)"
    value="{onSalePrice}"
    disabled="{true}"
    required="{false}"
  />
  <FormFooter>
    <Button
      type="button"
      disabled="{$isLoading}"
      class="back w-full mx-8"
      on:click="{module.goToList}"
      >Annuler
    </Button>
    <Button
      type="submit"
      disabled="{!$isValid || $isLoading}"
      isLoading="{$isLoading}"
      class="accent w-full mx-8"
      >Sauvegarder
    </Button>
  </FormFooter>
</form>
