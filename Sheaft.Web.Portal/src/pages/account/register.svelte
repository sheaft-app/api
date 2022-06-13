<script lang="ts">
  import { page, goto, params } from "@roxi/routify";
  import Email from "$components/Inputs/Email.svelte";
  import Password from "$components/Inputs/Password.svelte";
  import Button from "$components/Buttons/Button.svelte";
  import HorizontalSeparator from "$components/HorizontalSeparator.svelte";
  import Text from "$components/Inputs/Text.svelte";
  import { getAccountModule } from "$pages/account/module";
  import { createForm } from "felte";
  import type { Components } from "$types/api";
  import { mediator } from "$services/mediator";
  import { RegisterRequest } from "$commands/account/register";
  import { LoginRequest } from "$commands/account/login";

  const module = getAccountModule($goto);

  const { form, data, isSubmitting } = createForm<Components.Schemas.RegisterRequest>({
    initialValues: {
      email: $params.username,
      password: "",
      confirm: "",
      firstname: "",
      lastname: ""
    },
    onSubmit: async values => {
      await mediator.send(
        new RegisterRequest(
          values.email,
          values.password,
          values.confirm,
          values.firstname,
          values.lastname
        )
      );
      await mediator.send(new LoginRequest(values.email, values.password));
    },
    onSuccess: (id: string) => {
      module.redirectIfRequired($params.returnUrl);
    }
  });
</script>

<!-- routify:options anonymous=true -->
<!-- routify:options title="Renseigner vos informations" -->

<div class="flex justify-center items-center flex-wrap h-full g-6 text-gray-800">
  <div class="md:w-8/12 lg:w-6/12 mb-12 md:mb-0">
    <img data-ujs-name="Sign in" />
  </div>
  <div class="md:w-8/12 lg:w-5/12 lg:ml-20">
    <h1>{$page.title}</h1>
    <form use:form>
      <div class="flex justify-between">
        <Text
          label="Prénom"
          bind:value="{$data.firstname}"
          disabled="{$isSubmitting}"
          class="w-full"
          placeholder="Votre prénom"
        />
        <Text
          label="Nom"
          bind:value="{$data.lastname}"
          disabled="{$isSubmitting}"
          class="w-full"
          placeholder="Votre nom"
        />
      </div>
      <Email
        label="Adresse mail"
        bind:value="{$data.email}"
        disabled="{$isSubmitting}"
        class="mb-6 w-full"
      />
      <Password
        label="Mot de passe"
        bind:value="{$data.password}"
        disabled="{$isSubmitting}"
        class="mb-6 w-full"
      />
      <Password
        label="Confirmer le mot de passe"
        bind:value="{$data.confirm}"
        placeholder="Confirmation de mot de passe"
        disabled="{$isSubmitting}"
        class="mb-6 w-full"
      />
      <Button type="submit" isLoading="{$isSubmitting}" class="primary w-full mt-8"
        >Créer
      </Button>
      <HorizontalSeparator>
        <a href="/account/login?&username={$data.email}&returnUrl={$params.returnUrl}"
          >J'ai déjà un compte</a
        >
      </HorizontalSeparator>
    </form>
  </div>
</div>
