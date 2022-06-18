<script lang="ts">
  import { page, goto } from "@roxi/routify";
  import { onMount } from "svelte";
  import { getContext } from "svelte";
  import { getCustomerModule } from "$components/Customers/module";
  import type { Components } from "$types/api";
  import { mediator } from "$components/mediator";
  import { GetAvailableCustomerQuery } from "$components/Customers/queries/getAvailableCustomer";
  import type { IModalResult } from "$components/Modal/modal";
  import ConfirmAddCustomer from "$components/Customers/Modals/ConfirmAddCustomer.svelte";
  import PageHeader from "$components/Page/PageHeader.svelte";
  import AvailableProfile from "$components/Agreements/AvailableProfile.svelte";

  export let id: string;

  const module = getCustomerModule($goto);
  const { open } = getContext("simple-modal");

  let initializing = true;
  let customer: Components.Schemas.AvailableCustomerDto = {};

  onMount(async () => {
    try {
      initializing = true;
      customer = await mediator.send(new GetAvailableCustomerQuery(id));
      initializing = false;
    } catch (exc) {
      console.error(exc);
      module.goToCustomers();
    }
  });

  const onClose = (result: IModalResult<string>) => {
    module.goToSearch();
  };

  const openModal = () => {
    open(
      ConfirmAddCustomer,
      {
        customer,
        onClose
      },
      {
        closeButton: false,
        closeOnEsc: true,
        closeOnOuterClick: false
      }
    );
  };

  const actions = [
    {
      name: "Proposer mes produits",
      disabled: false,
      visible: true,
      color: "primary",
      action: () => openModal()
    }
  ];
</script>

<!-- routify:options index=true -->
<!-- routify:options title="Fiche du client" -->

<PageHeader
  title="{$page.title}"
  previous="{() => module.goToCustomers()}"
  actions="{actions}"
/>

<AvailableProfile profile="{customer}" />
