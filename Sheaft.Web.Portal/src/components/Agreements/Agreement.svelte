<script lang="ts">
  import { getContext, onMount } from 'svelte'
  import type { Components } from "$types/api";
  import { mediator } from "$components/mediator";
  import { GetAgreementQuery } from "$components/Agreements/queries/getAgreement";
  import { AgreementOwner, AgreementStatus } from '$components/Agreements/enums'
  import PageHeader from "$components/Page/PageHeader.svelte";
  import RefuseAgreement from '$components/Agreements/Modals/RefuseAgreement.svelte'
  import AcceptSupplierAgreement from '$components/Agreements/Modals/AcceptSupplierAgreement.svelte'
  import AcceptCustomerAgreement from '$components/Agreements/Modals/AcceptCustomerAgreement.svelte'
  import CancelAgreement from '$components/Agreements/Modals/CancelAgreement.svelte'

  export let id: string;
  export let title: string = "Détails de l'accord commercial";
  export let goHome = () => {};
  export let previous = () => {
    history.back();
  };
  
  const { open } = getContext("simple-modal");

  let agreement: Components.Schemas.AgreementDetailsDto = null;

  const openAcceptModal = () => {
    openModal(agreement.owner == AgreementOwner.Supplier ? AcceptSupplierAgreement : AcceptCustomerAgreement);
  };
  
  const openRefuseModal = () => {
    openModal(RefuseAgreement);
  };
  
  const openCancelModal = () => {
    openModal(CancelAgreement);
  };
  
  const onClose = () => {
    
  }
  
  const openModal = (Modal) => {
    open(
      Modal,
      {
        agreement,
        onClose
      },
      {
        closeButton: false,
        closeOnEsc: true,
        closeOnOuterClick: false
      }
    );
  }

  onMount(async () => {
    try {
      isLoading = true;
      agreement = await mediator.send(new GetAgreementQuery(id));
      isLoading = false;
    } catch (exc) {
      goHome();
    }
  });

  $: isLoading = false;
  $: canAcceptOrRefuse = agreement?.status == AgreementStatus.Pending;
  $: canCancel = agreement?.status == AgreementStatus.Active || canAcceptOrRefuse;

  $: actions = [
    {
      name: "Accepter",
      disabled: isLoading,
      visible: canAcceptOrRefuse,
      color: "success",
      action: () => openAcceptModal()
    },
    {
      name: "Refuser",
      disabled: isLoading,
      visible: canAcceptOrRefuse,
      color: "danger",
      action: () => openRefuseModal()
    },
    {
      name: agreement?.status == AgreementStatus.Pending ? "Annuler la demande" : "Revoquer l'accord",
      disabled: isLoading,
      visible: canCancel,
      color: "warning",
      action: () => openCancelModal()
    }
  ];
</script>

<PageHeader title="{title}" actions="{actions}" previous="{previous}" />
