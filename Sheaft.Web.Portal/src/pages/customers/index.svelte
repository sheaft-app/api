<script lang='ts'>
  import { page, goto } from '@roxi/routify'
  import { getCustomerModule } from '$components/Customers/module'
  import Agreements from '$components/Agreements/Agreements.svelte'
  import { ProfileKind } from '$components/Account/enums'

  export let pageNumber: number = 1,
    take: number = 10

  const module = getCustomerModule($goto)
</script>

<!-- routify:options menu="Mes clients" -->
<!-- routify:options title="Mes clients" -->
<!-- routify:options index=1 -->
<!-- routify:options default=true -->

<Agreements {module} {pageNumber} {take} title='{$page.title}' profileKind='{ProfileKind.Customer}'/>
