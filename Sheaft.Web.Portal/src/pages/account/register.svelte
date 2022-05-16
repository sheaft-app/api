<script lang="ts">
  import { page, params } from "@roxi/routify";
  import Link from "$components/Link.svelte";
  import Email from "$components/Inputs/Email.svelte";
  import Password from "$components/Inputs/Password.svelte";
  import Submit from "$components/Buttons/Submit.svelte";
  import HorizontalSeparator from "$components/HorizontalSeparator.svelte";

  export let username: string = $params.username;
  export let password: string = "";
  export let confirmPassword: string = "";

  const register = () => {};
</script>

<!-- routify:options redirectIfAuthenticated=true -->
<!-- routify:options public=true -->
<!-- routify:options title="Renseigner vos informations" -->

<section class="h-screen bg-back-100">
  <div class="container px-6 py-12 h-full">
    <div class="flex justify-center items-center flex-wrap h-full g-6 text-gray-800">
      <div class="md:w-8/12 lg:w-6/12 mb-12 md:mb-0">
        <img
          src="https://mdbcdn.b-cdn.net/img/Photos/new-templates/bootstrap-login-form/draw1.svg"
          class="w-full"
          alt="Phone image"
        />
      </div>
      <div class="md:w-8/12 lg:w-5/12 lg:ml-20">
        <h1>{$page.title}</h1>
        <form>
          <Email bind:value="{username}" className="mb-6 w-full" />
          <Password bind:value="{password}" className="mb-6 w-full" />
          <Password
            bind:value="{confirmPassword}"
            placeholder="Confirmation de mot de passe"
            className="mb-6 w-full"
          />
          <Submit on:click="{() => register()}" className="block w-full mt-8"
            >Créer</Submit
          >
          <HorizontalSeparator>
            <Link href="/auth/login?&username={username}">J'ai déjà un compte</Link>
          </HorizontalSeparator>
        </form>
      </div>
    </div>
  </div>
</section>
