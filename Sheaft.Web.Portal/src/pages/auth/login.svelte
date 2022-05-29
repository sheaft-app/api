<script lang="ts">
  import { page, goto, params } from "@roxi/routify";
  import { login } from "$stores/auth";
  import Password from "$components/Inputs/Password.svelte";
  import Email from "$components/Inputs/Email.svelte";
  import HorizontalSeparator from "$components/HorizontalSeparator.svelte";
  import Button from "$components/Buttons/Button.svelte";

  let username: string = $params.username;
  let password: string = "";
  let isLoading: boolean = false;

  const loginUser = async () => {
    try {
      isLoading = true;
      const result = await login(username, password);
      if (!result) return;

      $goto($params.returnUrl ?? "/");
    } catch (e) {
      isLoading = false;
      console.log(e);
    }
  };
</script>

<!-- routify:options anonymous=true -->
<!-- routify:options title="Bienvenue" -->

<div class="flex justify-center items-center flex-wrap g-6 h-full">
  <div class="md:w-8/12 lg:w-6/12 mb-12 md:mb-0">
    <img data-ujs-name="Login" />
  </div>
  <div class="md:w-8/12 lg:w-5/12 lg:ml-20">
    <h1>{$page.title}</h1>
    <form>
      <Email label="Nom d'utilisateur" bind:value="{username}" isLoading="{isLoading}" class="mb-6 w-full" />
      <Password label="Votre mot de passe" bind:value="{password}" isLoading="{isLoading}" class="mb-6 w-full" />
      <a href="/auth/forgot?&email={username}">Mot de passe oublié?</a>
      <Button
        type="submit"
        on:click="{loginUser}"
        class="primary w-full mt-8"
        isLoading="{isLoading}">Se connecter</Button
      >
      <HorizontalSeparator>
        <a href="/auth/register?&username={username}&returnUrl={$params.returnUrl}"
          >Je n'ai pas de compte</a
        >
      </HorizontalSeparator>
    </form>
  </div>
</div>
