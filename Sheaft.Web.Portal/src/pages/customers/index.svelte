<script lang="ts">
  import { page, goto } from "@roxi/routify";
  import { onMount } from "svelte";
  import PageHeader from '$components/Page/PageHeader.svelte'
  import PageContent from '$components/Page/PageContent.svelte'
  import { getCustomerModule } from '$components/Customers/module'
  import type { Components } from '$types/api'
  import { mediator } from '$components/mediator'
  import { ListActiveAgreementsQuery } from '$components/Agreements/queries/listActiveAgreements'
  import Customers from '$components/Customers/Customers.svelte'

  export let pageNumber: number = 1,
    take: number = 10;

  const module = getCustomerModule($goto);

  let isLoading = true;
  let agreements: Components.Schemas.AgreementDto[] = [];

  onMount(async () => {
    try {
      isLoading = true;
      agreements = await mediator.send(new ListActiveAgreementsQuery(pageNumber, take));
      isLoading = false;
    } catch (exc) {
      module.goToHome();
    }
  });
  
  const actions = [
    {
      name:'Ajouter',
      disabled:false,
      visible: true,
      color:'primary',
      action: () => module.goToSearch()
    }
  ];
  
</script>

<!-- routify:options menu="Mes clients" -->
<!-- routify:options title="Mes clients" -->
<!-- routify:options index=1 -->
<!-- routify:options default=true -->

<PageHeader
  title={$page.title}
  actions={actions}
/>
<PageContent {isLoading}>
  <Customers customers='{agreements}' noResultsMessage='Aucun accord commercial actif'/>
</PageContent>
