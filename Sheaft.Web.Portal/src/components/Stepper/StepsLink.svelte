<script lang='ts'>
  export let isActive:boolean = false;
</script>

<div class='flex-auto border-t-2 transition duration-500 ease-in-out'
class:border-primary-600={isActive}
class:border-gray-300={!isActive}>  
</div>
