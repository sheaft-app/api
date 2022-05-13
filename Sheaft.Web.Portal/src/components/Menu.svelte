<script lang='ts'>
  import MenuEntry from '$components/MenuEntry.svelte'
  import MenuGroup from '$components/MenuGroup.svelte'

  export let entries = []

</script>

<menu class='nav-menu'>
  {#each Object.keys(entries) as key}
    {#if entries[key].visible}
      {#if entries[key].pages && entries[key].pages.length > 0}
        <MenuGroup entry={entries[key]} />
      {:else if entries[key].path && entries[key].path.length > 0 }
        <MenuEntry entry={entries[key]} />
      {/if}
    {/if}
  {/each}
</menu>
