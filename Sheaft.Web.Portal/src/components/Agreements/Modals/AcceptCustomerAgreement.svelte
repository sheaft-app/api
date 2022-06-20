<script lang="ts">
  import { getContext } from "svelte";
  import Button from "$components/Button/Button.svelte";
  import { type IModalResult, ModalResult } from "$components/Modal/modal";
  import type { Components } from "$types/api";
  import { mediator } from "$components/mediator";
  import { AcceptCustomerAgreementCommand } from "$components/Agreements/commands/acceptCustomerAgreement";
  import { DayOfWeek } from "$enums/days";
  import Checkbox from "$components/Checkbox/Checkbox.svelte";
  import Input from "$components/Input/Input.svelte";

  export let agreement: Components.Schemas.AgreementDetailsDto;
  export let onClose: (result: IModalResult<string>) => {};

  const { close } = getContext("simple-modal");

  let deliveryDays = [];
  let orderDelayed = false;
  let orderDelayInHoursBeforeDeliveryDay: number = 0;

  const validate = async () => {
    try {
      const result = await mediator.send(
        new AcceptCustomerAgreementCommand(
          agreement.id,
          deliveryDays,
          orderDelayInHoursBeforeDeliveryDay
        )
      );
      close();
      await onClose(ModalResult.Success(result));
    } catch (exc) {
      console.error(exc);
    }
  };
</script>

<h2 class="mb-4">Accepter cette demande</h2>
<hr />
<div class="my-6">
  <p>
    Vous vous apprêtez à accepter la demande d'accord commercial proposé par {agreement
      .target.name}. Vous pourrez après cette acceptation recevoir des commandes de leur
    part.
  </p>
</div>

<p>Veuillez selectionner les jours où vous pouvez livrer ce magasin</p>
<ul class="m-3">
  <li>
    <label class="form-check-label inline-block text-gray-800 cursor-pointer">
      <input
        bind:group="{deliveryDays}"
        type="checkbox"
        value="{DayOfWeek.Monday}"
        class="w-4 h-4 m-1" />Lundi</label>
  </li>
  <li>
    <label class="form-check-label inline-block text-gray-800 cursor-pointer">
      <input
        bind:group="{deliveryDays}"
        type="checkbox"
        value="{DayOfWeek.Tuesday}"
        class="w-4 h-4 m-1" />Mardi</label>
  </li>
  <li>
    <label class="form-check-label inline-block text-gray-800 cursor-pointer">
      <input
        bind:group="{deliveryDays}"
        type="checkbox"
        value="{DayOfWeek.Wednesday}"
        class="w-4 h-4 m-1" />Mercredi</label>
  </li>
  <li>
    <label class="form-check-label inline-block text-gray-800 cursor-pointer">
      <input
        bind:group="{deliveryDays}"
        type="checkbox"
        value="{DayOfWeek.Thursday}"
        class="w-4 h-4 m-1" />Jeudi</label>
  </li>
  <li>
    <label class="form-check-label inline-block text-gray-800 cursor-pointer">
      <input
        bind:group="{deliveryDays}"
        type="checkbox"
        value="{DayOfWeek.Friday}"
        class="w-4 h-4 m-1" />Vendredi</label>
  </li>
  <li>
    <label class="form-check-label inline-block text-gray-800 cursor-pointer">
      <input
        bind:group="{deliveryDays}"
        type="checkbox"
        value="{DayOfWeek.Saturday}"
        class="w-4 h-4 m-1" />Samedi</label>
  </li>
  <li>
    <label class="form-check-label inline-block text-gray-800 cursor-pointer">
      <input
        bind:group="{deliveryDays}"
        type="checkbox"
        value="{DayOfWeek.Sunday}"
        class="w-4 h-4 m-1" />Dimanche</label>
  </li>
</ul>
<div class="mt-4">
  <Checkbox
    id="orderDelay"
    bind:value="{orderDelayed}"
    label="Verrouiller la prise de commandes x heures avant le jour de livraison" />
  {#if orderDelayed}
    <Input
      type="number"
      label="Nombre d'heures"
      bind:value="{orderDelayInHoursBeforeDeliveryDay}"
      class="mt-2" />
  {/if}
</div>
<hr />
<div class="flex items-center justify-evenly pt-4">
  <Button class="bg-default-600" on:click="{close}">Fermer</Button>
  <Button class="bg-accent-600" on:click="{validate}">Valider</Button>
</div>
