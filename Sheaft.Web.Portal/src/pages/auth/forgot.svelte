<script lang="ts">
  import { goto, page, params } from '@roxi/routify'
  import Email from "$components/Inputs/Email.svelte";
  import Button from "$components/Buttons/Button.svelte";
  import Fa from "svelte-fa";
  import { faCheck } from "@fortawesome/free-solid-svg-icons";
  import HorizontalSeparator from "$components/HorizontalSeparator.svelte";
  import { getAuthModule } from '$pages/auth/module'
  import { createForm } from 'felte'
  import type { Components } from '$types/api'
  import { mediator } from '$services/mediator'
  import { ForgotPasswordRequest } from '$commands/auth/forgotPassword'

  const module = getAuthModule($goto);
  let resetRequested = false;

  const { form, data, isSubmitting } = createForm<Components.Schemas.ForgotPasswordRequest>({
    initialValues: {
      email:$params.username
    },
    onSubmit: async (values) => {
      await mediator.send(new ForgotPasswordRequest(values.email))
    },
    onSuccess: () => {
      resetRequested = true;
    }
  })
</script>

<!-- routify:options anonymous=true -->
<!-- routify:options title="J'ai oublié mon mot de passe" -->

<div class="flex justify-center items-center flex-wrap h-full g-6 text-gray-800">
  <div class="md:w-8/12 lg:w-6/12 mb-12 md:mb-0">
    <img data-ujs-name="Forgot password" />
  </div>
  <div class="md:w-8/12 lg:w-5/12 lg:ml-20">
    <h1>{$page.title}</h1>
    {#if !resetRequested}
      <form use:form>
        <Email
          label='Votre adresse mail'
          bind:value="{$data.email}"
          isLoading="{$isSubmitting}"
          placeholder="Adresse mail de votre compte"
          class="mb-6 w-full"
        />
        <Button
          type="submit"
          isLoading="{$isSubmitting}"
          class="primary w-full mt-8"
          >Reinitialiser
        </Button>
        <HorizontalSeparator>
          <a href="/auth/login?&username={$data.email}">Finalement je m'en rappel</a>
        </HorizontalSeparator>
      </form>
    {:else}
      <div class="items-center justify-center">
        <Fa icon="{faCheck}" scale="{5}" class="text-emerald-600 icon pulse w-full" />
        <div class=" text-center">
          <p class="w-full mt-16 mb-4">
            Votre demande de réinitialisation de mot de passe a bien été prise en compte.
          </p>
          <i>(cliquez sur le lien contenu dans l'email)</i>
        </div>
      </div>
    {/if}
  </div>
</div>

<style lang="scss" global>
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
