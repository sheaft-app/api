<script lang="ts">
  import { page } from "@roxi/routify";
  import MenuEntry from "./MenuEntry.svelte";
  import MenuGroup from "./MenuGroup.svelte";
  import { isAuthenticated, userIsInRoles } from "$stores/auth";
  import {
    canDisplayEntry,
    canHighlightMenuItem,
    entryIsGroup,
    entryIsPage
  } from "$components/Nav/menus";

  export let entries = [];
  export let expand: boolean = false;
</script>

<menu class="nav-menu flex-grow" class:h-0="{!expand}" class:hidden="{!expand}">
  {#each Object.keys(entries) as key}
    {#if canDisplayEntry(entries[key], $isAuthenticated, userIsInRoles)}
      {#if entryIsGroup(entries[key])}
        <MenuGroup entry="{entries[key]}" />
      {:else if entryIsPage(entries[key])}
        <MenuEntry
          entry="{entries[key]}"
          canHighlight="{canHighlightMenuItem(entries[key], $page.path)}"
        />
      {/if}
    {/if}
  {/each}
</menu>
