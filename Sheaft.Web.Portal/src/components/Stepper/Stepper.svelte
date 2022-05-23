<script lang='ts'>
  import CompletedStep from '$components/Stepper/CompletedStep.svelte'
  import ActiveStep from '$components/Stepper/ActiveStep.svelte'
  import NextStep from '$components/Stepper/NextStep.svelte'
  import StepsLink from '$components/Stepper/StepsLink.svelte'

  export let steps: { name: string, position: number }[] = []
  export let activeStep: number = 0

  const goToStep = (position) => {
    activeStep = position
  }

  const next = () => {
    if (++activeStep > steps.length)
      return

    activeStep++
  }

  const previous = () => {
    if (--activeStep < 0)
      return

    activeStep--
  }

</script>

<div class='flex items-center mt-4 mb-16'>
  {#each steps as step}
    {#if step.position < activeStep }
      <CompletedStep name={step.name} icon={step.icon} on:click={() => goToStep(step.position)} />
    {:else if step.position === activeStep }
      <ActiveStep name={step.name} icon={step.icon} />
    {:else}
      <NextStep name={step.name} icon={step.icon} on:click={() => goToStep(step.position)} />
    {/if}
    {#if step.position < steps.length - 1}
      <StepsLink isActive={step.position < activeStep} />
    {/if}
  {/each}
</div>
