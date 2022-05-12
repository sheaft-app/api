<!-- routify:options redirectIfAuthenticated=true -->
<script lang="ts">
  import { goto } from "@roxi/routify";
  import { getAuthStore } from "$stores/auth";
  import Password from "$components/Password.svelte";
  import Email from "$components/Email.svelte";
  import Link from "$components/Link.svelte";
  import SubmitButton from "$components/SubmitButton.svelte";

  let username: string = "";
  let password: string = "";

  const login = async () => {
    try {
      const returnUrl = await getAuthStore().login(username, password);
      $goto(returnUrl);
    } catch (e) {
      console.log(e);
    }
  };
</script>

<!-- routify:options public=true -->

<section class="h-screen">
  <div class="container px-6 py-12 h-full">
    <div class="flex justify-center items-center flex-wrap h-full g-6 text-gray-800">
      <div class="md:w-8/12 lg:w-6/12 mb-12 md:mb-0">
        <img
          src="https://mdbcdn.b-cdn.net/img/Photos/new-templates/bootstrap-login-form/draw2.svg"
          class="w-full"
          alt="Phone image"
        />
      </div>
      <div class="md:w-8/12 lg:w-5/12 lg:ml-20">
        <form>
          <div class="mb-6">
            <Email bind:value="{username}" />
          </div>
          <div class="mb-6">
            <Password bind:value="{password}" />
          </div>

          <div class="flex justify-between items-center mb-6">
            <Link href="/auth/forgot">Mot de passe oublié?</Link>
          </div>

          <SubmitButton on:click="{() => login()}">Se connecter</SubmitButton>
        </form>
      </div>
    </div>
  </div>
</section>
