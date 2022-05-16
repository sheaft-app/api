<script lang="ts">
  import { goto, params } from "@roxi/routify";
  import { getAuthStore } from "$stores/auth";
  import Submit from "$components/Buttons/Submit.svelte";
  import Password from "$components/Inputs/Password.svelte";

  let code: string = $params.code;
  let newPassword: string = $params.email;
  let confirmPassword = $params.reset;

  const reset = async () => {
    try {
      const result = await getAuthStore().reset(code, newPassword, confirmPassword);
      if (!result) return;

      $goto("/");
    } catch (e) {
      console.log(e);
    }
  };
</script>

<!-- routify:options public=true -->
<!-- routify:options redirectIfAuthenticated=true -->

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
        <h1>Modifier votre mot de passe</h1>
        <form>
          <Password
            bind:value="{newPassword}"
            placeholder="Votre nouveau mot de passe"
            className="mb-6 w-full"
          />
          <Password
            bind:value="{confirmPassword}"
            placeholder="Confirmer le nouveau mot de passe"
            className="mb-6 w-full"
          />
          <Submit on:click="{() => reset()}" className="block w-full mt-6"
            >Enregistrer</Submit
          >
        </form>
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
