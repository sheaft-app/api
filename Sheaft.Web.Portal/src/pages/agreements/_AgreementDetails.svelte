<script lang='ts'>
  import { page, goto } from '@roxi/routify'
  import { onMount } from 'svelte'
  import PageHeader from '$components/Page/PageHeader.svelte'
  import { mediator } from '$features/mediator'
  import type { Components } from '$features/api'
  import { GetAgreementQuery } from '$features/agreements/queries/getAgreement'
  import { AgreementStatus } from '$features/agreements/enums'

  export let id = ''
  export let goHome = () => {}
  export let previous = () => {
    history.back();
  }
  
  $: isLoading = false
  let agreement: Components.Schemas.AgreementDto = null;
  
  onMount(async () => {
    try {
      isLoading = true
      agreement = await mediator.send(new GetAgreementQuery(id))
      isLoading = false
    } catch (exc) {
      goHome()
    }
  })
  
  $: canAcceptOrRefuse = agreement?.status == AgreementStatus.Pending;
  $: canCancel = agreement?.status == AgreementStatus.Accepted;

  $: actions = [
    {
      name: 'Accepter',
      disabled: isLoading,
      visible: canAcceptOrRefuse,
      color: 'success',
      action: () => {}
    },
    {
      name: 'Refuser',
      disabled: isLoading,
      visible: canAcceptOrRefuse,
      color: 'danger',
      action: () => {}
    },
    {
      name: "Annuler l'accord",
      disabled: isLoading,
      visible: canCancel,
      color: 'warning',
      action: () => {}
    }
  ]
</script>

<PageHeader
  title={$page.title}
  actions={actions}
  previous='{previous}'
/>

