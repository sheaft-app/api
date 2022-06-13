<script lang='ts'>
  import { page, goto } from '@roxi/routify'
  import { onMount } from 'svelte'
  import { mediator } from '$services/mediator'
  import { getCustomerModule } from '$pages/customers/module'
  import type { Components } from '$types/api'
  import { GetAvailableCustomerQuery } from '$queries/customers/getAvailableCustomer'
  import Text from '$components/Inputs/Text.svelte'
  import Email from '$components/Inputs/Email.svelte'
  import Phone from '$components/Inputs/Phone.svelte'
  import Address from '$components/Inputs/Address.svelte'
  import PageHeader from '$components/Page/PageHeader.svelte'
  import { getContext } from 'svelte';
  import ConfirmAddCustomer from './../components/ConfirmAddCustomer.svelte'
  
  export let id = ''
  
  const module = getCustomerModule($goto)
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
  
  const openModal = () => {
    open(ConfirmAddCustomer, {
        customer
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
<Email
  label='Adresse mail'
  value='{customer.email}'
  disabled={true} />
<Phone
  label='Numéro de téléphone'
  value='{customer.phone}'
  disabled={true} />
<Address
  label='Adresse de livraison'
  disabled='{true}'
  value='{customer.deliveryAddress}' />
