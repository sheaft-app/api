<script lang='ts'>
  import './inputs.scss'
  import { nanoid } from 'nanoid'

  export let id: string | undefined
  export let name: string | undefined
  export let value: string | undefined
  export let label: string | undefined
  export let placeholder: string | undefined
  export let disabled: boolean = false

  if (!id) id = nanoid(10);
  else if(!name) name = id;
</script>

<div class='p-4 bg-white'>
  {#if label}
    <label for='{id}'>{label}</label>
  {/if}
  <div class='relative mt-1'>
    <div class='absolute inset-y-0 left-0 flex items-center pl-3 pointer-events-none'>
      <svg
        class='w-5 h-5 text-gray-500'
        fill='currentColor'
        viewBox='0 0 20 20'
        xmlns='http://www.w3.org/2000/svg'
      >
        <path
          fill-rule='evenodd'
          d='M8 4a4 4 0 100 8 4 4 0 000-8zM2 8a6 6 0 1110.89 3.476l4.817 4.817a1 1 0 01-1.414 1.414l-4.816-4.816A6 6 0 012 8z'
          clip-rule='evenodd'></path>
      </svg>
    </div>
    <input
      id='{id}'
      name='{name}'
      type='search'
      disabled='{disabled}'
      placeholder='{placeholder}'
      bind:value
      class='{$$props.class} border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-80 pl-10 p-2.5'
    />
  </div>
</div>
