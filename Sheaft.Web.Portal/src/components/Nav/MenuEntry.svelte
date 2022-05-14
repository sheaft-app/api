<script lang='ts'>
  import { isActive, goto, page } from '@roxi/routify'
  import { parseActivePath } from './path'
  import Fa from 'svelte-fa'
  import { getFaIcon, getFaIconFromFullName } from '$utils/faIcon.js'

  export let entry: { title: string; path: string; pages: Array<any> } = null
  export let canHighlight: boolean

  $: isEntryActive =
    canHighlight &&
    ($isActive(parseActivePath(entry.path)) ||
      parseActivePath($page.path) == parseActivePath(entry.path))

  const navigate = path => {
    $goto(path)
  }
</script>

<li
  class='menu-entry cursor-pointer mx-4 p-3 rounded-xl'
  class:active='{isEntryActive}'
  class:has-parent='{entry.parent}'
  on:click='{() => navigate(entry.path)}'
>
  <div class='flex items-center'>
    {#if !entry.parent}
      {#if entry.icon}
        <div class='menu-icon'>
          <Fa icon={getFaIconFromFullName(entry.icon)} class='menu-icon' />
        </div>
      {:else}
        <div class='menu-icon'>
          <Fa icon={getFaIcon('fas', 'angleRight')} class='' />
        </div>
      {/if}
    {/if}
    <span class='ml-2'>{entry.title}</span>
  </div>
</li>
