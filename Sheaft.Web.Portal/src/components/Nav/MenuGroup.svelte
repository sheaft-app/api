<script lang="ts">
  import { goto, isActive } from "@roxi/routify";
  import Menu from "./Menu.svelte";
  import { parseSubActivePath } from "./path";
  import { getFaIcon, getFaIconFromFullName } from "$utils/faIcon";
  import Fa from "svelte-fa";
  import type { IEntry } from '$components/Nav/menus'

  export let entry: IEntry = { visible:true, default: false, name: '', referenced: true, pages:[] };

  let isGroupActive: boolean = false;
  let isGroupSubActive: boolean = false;

  const navigate = (path: string) => {
    if (path) $goto(path);
  };

  $: if (entry.pages && entry.pages.length > 0) {
    isGroupActive = entry.pages.find((p:IEntry) => $isActive(parseSubActivePath(p.path)) && p.default) != null;    
    isGroupSubActive = entry.pages.find((p:IEntry) => $isActive(parseSubActivePath(p.path)) && !p.default) != null;
  }

  $: isVisible = entry.visible && entry.pages.find(p => p.visible) != null;
  $: defaultPage = entry.pages.find(p => p.default);
</script>

{#if isVisible}
  <li class="menu-group">
    <div
      class="flex items-center mx-4 p-3 rounded-xl font-medium"
      class:active="{isGroupActive}"
      class:subactive="{isGroupSubActive}"
      class:cursor-pointer="{defaultPage}"
      on:click="{() => navigate(defaultPage?.path)}"
    >
      {#if entry.icon}
        <div class="menu-icon">
          <Fa icon="{getFaIconFromFullName(entry.icon)}" class="menu-icon" />
        </div>
      {:else}
        <div class="menu-icon">
          <Fa icon="{getFaIcon('fas', 'angleRight')}" class="" />
        </div>
      {/if}
      <span class="ml-2 ">{entry.name}</span>
    </div>
    <Menu entries="{entry.pages}" expand="{isGroupActive || isGroupSubActive}" />
  </li>
{/if}
