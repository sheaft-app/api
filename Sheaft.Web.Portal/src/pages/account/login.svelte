<script lang="ts">
  import { page, goto, params } from "@roxi/routify";
  import Password from "$components/Inputs/Password.svelte";
  import Email from "$components/Inputs/Email.svelte";
  import HorizontalSeparator from "$components/HorizontalSeparator.svelte";
  import Button from "$components/Buttons/Button.svelte";
  import { createForm } from 'felte'
  import type { Components } from '$types/api'
  import { mediator } from '$services/mediator'
  import { getAccountModule } from '$pages/account/module'
  import { LoginRequest } from '$commands/account/login'

  const module = getAccountModule($goto);
  
  const { form, data, isSubmitting } = createForm<Components.Schemas.LoginRequest>({
    initialValues: {
      username:$params.username,
      password: ''
    },
    onSubmit: async (values) => {
      await mediator.send(new LoginRequest(values.username, values.password))
    },
    onSuccess: (id: string) => {
      module.redirectIfRequired($params.returnUrl)
    }
  })
</script>

<!-- routify:options anonymous=true -->
<!-- routify:options title="Bienvenue" -->

<div class="flex justify-center items-center flex-wrap g-6 h-full">
  <div class="md:w-8/12 lg:w-6/12 mb-12 md:mb-0">
    <img data-ujs-name="Login" />
  </div>
  <div class="md:w-8/12 lg:w-5/12 lg:ml-20">
    <h1>{$page.title}</h1>
    <form use:form>
      <Email 
        id='username'
        label="Nom d'utilisateur" 
        bind:value="{$data.username}" 
        isLoading="{$isSubmitting}" 
        class="mb-6 w-full" />
      <Password 
        id='password'
        label="Votre mot de passe" 
        bind:value="{$data.password}" 
        isLoading="{$isSubmitting}" 
        class="mb-6 w-full" />
      <a href="/account/forgot?&email={$data.username}">Mot de passe oublié?</a>
      <Button
        type="submit"
        class="primary w-full mt-8"
        isLoading="{$isSubmitting}">Se connecter</Button
      >
      <HorizontalSeparator>
        <a href="/account/register?&username={$data.username}&returnUrl={$params.returnUrl}"
          >Je n'ai pas de compte</a
        >
      </HorizontalSeparator>
    </form>
  </div>
</div>
