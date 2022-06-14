<script lang='ts'>
  import { page, goto, params } from '@roxi/routify'
  import { getAccountModule } from '$pages/account/module'
  import { mediator } from '$features/mediator'
  import Stepper from '$components/Stepper/Stepper.svelte'
  import Information from '$pages/account/_forms/Information.svelte'
  import Addresses from '$pages/account/_forms/Addresses.svelte'
  import type { IAccountConfigurationResults, IAccountConfigurationSteps } from '$features/account/types'
  import { ConfigureAccountCommand } from '$features/account/commands/configureAccount'
  import { RefreshAccessTokenCommand } from '$features/account/commands/refreshAccessToken'

  const module = getAccountModule($goto)

  const steps: IAccountConfigurationSteps = {
    information: {
      component: Information,
      name: 'Informations',
      initialValues: {}
    },
    addresses: { component: Addresses, name: 'Adresses', initialValues: {} }
  }

  const submit = async (results: IAccountConfigurationResults): Promise<void> => {
    try {
      await mediator.send(
        new ConfigureAccountCommand(
          results.information.accountType,
          results.information,
          results.addresses
        )
      )
      await mediator.send(new RefreshAccessTokenCommand())
      module.redirectIfRequired($params.returnUrl)
      return Promise.resolve()
    } catch (exc) {
      return Promise.reject()
    }
  }

  const cancel = (): void => {
    module.redirectIfRequired(params.returnUrl)
  }
</script>

<!-- routify:options title="Compléter votre inscription" -->

<div class='flex justify-center items-center flex-wrap h-full g-6 text-gray-800 mb-6'>
  <div class='md:w-8/12 lg:w-6/12 mb-12 sm:hidden lg:visible'>
    <img data-ujs-name='Sign in' />
  </div>
  <div class='md:w-8/12 lg:w-5/12'>
    <h1 class='text-center pb-10'>{$page.title}</h1>
    <Stepper steps='{steps}' cancel='{cancel}' submit='{submit}' />
  </div>
</div>
