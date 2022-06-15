<script lang='ts'>
  import { page, goto } from '@roxi/routify'
  import { onMount } from 'svelte'
  import Text from '$components/Inputs/Text.svelte'
  import Address from '$components/Addresses/Address.svelte'
  import PageHeader from '$components/Page/PageHeader.svelte'
  import { getContext } from 'svelte';
  import { getAgreementModule } from '$features/agreements/module'
  import type { Components } from '$features/api'
  import { mediator } from '$features/mediator'
  import { GetAvailableCustomerQuery } from '$features/agreements/queries/getAvailableCustomer'
  import ConfirmAddCustomer from '$pages/customers/_modals/ConfirmAddCustomer.svelte'
  import type { IModalResult } from '$components/Modal/types'
  
  export let id = ''
  
  const module = getAgreementModule($goto)
  const { open } = getContext('simple-modal');

  let initializing = true
  let customer: Components.Schemas.AvailableCustomerDto = {}

  onMount(async () => {
    try {
      initializing = true
      customer = await mediator.send(new GetAvailableCustomerQuery(id))
      initializing = false
    } catch (exc) {
      console.error(exc)
      module.goToCustomers()
    }
  })
  
  const onClose = (result:IModalResult<string>) => {
    module.goToAvailableCustomers();
  }
  
  const openModal = () => {
    open(ConfirmAddCustomer, {
        customer,
        onClose
      },
      {
        closeButton: false,
        closeOnEsc: true,
        closeOnOuterClick: false,
      });
  }

  const actions = [
    {
      name:'Proposer mes produits',
      disabled:false,
      color:'primary',
      action: () => openModal()
    }
  ];
</script>

<!-- routify:options index=true -->
<!-- routify:options title="Fiche du magasin" -->
<!-- routify:options roles=[] -->

<PageHeader
  title={$page.title}
  previous='{() => module.goToCustomers()}'
  actions='{actions}'/>

<Text
  label='Nom'
  value='{customer.name}'
  disabled={true} />
<Text
  type='email'
  label='Adresse mail'
  value='{customer.email}'
  disabled={true} />
<Text
  type='tel'
  label='Numéro de téléphone'
  value='{customer.phone}'
  disabled={true} />
<Address
  label='Adresse de livraison'
  disabled='{true}'
  value='{customer.deliveryAddress}' />
