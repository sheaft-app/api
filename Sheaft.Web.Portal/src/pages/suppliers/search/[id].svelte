<script lang='ts'>
  import { page, goto } from '@roxi/routify'
  import { onMount } from 'svelte'
  import Text from '$components/Inputs/Text.svelte'
  import Address from '$components/Addresses/Address.svelte'
  import PageHeader from '$components/Page/PageHeader.svelte'
  import { getContext } from 'svelte';
  import type { Components } from '$features/api'
  import { mediator } from '$features/mediator'
  import ConfirmAddSupplier from './_ConfirmAddSupplier.svelte'
  import { getSupplierModule } from '$features/suppliers/module'
  import { GetAvailableSupplierQuery } from '$features/suppliers/queries/getAvailableSupplier'
  
  export let id = ''
  
  const module = getSupplierModule($goto)
  const { open } = getContext('simple-modal');

  let initializing = true
  let supplier: Components.Schemas.AvailableSupplierDto = {}

  onMount(async () => {
    try {
      initializing = true
      supplier = await mediator.send(new GetAvailableSupplierQuery(id))
      initializing = false
    } catch (exc) {
      console.error(exc)
      module.goToSuppliers()
    }
  })
  
  const onClose = (result) => {
    module.goToAvailableSuppliers();
  }
  
  const openModal = () => {
    open(ConfirmAddSupplier, {
        supplier,
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
      name:'Acheter ses produits',
      disabled:false,
      visible: true,
      color:'primary',
      action: () => openModal()
    }
  ];
</script>

<!-- routify:options index=true -->
<!-- routify:options title="Fiche du fournisseur" -->
<!-- routify:options roles=[] -->

<PageHeader
  title={$page.title}
  previous='{() => module.goToSuppliers()}'
  actions='{actions}'/>

<Text
  label='Nom'
  value='{supplier.name}'
  disabled={true} />
<Text
  type='email'
  label='Adresse mail'
  value='{supplier.email}'
  disabled={true} />
<Text
  type='tel'
  label='Numéro de téléphone'
  value='{supplier.phone}'
  disabled={true} />
<Address
  label="Adresse d'expédition"
  disabled='{true}'
  value='{supplier.shippingAddress}' />
