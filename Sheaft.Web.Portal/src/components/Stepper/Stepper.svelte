<script lang="ts">
  import CompletedStep from "./CompletedStep.svelte";
  import ActiveStep from "./ActiveStep.svelte";
  import NextStep from "./NextStep.svelte";
  import StepsLink from "./StepsLink.svelte";
  import type {
    IStepDefinition,
    IStepsDefinition,
    IStepsResult
  } from "$components/Stepper/types";
  import { onMount } from "svelte";

  export let submit = async (results: IStepsResult): Promise<void> => {};
  export let cancel = async (): Promise<void> => {};
  export let steps: IStepsDefinition = {};
  export let results: IStepsResult = {};
  let pages: Record<string, IStepDefinition<any>> = {};

  let pageNumber = 0;

  const onSubmit = async (values: any): Promise<void> => {
    results[pages[pageNumber].id] = values;
    if (pageNumber === pagesCount) {
      return await submit(results);
    }

    pageNumber += 1;
    return Promise.resolve();
  };

  const onBack = async (values: any): Promise<void> => {
    results[pages[pageNumber].id] = values;

    if (pageNumber === 0) {
      return await cancel();
    }

    pageNumber -= 1;
    return Promise.resolve();
  };

  onMount(() => {
    Object.entries(steps).map((s, index) => {
      pages[index] = <any>{ ...s[1], id: s[0] };

      if (pages[index].initialValues)
        results[pages[index].id] = pages[index].initialValues;
    });
  });

  $: pageSteps = Object.entries(steps).map(s => s[1]);
  $: pagesCount = pageSteps.length - 1;
</script>

<div class="flex items-center mt-4 mb-16 mx-6">
  {#each pageSteps as step, index}
    {#if index < pageNumber}
      <CompletedStep name="{step.name}" icon="{step.icon}" step="{index + 1}" />
    {:else if index === pageNumber}
      <ActiveStep name="{step.name}" icon="{step.icon}" step="{index + 1}" />
    {:else}
      <NextStep name="{step.name}" icon="{step.icon}" step="{index + 1}" />
    {/if}
    {#if index < pagesCount}
      <StepsLink isActive="{index < pageNumber}" />
    {/if}
  {/each}
</div>
<div class="my-4">
  <svelte:component
    this="{pages[pageNumber]?.component}"
    onSubmit="{onSubmit}"
    onBack="{onBack}"
    initialValues="{results[pages[pageNumber].id]}"
    state="{results}"
  />
</div>
