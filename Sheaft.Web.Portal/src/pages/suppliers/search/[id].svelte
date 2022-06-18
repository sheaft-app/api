<script lang='ts'>
  import { page, goto } from '@roxi/routify'
  import { onMount } from 'svelte'
  import PageHeader from '$components/Page/PageHeader.svelte'
  import { getContext } from 'svelte';
  import { getSupplierModule } from '$components/Suppliers/module'
  import type { Components } from '$types/api'
  import { mediator } from '$components/mediator'
  import { GetAvailableSupplierQuery } from '$components/Suppliers/queries/getAvailableSupplier'
  import ConfirmAddSupplier from '$components/Suppliers/Modals/ConfirmAddSupplier.svelte'
  import Input from '$components/Input/Input.svelte'
  import Address from '$components/Address/Address.svelte'

  export let id:string;
  
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
    module.goToSearch();
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

<PageHeader
  title={$page.title}
  previous='{() => module.goToSuppliers()}'
  actions='{actions}'/>

<Input
  label='Nom'
  value='{supplier.name}'
  disabled={true} />
<Input
  type='email'
  label='Adresse mail'
  value='{supplier.email}'
  disabled={true} />
<Input
  type='tel'
  label='Numéro de téléphone'
  value='{supplier.phone}'
  disabled={true} />
<Address
  label="Adresse d'expédition"
  disabled='{true}'
  value='{supplier.shippingAddress}' />
