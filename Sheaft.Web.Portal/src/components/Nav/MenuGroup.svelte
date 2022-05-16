<script lang="ts">
  import { goto, isActive } from "@roxi/routify";
  import Menu from "./Menu.svelte";
  import { parseSubActivePath } from "./path";
  import { getFaIcon, getFaIconFromFullName } from "$utils/faIcon";
  import Fa from "svelte-fa";

  export let entry: {
    title: string;
    path: string;
    pages: Array<any>;
    visible: boolean;
    icon: string;
  } = null;

  let isGroupActive: boolean = false;

  const navigate = (path: string) => {
    if (path) $goto(path);
  };

  $: if (entry.pages && entry.pages.length > 0) {
    isGroupActive =
      entry.pages.find(p => {
        //console.log(entry, parseSubActivePath(p.path), $isActive(parseSubActivePath(p.path)))
        return $isActive(parseSubActivePath(p.path));
      }) != null;
  }

  $: isVisible = entry.visible && entry.pages.find(p => p.visible) != null;
  $: defaultPage = entry.pages.find(p => p.default);
</script>

{#if isVisible}
  <li class="menu-group">
    <div
      class="flex items-center mx-4 p-3 rounded-xl font-medium"
      class:active="{isGroupActive}"
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
      <span class="ml-2 ">{entry.title}</span>
    </div>
    <Menu entries="{entry.pages}" expand="{isGroupActive}" />
  </li>
{/if}
