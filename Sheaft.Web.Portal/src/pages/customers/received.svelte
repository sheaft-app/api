<script lang="ts">
  import { page, goto } from "@roxi/routify";
  import { onMount } from "svelte";
  import PageHeader from "$components/Page/PageHeader.svelte";
  import PageContent from "$components/Page/PageContent.svelte";
  import { getCustomerModule } from "$components/Customers/module";
  import type { Components } from "$types/api";
  import { mediator } from "$components/mediator";
  import { ListReceivedAgreementsQuery } from "$components/Agreements/queries/listReceivedAgreements";
  import Agreements from "$components/Agreements/Agreements.svelte";

  export let pageNumber: number = 1,
    take: number = 10;

  const module = getCustomerModule($goto);

  let isLoading = true;
  let agreements: Components.Schemas.AgreementDto[] = [];

  onMount(async () => {
    try {
      isLoading = true;
      agreements = await mediator.send(new ListReceivedAgreementsQuery(pageNumber, take));
      isLoading = false;
    } catch (exc) {
      module.goToHome();
    }
  });
</script>

<!-- routify:options menu="Demandes reçues" -->
<!-- routify:options title="Demandes d'accord commercial reçues" -->
<!-- routify:options index=3 -->

<PageHeader
  title="{$page.title}"
  subtitle="Toutes les demandes en attente de votre acceptation, sont listées ci-dessous"
  previous="{() => module.goToCustomers()}"
/>
<PageContent isLoading="{isLoading}">
  <Agreements
    agreements="{agreements}"
    module="{module}"
    noResultsMessage="Aucun accord commercial reçu en attente"
  />
</PageContent>
