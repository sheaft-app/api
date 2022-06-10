<script lang='ts'>
  import { page, goto, params } from '@roxi/routify'
  import Information from './components/Information.svelte'
  import Legals from './components/Legals.svelte'
  import Localisation from './components/Localisation.svelte'
  import { getAuthModule } from '$pages/auth/module'
  import { mediator } from '$services/mediator'
  import { ConfigureAccountRequest } from '$commands/auth/configureAccount'
  import type { IConfigureState } from '$commands/auth/configureAccount'

  const module = getAuthModule($goto)
  const pages = {
    0: { name: 'info', page: Information },
    1: { name: 'legals', page: Legals },
    2: { name: 'localisation', page: Localisation }
  }
  
  let state: IConfigureState = {
    info: {},
    legals: {},
    localisation: {}
  }
  
  let pageNumber = 0;

  const onSubmit = async (values: any): Promise<void> => {
    state[pages[pageNumber].name] = values;
    if (pageNumber === pagesCount) {
      try {
        await mediator.send(new ConfigureAccountRequest(state.info.accountType, state))
        module.redirectIfRequired($params.returnUrl)
      } catch (exc) {
      }
    } else {
      pageNumber += 1
    }
    return Promise.resolve()
  }

  const onBack = (values: any): void => {
    if (pageNumber === 0) {
      module.redirectIfRequired(params.returnUrl)
      return
    }

    state[pages[pageNumber]] = values
    pageNumber -= 1
  }

  $: pagesCount = Object.entries(pages).length - 1
</script>

<!-- routify:options title="Compléter votre inscription" -->

<div class='flex justify-center items-center flex-wrap h-full g-6 text-gray-800 mb-6'>
  <div class='md:w-8/12 lg:w-6/12 mb-12 md:mb-0'>
    <img data-ujs-name='Sign in' />
  </div>
  <div class='md:w-8/12 lg:w-5/12 lg:ml-20'>
    <h1>{$page.title}</h1>
    <svelte:component
      this={pages[pageNumber].page}
      onSubmit={onSubmit}
      onBack={onBack}
      accountType='{state.info.accountType}'
      initialValues={state[pages[pageNumber].name]}
    />
  </div>
</div>
