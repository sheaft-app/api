<script lang="ts">
  import { goto, params, page } from "@roxi/routify";
  import Button from "$components/Buttons/Button.svelte";
  import Password from "$components/Inputs/Password.svelte";
  import { getAccountModule } from '$pages/account/module'
  import { createForm } from 'felte'
  import type { Components } from '$types/api'
  import { mediator } from '$services/mediator'
  import { ResetPasswordRequest } from '$commands/account/resetPassword'

  const module = getAccountModule($goto);

  const { form, data, isSubmitting } = createForm<Components.Schemas.ResetPasswordRequest>({
    initialValues: {
      resetToken:$params.resetToken,
      password: '',
      confirm: ''
    },
    onSubmit: async (values) => {
      await mediator.send(new ResetPasswordRequest(values.resetToken, values.password, values.confirm))
    },
    onSuccess: () => {
      module.redirectIfRequired($params.returnUrl)
    }
  })
</script>

<!-- routify:options anonymous=true -->
<!-- routify:options title="Modifier votre mot de passe" -->

<div class="flex justify-center items-center flex-wrap h-full g-6 text-gray-800">
  <div class="md:w-8/12 lg:w-6/12 mb-12 md:mb-0">
    <img data-ujs-name="My password" />
  </div>
  <div class="md:w-8/12 lg:w-5/12 lg:ml-20">
    <h1>{$page.title}</h1>
    <form use:form>
      <Password
        label='Nouveau mot de passe'
        bind:value="{$data.password}"
        isLoading="{$isSubmitting}"
        placeholder="Votre nouveau mot de passe"
        class="mb-6 w-full"
      />
      <Password
        label='Confirmer le mot de passe'
        bind:value="{$data.confirm}"
        isLoading="{$isSubmitting}"
        placeholder="Confirmer le nouveau mot de passe"
        class="mb-6 w-full"
      />
      <Button
        type="submit"
        isLoading="{$isSubmitting}"
        class="primary w-full mt-6"
        >Enregistrer
      </Button>
    </form>
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
