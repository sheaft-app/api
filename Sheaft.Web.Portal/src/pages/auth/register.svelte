<script lang='ts'>
  import { page, goto } from '@roxi/routify'
  import Email from '$components/Inputs/Email.svelte'
  import Password from '$components/Inputs/Password.svelte'
  import Button from '$components/Buttons/Button.svelte'
  import HorizontalSeparator from '$components/HorizontalSeparator.svelte'
  import { register } from '$stores/auth'

  export let account: {email:string, password:string, confirm:string} = {}
  let isLoading: boolean = false

  const registerAccount = async () => {
    try {
      isLoading = true

      const result = await register(account)
      if (result)
        $goto('/auth/login', {username: account.email})

      isLoading = false
    } catch (e) {
      isLoading = false
    }
  }
</script>

<!-- routify:options redirectIfAuthenticated=true -->
<!-- routify:options public=true -->
<!-- routify:options title="Renseigner vos informations" -->

<div class='flex justify-center items-center flex-wrap h-full g-6 text-gray-800'>
  <div class='md:w-8/12 lg:w-6/12 mb-12 md:mb-0'>
    <img data-ujs-name='Sign in' />
  </div>
  <div class='md:w-8/12 lg:w-5/12 lg:ml-20'>
    <h1>{$page.title}</h1>
    <form>
      <Email bind:value='{account.email}' {isLoading} class='mb-6 w-full' />
      <Password bind:value='{account.password}' {isLoading} class='mb-6 w-full' />
      <Password
        bind:value='{account.confirm}'
        placeholder='Confirmation de mot de passe'
        class='mb-6 w-full'
      />
      <Button type='submit' {isLoading} on:click='{registerAccount}' class='primary w-full mt-8'
      >Créer
      </Button
      >
      <HorizontalSeparator>
        <a href='/auth/login?&username={account.email}'>J'ai déjà un compte</a>
      </HorizontalSeparator>
    </form>
  </div>
</div>
