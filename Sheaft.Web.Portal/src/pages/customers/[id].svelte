<script lang='ts'>
  import { goto } from '@roxi/routify'
  import { getCustomerModule } from '$components/Customers/module'
  import Agreement from '$components/Agreements/Agreement.svelte'

  export let id:string; 
  const module = getCustomerModule($goto);
  
</script>

<!-- routify:options index=true -->
<!-- routify:options title="Details de l'accord commercial" -->

<Agreement {id} goHome='{() => module.goToCustomers()}'/>
