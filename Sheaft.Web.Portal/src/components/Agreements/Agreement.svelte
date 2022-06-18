<script lang="ts">
  import { onMount } from "svelte";
  import type { Components } from "$types/api";
  import { mediator } from "$components/mediator";
  import { GetAgreementQuery } from "$components/Agreements/queries/getAgreement";
  import { AgreementStatus } from "$components/Agreements/enums";
  import PageHeader from "$components/Page/PageHeader.svelte";

  export let id: string;
  export let title: string = "Détails de l'accord commercial";
  export let goHome = () => {};
  export let previous = () => {
    history.back();
  };

  $: isLoading = false;
  let agreement: Components.Schemas.AgreementDto = null;

  onMount(async () => {
    try {
      isLoading = true;
      agreement = await mediator.send(new GetAgreementQuery(id));
      isLoading = false;
    } catch (exc) {
      goHome();
    }
  });

  $: canAcceptOrRefuse = agreement?.status == AgreementStatus.Pending;
  $: canCancel = agreement?.status == AgreementStatus.Active;

  $: actions = [
    {
      name: "Accepter",
      disabled: isLoading,
      visible: canAcceptOrRefuse,
      color: "success",
      action: () => {}
    },
    {
      name: "Refuser",
      disabled: isLoading,
      visible: canAcceptOrRefuse,
      color: "danger",
      action: () => {}
    },
    {
      name: "Annuler l'accord",
      disabled: isLoading,
      visible: canCancel,
      color: "warning",
      action: () => {}
    }
  ];
</script>

<PageHeader title="{title}" actions="{actions}" previous="{previous}" />
