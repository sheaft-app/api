<script lang="ts">
  import { authStore } from '$stores/auth';
  import Password from '$components/Password.svelte';
  import Email from '$components/Email.svelte';
  import Checkbox from '$components/Checkbox.svelte';

  let username: string = '';
  let password: string = '';

  const login = async () => {
    await authStore.login(username, password);
  };
</script>

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
            <Email />
          </div>
          <div class="mb-6">
            <Password />
          </div>

          <div class="flex justify-between items-center mb-6">
            <Checkbox label="Se souvenir de moi" />
            <a
              href="#!"
              class="text-blue-600 hover:text-blue-700 focus:text-blue-700 active:text-blue-800 duration-200 transition ease-in-out"
              >Mot de passe oublié?</a
            >
          </div>

          <!-- Submit button -->
          <button
            type="submit"
            class="inline-block px-7 py-3 bg-blue-600 text-white font-medium text-sm leading-snug uppercase rounded shadow-md hover:bg-blue-700 hover:shadow-lg focus:bg-blue-700 focus:shadow-lg focus:outline-none focus:ring-0 active:bg-blue-800 active:shadow-lg transition duration-150 ease-in-out w-full"
            data-mdb-ripple="true"
            data-mdb-ripple-color="light"
            on:click="{() => login()}"
          >
            Se connecter
          </button>
        </form>
      </div>
    </div>
  </div>
</section>
