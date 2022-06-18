<script lang="ts">
  import { page, goto, params } from "@roxi/routify";
  import Button from "$components/Button/Button.svelte";
  import { createForm } from "felte";
  import { getAccountModule } from "$components/Account/module";
  import type { Components } from "$types/api";
  import { mediator } from "$components/mediator";
  import { RegisterAccountCommand } from "$components/Account/commands/registerAccount";
  import { LoginUserCommand } from "$components/Account/commands/loginUser";
  import Input from "$components/Input/Input.svelte";
  import HorizontalSeparator from "$components/Separators/HorizontalSeparator.svelte";

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
        new RegisterAccountCommand(
          values.email,
          values.password,
          values.confirm,
          values.firstname,
          values.lastname
        )
      );
      await mediator.send(new LoginUserCommand(values.email, values.password));
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
        <Input
          label="Prénom"
          bind:value="{$data.firstname}"
          disabled="{$isSubmitting}"
          class="w-full"
          placeholder="Votre prénom"
        />
        <Input
          label="Nom"
          bind:value="{$data.lastname}"
          disabled="{$isSubmitting}"
          class="w-full"
          placeholder="Votre nom"
        />
      </div>
      <Input
        type="email"
        label="Adresse mail"
        bind:value="{$data.email}"
        disabled="{$isSubmitting}"
        class="mb-6 w-full"
      />
      <Input
        type="password"
        label="Mot de passe"
        bind:value="{$data.password}"
        disabled="{$isSubmitting}"
        class="mb-6 w-full"
      />
      <Input
        type="password"
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
