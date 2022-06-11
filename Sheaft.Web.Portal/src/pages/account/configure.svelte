<script lang="ts">
  import { page, goto, params } from "@roxi/routify";
  import Information from "./forms/Information.svelte";
  import Addresses from "./forms/Addresses.svelte";
  import { getAccountModule } from "$pages/account/module";
  import { mediator } from "$services/mediator";
  import { ConfigureAccountRequest } from "$commands/account/configureAccount";
  import Stepper from "$components/Stepper/Stepper.svelte";
  import { RefreshAccessTokenRequest } from "$commands/account/refreshAccessToken";
  import type {
    IAccountConfigurationResults,
    IAccountConfigurationSteps
  } from "$pages/account/account";

  const module = getAccountModule($goto);

  const steps: IAccountConfigurationSteps = {
    information: {
      component: Information,
      name: "Informations",
      initialValues: { tradeName: "test" }
    },
    addresses: { component: Addresses, name: "Adresses", initialValues: {} }
  };

  const submit = async (results: IAccountConfigurationResults): Promise<void> => {
    try {
      await mediator.send(
        new ConfigureAccountRequest(
          results.information.accountType,
          results.information,
          results.addresses
        )
      );
      await mediator.send(new RefreshAccessTokenRequest());
      module.redirectIfRequired($params.returnUrl);
      return Promise.resolve();
    } catch (exc) {
      return Promise.reject();
    }
  };

  const cancel = (): void => {
    module.redirectIfRequired(params.returnUrl);
  };
</script>

<!-- routify:options title="Compléter votre inscription" -->

<div class="flex justify-center items-center flex-wrap h-full g-6 text-gray-800 mb-6">
  <div class="md:w-8/12 lg:w-6/12 mb-12 sm:hidden lg:visible">
    <img data-ujs-name="Sign in" />
  </div>
  <div class="md:w-8/12 lg:w-5/12">
    <h1 class="text-center pb-10">{$page.title}</h1>
    <Stepper steps="{steps}" cancel="{cancel}" submit="{submit}" />
  </div>
</div>
