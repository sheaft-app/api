<script lang="ts">
  import { page, goto } from "@roxi/routify";
  import PageHeader from "$components/Page/PageHeader.svelte";
  import { getProductModule } from "$components/Products/module";

  const module = getProductModule($goto);

  let isLoading = false;

</script>

<!-- routify:options index=true -->
<!-- routify:options title="Brouillon" -->

<PageHeader 
  title="{$page.title}" 
  previous="{() => module.goToList()}"
  class='max-w-xl'/>
