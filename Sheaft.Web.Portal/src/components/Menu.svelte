<script lang='ts'>
  import { page } from '@roxi/routify'
  import MenuEntry from '$components/MenuEntry.svelte'
  import MenuGroup from '$components/MenuGroup.svelte'

  export let entries = []

  const canHighlightMenuItem = (entry, currentPath): boolean => {
    let existingPage = false
    if (entry.pages && entry.pages.length > 0) {
      existingPage = entry.pages.find(p => p.visible && p.path == currentPath) != null
    }

    return currentPath == entry.path || existingPage
  }

</script>

<menu class='nav-menu'>
  {#each Object.keys(entries) as key}
    {#if entries[key].visible}
      {#if entries[key].pages && entries[key].pages.length > 0}
        <MenuGroup entry={entries[key]} canHighlight={canHighlightMenuItem(entries[key], $page.path)} />
      {:else if entries[key].path && entries[key].path.length > 0 }
        <MenuEntry entry={entries[key]} canHighlight={canHighlightMenuItem(entries[key], $page.path)} />
      {/if}
    {/if}
  {/each}
</menu>
