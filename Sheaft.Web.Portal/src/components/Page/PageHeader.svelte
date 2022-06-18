<script lang='ts'>
  import { goto } from '@roxi/routify'
  import Fa from 'svelte-fa'
  import { faArrowAltCircleLeft } from '@fortawesome/free-solid-svg-icons'
  import Button from '$components/Button/Button.svelte'
  import type { PageAction } from '$components/Page/types'

  export let title: string
  export let subtitle: string
  export let previous: string | Function
  export let disabled: boolean = false
  export let actions: PageAction[] = []

  const navigateTo = (action: string | Function) => {
    if (typeof (action) == 'string')
      $goto(action)
    else
      action()
  }
</script>

<svelte:head>
  <title>{title}</title>
</svelte:head>

<div class='flex w-full flex flex-row items-center my-6'>
  {#if previous}
    <div class='mr-4'>
      <a href='#' on:click={() => navigateTo(previous)}>
        <Fa icon='{faArrowAltCircleLeft}' class='text-gray-500 hover:text-gray-400' size='24' />
      </a>
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
      {#if action.visible}
        <div class='mx-2'>
          {#if action.color == 'accent'}
            <Button type='button' class='bg-accent-600 hover:bg-accent-500' on:click={() => navigateTo(action.action)}
                    disabled='{action.disabled}'>{action.name}</Button>
          {:else if action.color == 'primary'}
            <Button type='button' class='bg-primary-600 hover:bg-primary-500' on:click={() => navigateTo(action.action)}
                    disabled='{action.disabled}'>{action.name}</Button>
          {:else if action.color == 'danger'}
            <Button type='button' class='bg-danger-600 hover:bg-danger-500' on:click={() => navigateTo(action.action)}
                    disabled='{action.disabled}'>{action.name}</Button>
          {:else if action.color == 'warning'}
            <Button type='button' class='bg-warning-600 hover:bg-warning-500' on:click={() => navigateTo(action.action)}
                    disabled='{action.disabled}'>{action.name}</Button>
          {:else if action.color == 'success'}
            <Button type='button' class='bg-success-600 hover:bg-success-500' on:click={() => navigateTo(action.action)}
                    disabled='{action.disabled}'>{action.name}</Button>
          {:else if action.color == 'info'}
            <Button type='button' class='bg-info-600 hover:bg-info-500' on:click={() => navigateTo(action.action)}
                    disabled='{action.disabled}'>{action.name}</Button>
          {:else}
            <Button type='button' class='bg-default-600 hover:bg-default-500' on:click={() => navigateTo(action.action)}
                    disabled='{action.disabled}'>{action.name}</Button>
          {/if}
        </div>
      {/if}
    {/each}
  {/if}
</div>
