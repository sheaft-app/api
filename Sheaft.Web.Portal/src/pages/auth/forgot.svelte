<script lang="ts">
  import { params } from "@roxi/routify";
  import { getAuthStore } from "$stores/auth";
  import Email from "$components/Inputs/Email.svelte";
  import Link from "$components/Link.svelte";
  import Submit from "$components/Buttons/Submit.svelte";
  import Fa from "svelte-fa";
  import { faCheck } from "@fortawesome/free-solid-svg-icons";
  import HorizontalSeparator from "$components/HorizontalSeparator.svelte";

  let email: string = $params.email;
  let resetRequested = $params.reset;

  const forgot = async () => {
    try {
      const result = await getAuthStore().forgot(email);
      if (!result) return;

      resetRequested = true;
    } catch (e) {
      console.log(e);
    }
  };
</script>

<!-- routify:options redirectIfAuthenticated=true -->
<!-- routify:options public=true -->

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
        <h1>J'ai oublié mon mot de passe</h1>
        {#if !resetRequested}
          <form class="">
            <Email
              bind:value="{email}"
              placeholder="Adresse mail de votre compte"
              className="mb-6 w-full"
            />
            <Submit on:click="{() => forgot()}" className="block w-full mt-8"
              >Reinitialiser
            </Submit>
            <HorizontalSeparator>
              <Link href="/auth/login?&username={email}">Finalement je m'en rappel</Link>
            </HorizontalSeparator>
          </form>
        {:else}
          <div class="items-center justify-center">
            <Fa icon="{faCheck}" scale="{5}" class="text-emerald-600 icon pulse w-full" />
            <div class=" text-center">
              <p class="w-full mt-16 mb-4">
                Votre demande de réinitialisation de mot de passe a bien été prise en
                compte.
              </p>
              <i>(cliquez sur le lien contenu dans l'email)</i>
            </div>
          </div>
        {/if}
      </div>
    </div>
  </div>
</section>

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
