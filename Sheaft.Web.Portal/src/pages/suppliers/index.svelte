<script lang="ts">
  import { page, goto } from "@roxi/routify";
  import { onMount } from "svelte";
  import PageHeader from '$components/Page/PageHeader.svelte'
  import PageContent from '$components/Page/PageContent.svelte'
  import type { Components } from '$types/api'
  import { mediator } from '$components/mediator'
  import { getSupplierModule } from '$components/Suppliers/module'
  import { ListActiveAgreementsQuery } from '$components/Agreements/queries/listActiveAgreements'
  import Suppliers from '$components/Suppliers/Suppliers.svelte'

  export let pageNumber: number = 1,
    take: number = 10;

  const module = getSupplierModule($goto);

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

<!-- routify:options menu="Mes fournisseurs" -->
<!-- routify:options title="Mes fournisseurs" -->
<!-- routify:options index=1 -->
<!-- routify:options default=true -->

<PageHeader
  title={$page.title}
  actions={actions}
/>
<PageContent {isLoading}>
  <Suppliers suppliers='{agreements}' noResultsMessage='Aucun accord commercial actif'/>
</PageContent>
