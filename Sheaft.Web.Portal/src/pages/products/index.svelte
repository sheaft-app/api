<script lang="ts">
  import { page } from "@roxi/routify";
  import { onMount } from 'svelte'
  import { listProducts } from '$pages/products/service'

  onMount(async () => {
    await listProducts(1, 10);
  });
</script>

<!-- routify:options menu="Mes produits" -->
<!-- routify:options title="Mes produits" -->
<!-- routify:options roles=[] -->
<!-- routify:options index=1 -->
<!-- routify:options default=true -->

<h1>{$page.title}</h1>
