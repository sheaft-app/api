<script lang="ts">
  import { page } from "@roxi/routify";
  import MenuEntry from "./MenuEntry.svelte";
  import MenuGroup from "./MenuGroup.svelte";

  export let entries = [];
  export let expand:boolean = false;

  const canHighlightMenuItem = (entry, currentPath): boolean => {
    if (currentPath == entry.path) return true;

    if (entry.parent)
      //should highlight current menu item => /sub only if /sub/1 is found but not visible in menu
      return entry.parent.pages.find(p => !p.visible && p.path == currentPath) != null;
  };
</script>

<menu class="nav-menu flex-grow" 
      class:h-0='{!expand}'
      class:hidden='{!expand}'>
  {#each Object.keys(entries) as key}
    {#if entries[key].visible}
      {#if entries[key].pages && entries[key].pages.length > 0}
        <MenuGroup
          entry="{entries[key]}"
          canHighlight="{canHighlightMenuItem(entries[key], $page.path)}"
        />
      {:else if entries[key].path && entries[key].path.length > 0}
        <MenuEntry
          entry="{entries[key]}"
          canHighlight="{canHighlightMenuItem(entries[key], $page.path)}"
        />
      {/if}
    {/if}
  {/each}
</menu>
