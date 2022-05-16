<script lang="ts">
  import { isActive, goto, page } from "@roxi/routify";
  import { parseActivePath } from "./path";
  import Fa from "svelte-fa";
  import { getFaIcon, getFaIconFromFullName } from "$utils/faIcon";

  export let entry: {
    title: string;
    path: string;
    pages: Array<any>;
    parent: any;
    icon: string;
  } = null;
  
  export let canHighlight: boolean;

  $: isEntryActive =
    canHighlight &&
    ($isActive(parseActivePath(entry.path)) ||
      parseActivePath($page.path) == parseActivePath(entry.path));

  const navigate = (path: string) => {
    $goto(path);
  };
</script>

<li
  class="menu-entry cursor-pointer mx-4 p-3 rounded-xl"
  class:active="{isEntryActive}"
  class:has-parent="{entry.parent}"
  on:click="{() => navigate(entry.path)}"
>
  <span class:flex="{!entry.parent}" class:items-center="{!entry.parent}">
    {#if !entry.parent}
      {#if entry.icon}
        <span class="menu-icon">
          <Fa icon="{getFaIconFromFullName(entry.icon)}" class="menu-icon" />
        </span>
      {:else}
        <span class="menu-icon">
          <Fa icon="{getFaIcon('fas', 'angleRight')}" class="" />
        </span>
      {/if}
    {/if}
    <span class="ml-2">{entry.title}</span>
  </span>
</li>
