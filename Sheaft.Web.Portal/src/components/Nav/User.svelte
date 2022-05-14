<script lang='ts'>
  import logo from '$assets/svelte.png'
  import { getAuthStore } from '$stores/auth'
  import Fa from 'svelte-fa'
  import { faRightFromBracket, faUserLock } from '@fortawesome/free-solid-svg-icons'
  import { goto } from '@roxi/routify'
  import Primary from '$components/Buttons/Primary.svelte'

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

<div class='w-full self-end border-t flex items-center justify-center'>
  {#if $isAuthenticated}
    <div class='flex items-center justify-center'>
      <div class='m-4 flex cursor-pointer'>
        <img src={logo} width='30' alt='logo' class='mr-4' />
        <span class='overflow-hidden overflow-ellipsis'>{$user.username}</span>
      </div>
      <div on:click={logout}>
        <Fa icon={faRightFromBracket} class='m-4 cursor-pointer' />
      </div>
    </div>
  {:else}
    <Primary on:click={login} className='m-4'>
      <div class='flex items-center justify-center'>
      <Fa icon={faUserLock} class='mr-2' />
      <span class='ml-4'>Se connecter</span>
      </div>
    </Primary>
  {/if}
</div>
