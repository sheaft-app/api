<script lang='ts'>
  import { page, goto, params } from '@roxi/routify'
  import Information from './forms/Information.svelte'
  import Addresses from './forms/Addresses.svelte'
  import { getAccountModule } from '$pages/account/module'
  import { mediator } from '$services/mediator'
  import { ConfigureAccountRequest } from '$commands/account/configureAccount'
  import Stepper from '$components/Stepper/Stepper.svelte'
  import { RefreshAccessTokenRequest } from '$commands/account/refreshAccessToken'
  import type { IAccountConfigurationResults, IAccountConfigurationSteps } from '$pages/account/account'

  const module = getAccountModule($goto)

  const steps: IAccountConfigurationSteps = {
    information: { index: 0, component: Information, name: 'Mes informations' },
    addresses: { index: 1, component: Addresses, name: 'Mes adresses' },
  }

  const submit = async (results: IAccountConfigurationResults): Promise<void> => {
    try {
      await mediator.send(new ConfigureAccountRequest(results.information.values?.accountType, results.information.values, results.addresses.values))
      await mediator.send(new RefreshAccessTokenRequest())
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
  <div class='md:w-8/12 lg:w-6/12 mb-12 md:mb-0'>
    <img data-ujs-name='Sign in' />
  </div>
  <div class='md:w-8/12 lg:w-5/12 lg:ml-20'>
    <h1 class='text-center'>{$page.title}</h1>
    <Stepper {steps} {cancel} {submit} />
  </div>
</div>
