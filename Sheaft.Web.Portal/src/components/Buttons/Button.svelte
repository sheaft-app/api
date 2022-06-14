<script lang="ts">
  import { createEventDispatcher } from "svelte";
  import "./button.scss";
  import Fa from "svelte-fa";
  import { faSpinner } from "@fortawesome/free-solid-svg-icons";
  import { nanoid } from "nanoid";
  import { getFaIconFromFullName } from '$components/Icons/faIconRetriever'

  export let isLoading: boolean = false;
  export let disabled: boolean = false;
  export let icon: any = null;

  const dispatch = createEventDispatcher();

  const dispatchClick = (e: any) => {
    if (!isLoading) dispatch("click", e);
  };

  $: matchedIcon = typeof icon == "string" ? getFaIconFromFullName(icon) : icon;
</script>

<button
  id="{nanoid()}"
  type="{$$props.type}"
  disabled="{disabled || isLoading}"
  class="{$$props.class}"
  on:click="{dispatchClick}"
>
  {#if isLoading}
    <Fa icon="{faSpinner}" spin class="mx-5" scale="1.5" />
  {:else}
    {#if matchedIcon}
      <Fa icon="{matchedIcon}" class="mx-5" scale="1.5" />
    {/if}
    <slot />
  {/if}
</button>
