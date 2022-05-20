<script lang='ts'>
  import { goto, params, page } from '@roxi/routify'
  import { reset } from '$stores/auth'
  import Button from '$components/Buttons/Button.svelte'
  import Password from '$components/Inputs/Password.svelte'

  let code: string = $params.code
  let newPassword: string = $params.email
  let confirmPassword = $params.reset
  let isLoading: boolean = false

  const resetPassword = async () => {
    try {
      isLoading = true
      const result = await reset(code, newPassword, confirmPassword)
      if (!result) return

      $goto('/')
    } catch (e) {
      isLoading = false
      console.log(e)
    }
  }
</script>

<!-- routify:options public=true -->
<!-- routify:options redirectIfAuthenticated=true -->
<!-- routify:options title="Modifier votre mot de passe" -->

<div class='flex justify-center items-center flex-wrap h-full g-6 text-gray-800'>
  <div class='md:w-8/12 lg:w-6/12 mb-12 md:mb-0'>
    <img data-ujs-name='My password' />
  </div>
  <div class='md:w-8/12 lg:w-5/12 lg:ml-20'>
    <h1>{$page.title}</h1>
    <form>
      <Password
        bind:value='{newPassword}'
        {isLoading}
        placeholder='Votre nouveau mot de passe'
        class='mb-6 w-full'
      />
      <Password
        bind:value='{confirmPassword}'
        {isLoading}
        placeholder='Confirmer le nouveau mot de passe'
        class='mb-6 w-full'
      />
      <Button type='submit' {isLoading} on:click='{resetPassword}' class='primary w-full mt-6'
      >Enregistrer
      </Button
      >
    </form>
  </div>
</div>

<style lang='scss' global>
  .icon.pulse {
    animation: pulse 1s;
  }

  @keyframes pulse {
    0% {
      transform: scale(1);
    }

    70% {
      transform: scale(2);
    }

    100% {
      transform: scale(1);
    }
  }
</style>
