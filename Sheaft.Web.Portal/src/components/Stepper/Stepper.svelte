<script lang="ts">
  import CompletedStep from "$components/Stepper/CompletedStep.svelte";
  import ActiveStep from "$components/Stepper/ActiveStep.svelte";
  import NextStep from "$components/Stepper/NextStep.svelte";
  import StepsLink from "$components/Stepper/StepsLink.svelte";
  import { createEventDispatcher, onMount } from "svelte";
  import Button from "$components/Buttons/Button.svelte";

  export let steps: { name: string; icon: string }[] = [];
  export let isLoading: boolean = false;
  export let startingIndex: number = 0;
  export let submit = () => {};
  export let canGoNext = () => true;

  let currentPosition: number = 0;

  const dispatch = createEventDispatcher();

  const goToStep = (position, skipEvent?) => {
    currentPosition = position;
    if (skipEvent) return;

    dispatch("stepChanged", {
      step: steps[currentPosition],
      position: currentPosition
    });
  };

  const next = async () => {
    if (!canGoNext()) return;

    if (currentPosition + 1 > steps.length - 1) {
      await submit();
      return;
    }

    goToStep(++currentPosition);
  };

  const previous = () => {
    if (currentPosition - 1 < 0) return;

    goToStep(--currentPosition);
  };

  onMount(() => {
    if (startingIndex > 0) goToStep(startingIndex, true);
  });
</script>

<div class="flex items-center mt-4 mb-16 mx-6">
  {#each steps as step, index}
    {#if index < currentPosition}
      <CompletedStep
        name="{step.name}"
        icon="{step.icon}"
        on:click="{() => goToStep(index)}"
      />
    {:else if index === currentPosition}
      <ActiveStep name="{step.name}" icon="{step.icon}" />
    {:else}
      <NextStep
        name="{step.name}"
        icon="{step.icon}"
        on:click="{() => goToStep(index)}"
      />
    {/if}
    {#if index < steps.length - 1}
      <StepsLink isActive="{index < currentPosition}" />
    {/if}
  {/each}
</div>
<div class="my-4">
  <slot currentPosition="{currentPosition}" />
</div>
<div class="flex my-6">
  <Button
    type="button"
    disabled="{currentPosition == 0 || isLoading}"
    on:click="{previous}"
    class="back w-full mx-8"
    >Précédent
  </Button>
  <Button
    type="button"
    isLoading="{isLoading}"
    on:click="{next}"
    class="{currentPosition != steps.length - 1 ? 'accent' : 'primary'} w-full mx-8"
    >{currentPosition != steps.length - 1 ? "Suivant" : "Valider"}
  </Button>
</div>
