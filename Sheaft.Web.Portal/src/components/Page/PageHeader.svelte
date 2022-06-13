<script lang='ts'>
  import Fa from 'svelte-fa'
  import { faArrowAltCircleLeft } from '@fortawesome/free-solid-svg-icons'
  import type { IPageAction } from './types/pageHeader'
  import Button from '$components/Buttons/Button.svelte'
  import { goto } from '@roxi/routify'

  export let title: string
  export let subtitle: string
  export let previous: string | Function
  export let actions: IPageAction[] = []
  
  const navigateTo = (action:string | Function) => {
    if(typeof(action) == 'string')
      $goto(action);
    else
      action();
  }
</script>

<svelte:head>
  <title>{title}</title>
</svelte:head>

<div class='flex w-full flex flex-row items-center my-6'>
  {#if previous}
    <div class='mr-4'>
      {#if typeof (previous) == 'string'}
        <a href='{previous}'>
          <Fa icon='{faArrowAltCircleLeft}' />
        </a>
      {:else}
        <a href='#' on:click={previous}>
          <Fa icon='{faArrowAltCircleLeft}' class='text-gray-500 hover:text-gray-400' size='24' />
        </a>
      {/if}
    </div>
  {/if}
  <div class='flex-grow'>
    <h1>{title}</h1>
    {#if subtitle}
      <small>{subtitle}</small>
    {/if}
  </div>
  {#if actions?.length > 0}
    {#each actions as action}
      <div class='mx-2'>
      {#if action.color == 'accent'}
        <Button type='button' class='bg-accent-600 hover:bg-accent-500' on:click={() => navigateTo(action.action)}>{action.name}</Button>
      {:else if action.color == 'primary'}
        <Button type='button' class='bg-primary-600 hover:bg-primary-500' on:click={() => navigateTo(action.action)}>{action.name}</Button>
      {:else if action.color == 'danger'}
        <Button type='button' class='bg-red-600 hover:bg-red-500' on:click={() => navigateTo(action.action)}>{action.name}</Button>
      {:else}
        <Button type='button' class='bg-back-600 hover:bg-back-500' on:click={() => navigateTo(action.action)}>{action.name}</Button>
      {/if}
      </div>
    {/each}
  {/if}
</div>
