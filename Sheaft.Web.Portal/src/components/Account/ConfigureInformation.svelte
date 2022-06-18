<script lang="ts">
  import { createForm } from "felte";
  import type { AccountInformation } from "$components/Account/types";
  import type { SelectOption } from "$components/Select/types";
  import { ProfileKind } from "$components/Account/enums";
  import Select from "$components/Select/Select.svelte";
  import Input from "$components/Input/Input.svelte";
  import FormFooter from "$components/Form/FormFooter.svelte";
  import Button from "$components/Button/Button.svelte";

  export let initialValues: AccountInformation | null;
  export let onSubmit;
  export let onBack;

  const { form, data, isSubmitting } = createForm<AccountInformation>({
    initialValues,
    onSubmit
  });

  const accountTypeOptions: SelectOption[] = [
    { label: "Producteur", value: ProfileKind.Supplier },
    { label: "Commerçant", value: ProfileKind.Customer }
  ];
</script>

<form use:form>
  <Select
    id="accountType"
    label="Je suis"
    options="{accountTypeOptions}"
    disabled="{$isSubmitting}"
    bind:value="{$data.accountType}"
    class="mb-6"
  />
  <Input
    id="tradeName"
    label="Nom commercial"
    disabled="{$isSubmitting}"
    bind:value="{$data.tradeName}"
    placeholder="ex: Dupont & Fils"
  />
  <Input
    id="corporateName"
    label="Dénomination légale"
    disabled="{$isSubmitting}"
    bind:value="{$data.corporateName}"
    placeholder="ex: SARL Dupont"
  />
  <Input
    id="siret"
    label="SIRET"
    disabled="{$isSubmitting}"
    bind:value="{$data.siret}"
    placeholder="Votre numéro de SIRET (14 chiffres)"
  />
  <Input
    id="email"
    type="email"
    label="Mail de contact"
    disabled="{$isSubmitting}"
    bind:value="{$data.email}"
    placeholder="Votre adresse mail"
  />
  <Input
    id="phone"
    type="tel"
    label="Téléphone de contact"
    disabled="{$isSubmitting}"
    bind:value="{$data.phone}"
    placeholder="Votre numéro de téléphone"
  />
  <FormFooter>
    <Button
      class="back w-full mx-8"
      disabled="{$isSubmitting}"
      type="button"
      on:click="{() => onBack($data)}"
    >
      Annuler
    </Button>
    <Button class="accent w-full mx-8" isLoading="{$isSubmitting}" type="submit"
      >Suivant
    </Button>
  </FormFooter>
</form>
