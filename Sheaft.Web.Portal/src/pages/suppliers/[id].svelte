<script lang="ts">
  import { goto } from "@roxi/routify";
  import { getSupplierModule } from "$components/Suppliers/module";
  import Agreement from "$components/Agreements/Agreement.svelte";

  export let id: string;
  const module = getSupplierModule($goto);
</script>

<!-- routify:options index=true -->
<!-- routify:options title="Details de l'accord commercial" -->

<Agreement id="{id}" {module} />
