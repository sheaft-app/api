<script lang="ts">
  import { page, goto } from "@roxi/routify";
  import { onMount } from "svelte";
  import PageHeader from '$components/Page/PageHeader.svelte'
  import PageContent from '$components/Page/PageContent.svelte'
  import { dateDistance } from '$utils/dates'
  import { getCustomerModule } from '$components/Customers/module'
  import type { Components } from '$types/api'
  import { mediator } from '$components/mediator'
  import { formatInnerHtml } from "$actions/format"
  import { ListSentAgreementsQuery } from '$components/Agreements/queries/listSentAgreements'
  import { status } from '$components/Agreements/utils'
  import Customers from '$components/Customers/Customers.svelte'

  export let pageNumber: number = 1,
    take: number = 10;

  const module = getCustomerModule($goto);

  let isLoading = true;
  let agreements: Components.Schemas.AgreementDto[] = [];

  onMount(async () => {
    try {
      isLoading = true;
      agreements = await mediator.send(new ListSentAgreementsQuery(pageNumber, take));
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

<!-- routify:options menu="Demandes envoyées" -->
<!-- routify:options title="Demandes d'accord commercial envoyées" -->
<!-- routify:options index=2 -->

<PageHeader
  title={$page.title}
  subtitle="Toutes les demandes en attente d'acceptation par les magasins, sont listées ci-dessous"
  actions={actions}
  previous='{() => module.goToCustomers()}'
/>
<PageContent {isLoading}>
  <Customers customers='{agreements}' noResultsMessage='Aucun accord commercial envoyé en attente'/>
</PageContent>
