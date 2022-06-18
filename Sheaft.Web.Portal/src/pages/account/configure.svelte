<script lang="ts">
  import { page, goto, params } from "@roxi/routify";
  import { getAccountModule } from "$components/Account/module";
  import type {
    AccountConfigurationResults,
    AccountConfigurationSteps
  } from "$components/Account/types";
  import ConfigureInformation from "$components/Account/ConfigureInformation.svelte";
  import ConfigureAddresses from "$components/Account/ConfigureAddresses.svelte";
  import { mediator } from "$components/mediator";
  import { ConfigureAccountCommand } from "$components/Account/commands/configureAccount";
  import { RefreshAccessTokenCommand } from "$components/Account/commands/refreshAccessToken";
  import Stepper from "$components/Stepper/Stepper.svelte";

  const module = getAccountModule($goto);

  const steps: AccountConfigurationSteps = {
    information: {
      component: ConfigureInformation,
      name: "Informations",
      initialValues: {}
    },
    addresses: {
      component: ConfigureAddresses,
      name: "Adresses",
      initialValues: {}
    }
  };

  const submit = async (results: AccountConfigurationResults): Promise<void> => {
    try {
      await mediator.send(
        new ConfigureAccountCommand(
          results.information.accountType,
          results.information,
          results.addresses
        )
      );
      await mediator.send(new RefreshAccessTokenCommand());
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
