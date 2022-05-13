<script lang='ts'>
  import logo from '$assets/svelte.png'
  import { getAuthStore } from '$stores/auth'
  import Fa from 'svelte-fa'
  import { faRightFromBracket, faUserLock } from '@fortawesome/free-solid-svg-icons'
  import { goto } from '@roxi/routify'

  const authStore = getAuthStore()
  const user = authStore.user
  const isAuthenticated = authStore.isAuthenticated
  const logout = () => {
    authStore.logout()
  }
  const login = () => {
    $goto('/auth/login')
  }
</script>

<div class='w-full self-end border-t'>
  {#if $isAuthenticated}
    <div class='flex items-center justify-center '>
      <div class='m-4 flex cursor-pointer'>
        <img src={logo} width='30' alt='logo' class='mr-4' />
        <span class='overflow-hidden overflow-ellipsis'>{$user.username}</span>
      </div>
      <div on:click={logout}>
        <Fa icon={faRightFromBracket} class='m-4 cursor-pointer' />
      </div>
    </div>
  {:else}
    <div on:click={login}
         class='cursor-pointer justify-center items-center flex f-primary-color rounded-full text-white py-4 px-8 m-4'>
      <Fa icon={faUserLock} class='mr-2' />
      <span class='ml-4'>Se connecter</span>
    </div>
  {/if}
</div>
