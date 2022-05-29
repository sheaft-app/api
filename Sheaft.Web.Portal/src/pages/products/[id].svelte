<script lang="ts">
  import { page } from "@roxi/routify";
  import { onMount } from 'svelte'
  import { getProduct, product } from '$pages/products/service'

  export let id;
  
  onMount(async () => {
    await getProduct(id);
  });
</script>

<!-- routify:options index=true -->
<!-- routify:options title="Details du produit" -->
<!-- routify:options roles=[] -->

<h1>{$page.title}</h1>
