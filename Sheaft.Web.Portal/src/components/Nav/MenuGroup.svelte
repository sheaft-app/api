<script lang="ts">
  import { isActive } from "@roxi/routify";
  import Menu from "./Menu.svelte";
  import { parseSubActivePath } from "./path";
  import { getFaIcon, getFaIconFromFullName } from '$utils/faIcon.js'
  import Fa from 'svelte-fa'

  export let entry: { title: string; path: string; pages: Array<any>, visible:boolean } = null;

  let isGroupActive: boolean = false;

  $: if (entry.pages && entry.pages.length > 0) {
    isGroupActive = entry.pages.find(p => $isActive(parseSubActivePath(p.path))) != null;
  }

  $: isVisible = entry.visible && entry.pages.find(p => p.visible) != null;
</script>

{#if isVisible}
  <li class="menu-group">
    <div class='flex items-center mx-4 p-3'>
      {#if entry.icon}
        <Fa icon={getFaIconFromFullName(entry.icon)} class="mr-2 {isGroupActive ? 'text-violet-600 font-bold' : ''}"  />
      {:else}
        <Fa icon={getFaIcon('fas', 'angleRight')} class="mr-2 {isGroupActive ? 'text-violet-600 font-bold' : ''}" />
      {/if}
      <span 
        class:text-violet-600={isGroupActive}
        class:font-bold={isGroupActive}
      >{entry.title}</span>
    </div>
    <Menu entries="{entry.pages}" />
  </li>
{/if}
