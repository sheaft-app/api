<script lang="ts">
  import { page, goto, params } from "@roxi/routify";
  import Button from "$components/Button/Button.svelte";
  import { createForm } from "felte";
  import { getAccountModule } from "$components/Account/module";
  import type { Components } from "$types/api";
  import { mediator } from "$components/mediator";
  import { LoginUserCommand } from "$components/Account/commands/loginUser";
  import Input from "$components/Input/Input.svelte";
  import HorizontalSeparator from "$components/Separators/HorizontalSeparator.svelte";

  const module = getAccountModule($goto);

  const { form, data, isSubmitting } = createForm<Components.Schemas.LoginRequest>({
    initialValues: {
      username: $params.username,
      password: ""
    },
    onSubmit: async values => {
      await mediator.send(new LoginUserCommand(values.username, values.password));
    },
    onSuccess: (id: string) => {
      module.redirectIfRequired($params.returnUrl);
    }
  });
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
      <Input
        id="username"
        type="email"
        label="Nom d'utilisateur"
        bind:value="{$data.username}"
        disabled="{$isSubmitting}"
        class="mb-6 w-full" />
      <Input
        id="password"
        type="password"
        label="Votre mot de passe"
        bind:value="{$data.password}"
        disabled="{$isSubmitting}"
        class="mb-6 w-full" />
      <a href="/account/forgot?&email={$data.username}">Mot de passe oublié?</a>
      <Button type="submit" class="primary w-full mt-8" isLoading="{$isSubmitting}"
        >Se connecter</Button>
      <HorizontalSeparator>
        <a
          href="/account/register?&username={$data.username}&returnUrl={$params.returnUrl}"
          >Je n'ai pas de compte</a>
      </HorizontalSeparator>
    </form>
  </div>
</div>
