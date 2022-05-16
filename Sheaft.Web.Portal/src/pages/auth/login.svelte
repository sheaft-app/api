<!-- routify:options public=true -->
<!-- routify:options redirectIfAuthenticated=true -->

<script lang='ts'>
  import qs from 'qs'
  import { goto } from '@roxi/routify'
  import { getAuthStore } from '$stores/auth'
  import Password from '$components/Inputs/Password.svelte'
  import Email from '$components/Inputs/Email.svelte'
  import Link from '$components/Link.svelte'
  import Submit from '$components/Buttons/Submit.svelte'

  let username: string = ''
  let password: string = ''

  const login = async () => {
    try {
      const result = await getAuthStore().login(username, password)
      if (!result) return

      const search = window.location.search.replace('?', '')
      const returnUrl =
        search.indexOf('returnUrl') > -1 ? qs.parse(search)['returnUrl'] : '/'
      $goto(returnUrl)
    } catch (e) {
      console.log(e)
    }
  }
</script>

<section class='h-screen bg-back-100'>
  <div class='container px-6 py-12 h-full'>
    <div class='flex justify-center items-center flex-wrap h-full g-6 text-gray-800'>
      <div class='md:w-8/12 lg:w-6/12 mb-12 md:mb-0'>
        <img
          src='https://mdbcdn.b-cdn.net/img/Photos/new-templates/bootstrap-login-form/draw2.svg'
          class='w-full'
          alt='Phone image'
        />
      </div>
      <div class='md:w-8/12 lg:w-5/12 lg:ml-20'>
        <form class=''>
            <Email bind:value='{username}' className='mb-6 w-full' />
            <Password bind:value='{password}' className='mb-6 w-full' />
            <Link href='/auth/forgot'>Mot de passe oublié?</Link>
            <Submit on:click='{() => login()}' className='block w-full mt-6'>Se connecter</Submit>
        </form>
      </div>
    </div>
  </div>
</section>
