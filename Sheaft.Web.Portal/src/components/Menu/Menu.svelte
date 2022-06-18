<script lang="ts">
  import "./menu.scss";
  import { page } from "@roxi/routify";
  import MenuEntry from "./MenuEntry.svelte";
  import MenuGroup from "./MenuGroup.svelte";
  import { authStore } from "$components/Account/store";
  import {
    canDisplayMenuElement,
    canHighlightMenuItem,
    entryIsGroup,
    entryIsPage
  } from "$components/Menu/menus";
  import type { MenuElement } from "$components/Menu/types";

  export let entries: MenuElement[] = [];
  export let expand: boolean = false;

  const isPage = (entry: MenuElement): boolean => {
    if (entryIsPage(entry)) return !(<MenuEntry>entry).default;

    return false;
  };
</script>

<menu class="nav-menu flex-grow" class:h-0="{!expand}" class:hidden="{!expand}">
  {#each Object.keys(entries) as key}
    {#if canDisplayMenuElement(entries[key], $authStore.isAuthenticated, authStore.userIsInRoles)}
      {#if entryIsGroup(entries[key])}
        <MenuGroup entry="{entries[key]}" />
      {:else if isPage(entries[key])}
        <MenuEntry
          entry="{entries[key]}"
          canHighlight="{canHighlightMenuItem(entries[key], $page.path)}"
        />
      {/if}
    {/if}
  {/each}
</menu>
