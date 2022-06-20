<script lang="ts">
  import { page, goto } from "@roxi/routify";
  import { getSupplierModule } from "$components/Suppliers/module";
  import Agreements from "$components/Agreements/Agreements.svelte";
  import { ProfileKind } from '$components/Account/enums'

  export let pageNumber: number = 1,
    take: number = 10;

  const module = getSupplierModule($goto);
</script>

<!-- routify:options menu="Mes fournisseurs" -->
<!-- routify:options title="Mes fournisseurs" -->
<!-- routify:options index=1 -->
<!-- routify:options default=true -->

<Agreements {module} {pageNumber} {take} title='{$page.title}' profileKind='{ProfileKind.Supplier}'/>
