<script lang="ts">
  export let className: string = "";
</script>

<div class="relative flex py-5 items-center {className}">
  <div class="flex-grow border-t border-gray-400"></div>
  <span class="flex-shrink mx-4"><slot /></span>
  <div class="flex-grow border-t border-gray-400"></div>
</div>
