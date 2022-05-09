<script lang="ts">
  let count: number = 0;
  const increment = () => {
    count += 1;
  };
</script>

<button on:click="{increment}" class="btn-primary hover:bg-gray-300">
  Clicks: {count}
</button>

<style>
</style>
