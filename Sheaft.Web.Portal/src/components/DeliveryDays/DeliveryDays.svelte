<script lang='ts'>
  import { DayOfWeek } from '$enums/days'
  import { day } from '$components/DeliveryDays/utils'
  import { nanoid } from 'nanoid'

  export let id: string | undefined
  export let name: string | undefined
  export let message: string = 'Veuillez selectionner les jours où vous pouvez livrer ce magasin'
  export let days: DayOfWeek[] = []
  export let disabled: boolean = false

  let availableDays: DayOfWeek[] = [
    DayOfWeek.Monday,
    DayOfWeek.Tuesday,
    DayOfWeek.Wednesday,
    DayOfWeek.Thursday,
    DayOfWeek.Friday,
    DayOfWeek.Saturday,
    DayOfWeek.Sunday
  ]

  if (!id) id = nanoid(10)
  else if (!name) name = id
</script>

{#if days}
  <label>{message} * </label>
  <ul class='m-3'>
    {#each availableDays as availableDay}
      <li>
        <label class='form-check-label inline-block text-gray-800 cursor-pointer font-normal'>
          <input
            bind:group='{days}'
            type='checkbox'
            value='{availableDay}'
            disabled='{disabled}'
            class='w-4 h-4 m-1' />{day(availableDay)}</label>
      </li>
    {/each}
  </ul>
{/if}
