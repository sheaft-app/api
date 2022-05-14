<script lang='ts'>
  import { isActive } from '@roxi/routify'
  import Menu from './Menu.svelte'
  import { parseSubActivePath } from './path'
  import { getFaIcon, getFaIconFromFullName } from '$utils/faIcon.js'
  import Fa from 'svelte-fa'

  export let entry: { title: string; path: string; pages: Array<any>, visible: boolean, icon: string } = null

  let isGroupActive: boolean = false

  $: if (entry.pages && entry.pages.length > 0) {
    isGroupActive = entry.pages.find(p => $isActive(parseSubActivePath(p.path))) != null
  }

  $: isVisible = entry.visible && entry.pages.find(p => p.visible) != null
</script>

{#if isVisible}
  <li class='menu-group'>
    <div class='flex items-center mx-4 p-3 rounded-xl font-medium'
         class:active='{isGroupActive}'>
      {#if entry.icon}
        <div class='menu-icon'>
          <Fa icon={getFaIconFromFullName(entry.icon)} class='menu-icon' />
        </div>
      {:else}
        <div class='menu-icon'>
          <Fa icon={getFaIcon('fas', 'angleRight')} class='' />
        </div>
      {/if}
      <span class='ml-2 '>{entry.title}</span>
    </div>
    <Menu entries='{entry.pages}' />
  </li>
{/if}
