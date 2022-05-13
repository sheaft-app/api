<script lang="ts">
  import { isActive, goto, page } from "@roxi/routify";
  import { parseActivePath } from "$utils/path";

  export let entry: { title: string; path: string; pages: Array<any> } = null;
  export let canHighlight: boolean;

  $: isEntryActive =
    canHighlight &&
    ($isActive(parseActivePath(entry.path)) ||
      parseActivePath($page.path) == parseActivePath(entry.path));

  const navigate = path => {
    $goto(path);
  };
</script>

<li
  class="menu-entry"
  class:active="{isEntryActive}"
  on:click="{() => navigate(entry.path)}"
>
  <span>{entry.title}</span>
</li>

<style lang="scss">
  .menu-entry {
    cursor: pointer;
  }

  .menu-entry.active {
    @apply bg-blue-200;
  }
</style>
