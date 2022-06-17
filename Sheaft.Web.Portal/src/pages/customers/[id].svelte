<script lang='ts'>
  import AgreementDetails from "$pages/agreements/_AgreementDetails.svelte"
  import { getCustomerModule } from '$features/customers/module'
  import { goto } from '@roxi/routify'
  export let id = '';
  
  const module = getCustomerModule($goto);
  
</script>

<!-- routify:options index=true -->
<!-- routify:options title="Details de l'accord commercial" -->

<AgreementDetails {id} goHome='{() => module.goToCustomers()}'/>
