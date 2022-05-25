<script lang='ts'>
  import { page, goto, params } from '@roxi/routify'
  import Email from '$components/Inputs/Email.svelte'
  import Password from '$components/Inputs/Password.svelte'
  import Button from '$components/Buttons/Button.svelte'
  import HorizontalSeparator from '$components/HorizontalSeparator.svelte'
  import { login, register } from '$stores/auth'
  import type { IRegisterAccount } from '$types/auth'
  import Text from '$components/Inputs/Text.svelte'

  export let account: IRegisterAccount = {
    email: null,
    password: null,
    confirm: null,
    firstname: null,
    lastname: null
  }

  let isLoading: boolean = false

  const registerAccount = async () => {
    try {
      isLoading = true

      const result = await register(account)
      if (result) {
        const loginResult = await login(account.email, account.password)
        if (loginResult && $params.returnUrl && $params.returnUrl.length > 1) 
          $goto($params.returnUrl)
        else if (loginResult)
          $goto('/')
        else 
          $goto('/auth/login')
      }

      isLoading = false
    } catch (e) {
      isLoading = false
    }
  }
</script>

<!-- routify:options anonymous=true -->
<!-- routify:options title="Renseigner vos informations" -->

<div class='flex justify-center items-center flex-wrap h-full g-6 text-gray-800'>
  <div class='md:w-8/12 lg:w-6/12 mb-12 md:mb-0'>
    <img data-ujs-name='Sign in' />
  </div>
  <div class='md:w-8/12 lg:w-5/12 lg:ml-20'>
    <h1>{$page.title}</h1>
    <form>
      <div class='flex justify-between'>
        <Text bind:value='{account.firstname}' isLoading='{isLoading}' class='w-full' placeholder='Votre prénom' />
        <Text bind:value='{account.lastname}' isLoading='{isLoading}' class='w-full' placeholder='Votre nom' />
      </div>
      <Email bind:value='{account.email}' isLoading='{isLoading}' class='mb-6 w-full' />
      <Password
        bind:value='{account.password}'
        isLoading='{isLoading}'
        class='mb-6 w-full'
      />
      <Password
        bind:value='{account.confirm}'
        placeholder='Confirmation de mot de passe'
        class='mb-6 w-full'
      />
      <Button
        type='submit'
        isLoading='{isLoading}'
        on:click='{registerAccount}'
        class='primary w-full mt-8'
      >Créer
      </Button>
      <HorizontalSeparator>
        <a href='/auth/login?&username={account.email}&returnUrl={$params.returnUrl}'
        >J'ai déjà un compte</a
        >
      </HorizontalSeparator>
    </form>
  </div>
</div>
