<script lang="ts">
  import logo from "$assets/svelte.png";
  
  export let href: string = "/";
  export let width: number = 80;
  
</script>

<a href="{href}" class="flex items-center justify-center">
  <img src="{logo}" width="{width}" alt="logo" class="m-4" />
</a>
