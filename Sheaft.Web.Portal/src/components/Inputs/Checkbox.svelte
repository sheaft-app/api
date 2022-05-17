<script lang="ts">
  import "./inputs.scss";

  export let value: boolean = false;
  export let label: string = null;
  export let isLoading: boolean = false
</script>

<div class="form-check">
  <input
    type="checkbox"
    bind:value
    disabled={isLoading}
    class="appearance-none h-4 w-4 border border-gray-300 rounded-sm bg-white checked:bg-blue-600 checked:border-blue-600 focus:outline-none transition duration-200 mt-1 align-top bg-no-repeat bg-center bg-contain float-left mr-2 cursor-pointer {$$props.class}"
  />
  {#if label}
    <label class="form-check-label inline-block text-gray-800">
      {label}
    </label>
  {/if}
</div>
