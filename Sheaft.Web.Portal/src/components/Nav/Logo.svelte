<script lang='ts'>
  export let href: string = '/';
  import logo from "$assets/svelte.png";
</script>

<a href='{href}' class='w-full flex items-center self-end justify-center'>
  <img src={logo} width=80 alt='logo' class='m-4' />
</a>
