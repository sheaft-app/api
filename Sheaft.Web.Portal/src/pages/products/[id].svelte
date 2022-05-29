<script lang="ts">
  import { page } from "@roxi/routify";
  import { onMount } from 'svelte'
  import { getProduct, product } from '$pages/products/service'

  export let id;
  let isLoading = true;
  
  onMount(async () => {
    isLoading = true;
    const result = await getProduct(id);
    if(!result.success){
      
    }
    
    isLoading = false;
  });
</script>

<!-- routify:options index=true -->
<!-- routify:options title="Details du produit" -->
<!-- routify:options roles=[] -->

<h1>{$page.title}</h1>
