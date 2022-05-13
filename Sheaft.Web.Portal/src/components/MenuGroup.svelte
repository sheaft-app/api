<script lang="ts">
  import { isActive } from "@roxi/routify";
  import Menu from "$components/Menu.svelte";
  import { parseActivePath, parseSubActivePath } from '$utils/path'

  export let entry: { title: string; path: string; pages: Array<any> } = null;

  let isGroupActive: boolean = false;

  $: if (entry.pages && entry.pages.length > 0) {
    isGroupActive = entry.pages.find(p => $isActive(parseSubActivePath(p.path))) != null;
  }

  $: isVisible = entry.visible && entry.pages.find(p => p.visible) != null;
</script>

{#if isVisible}
  <li class="menu-group" class:active="{isGroupActive}">
    <span>{entry.title}</span>
    <Menu entries="{entry.pages}" />
  </li>
{/if}

<style lang="scss">
  .menu-group.active {
    @apply bg-gray-200;
  }
</style>
