<script lang="ts">
  export let href: string = "";
  export let className: string = "";
</script>

<a
  href="{href}"
  class="text-blue-600 hover:text-blue-700 focus:text-blue-700 active:text-blue-800 duration-200 transition ease-in-out {className}"
  ><slot />
</a>
