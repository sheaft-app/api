<script lang='ts'>
  import Fa from 'svelte-fa'
  import { getFaIconFromFullName } from '$utils/faIcon.js'
  
  export let name:string = "";
  export let icon: string = ''
</script>

  <div class='flex items-center text-white relative'>
    <div class='rounded-full transition duration-500 ease-in-out h-12 w-12 py-3 border-2 bg-primary-600 border-primary-600'>
      <Fa icon={getFaIconFromFullName(icon)}  class='m-auto' />
    </div>
    <div class='absolute top-0 -ml-10 text-center mt-16 w-32 text-xs font-medium uppercase text-primary-600'>{name}</div>
  </div>
