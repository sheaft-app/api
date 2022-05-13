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
  class='menu-entry cursor-pointer p-3 mx-4 rounded-xl'
  class:bg-violet-100='{isEntryActive}'
  class:border-violet-300='{isEntryActive}'
  class:border='{isEntryActive}'
  class:text-violet-600='{isEntryActive}'
  on:click='{() => navigate(entry.path)}'
>
  <div class='flex items-center'>
    {#if entry.icon}
      <Fa icon={getFaIconFromFullName(entry.icon)} class='mr-2' />
    {:else}
      <Fa icon={getFaIcon('fas', 'exclamation')} class='mr-2' />
    {/if}
    <span>{entry.title}</span>
  </div>
</li>
