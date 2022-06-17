<script lang='ts'>
  import AgreementDetails from "$pages/agreements/_AgreementDetails.svelte"
  import { goto } from '@roxi/routify'
  import { getSupplierModule } from '$features/suppliers/module'
  export let id = '';

  const module = getSupplierModule($goto);

</script>

<!-- routify:options index=true -->
<!-- routify:options title="Details de l'accord commercial" -->

<AgreementDetails {id} goHome='{() => module.goToSuppliers()}'/>
