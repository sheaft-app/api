<script lang="ts">
  import { getContext, onMount } from "svelte";
  import type { Components } from "$types/api";
  import { mediator } from "$components/mediator";
  import { GetAgreementQuery } from "$components/Agreements/queries/getAgreement";
  import { AgreementOwner, AgreementStatus } from "$components/Agreements/enums";
  import PageHeader from "$components/Page/PageHeader.svelte";
  import RefuseAgreement from "$components/Agreements/Modals/RefuseAgreement.svelte";
  import AcceptSupplierAgreement from "$components/Agreements/Modals/AcceptSupplierAgreement.svelte";
  import AcceptCustomerAgreement from "$components/Agreements/Modals/AcceptCustomerAgreement.svelte";
  import CancelAgreement from "$components/Agreements/Modals/CancelAgreement.svelte";
  import RevokeAgreement from "$components/Agreements/Modals/RevokeAgreement.svelte";

  export let id: string;
  export let title: string = "Détails de l'accord commercial";
  export let goHome = () => {};
  export let previous = () => {
    history.back();
  };

  const { open } = getContext("simple-modal");

  let agreement: Components.Schemas.AgreementDetailsDto = null;

  const openAcceptModal = () => {
    openModal(
      agreement.owner == AgreementOwner.Supplier
        ? AcceptSupplierAgreement
        : AcceptCustomerAgreement
    );
  };

  const openRefuseModal = () => {
    openModal(RefuseAgreement);
  };

  const openCancelModal = () => {
    openModal(CancelAgreement);
  };

  const openRevokeModal = () => {
    openModal(RevokeAgreement);
  };

  const onClose = () => {};

  const openModal = Modal => {
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
  };

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

  $: actions = [
    {
      name: "Accepter",
      disabled: isLoading,
      visible: agreement?.canBeAcceptedOrRefused,
      color: "success",
      action: () => openAcceptModal()
    },
    {
      name: "Refuser",
      disabled: isLoading,
      visible: agreement?.canBeAcceptedOrRefused,
      color: "danger",
      action: () => openRefuseModal()
    },
    {
      name: "Annuler la demande",
      disabled: isLoading,
      visible: agreement?.canBeCancelled,
      color: "warning",
      action: () => openCancelModal()
    },
    {
      name: "Revoquer l'accord",
      disabled: isLoading,
      visible: agreement?.canBeRevoked,
      color: "warning",
      action: () => openRevokeModal()
    }
  ];
</script>

<PageHeader title="{title}" actions="{actions}" previous="{previous}" />
