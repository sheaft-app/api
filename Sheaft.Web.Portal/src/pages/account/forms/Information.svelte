<script lang="ts">
  import Select from "$components/Inputs/Select.svelte";
  import Text from "$components/Inputs/Text.svelte";
  import Email from "$components/Inputs/Email.svelte";
  import Phone from "$components/Inputs/Phone.svelte";
  import { ProfileKind } from "$enums/profile";
  import { createForm } from "felte";
  import FormFooter from "$components/Form/FormFooter.svelte";
  import Button from "$components/Buttons/Button.svelte";
  import type { IAccountInformation } from "$commands/account/configureAccount";
  import type { ISelectOption } from "$components/Inputs/types/select";
  import Siret from "$components/Inputs/Siret.svelte";

  export let initialValues: IAccountInformation | null;
  export let onSubmit;
  export let onBack;

  const { form, data, isSubmitting } = createForm<IAccountInformation>({
    initialValues,
    onSubmit
  });

  const accountTypeOptions: ISelectOption[] = [
    { label: "Producteur", value: ProfileKind.Supplier },
    { label: "Commerçant", value: ProfileKind.Customer }
  ];
</script>

<form use:form>
  <Select
    id="accountType"
    label="Je suis"
    options="{accountTypeOptions}"
    isLoading="{$isSubmitting}"
    bind:value="{$data.accountType}"
    class="mb-6"
  />
  <Text
    id="tradeName"
    label="Nom commercial"
    isLoading="{$isSubmitting}"
    bind:value="{$data.tradeName}"
    placeholder="ex: Dupont & Fils"
  />
  <Text
    id="corporateName"
    label="Dénomination légale"
    isLoading="{$isSubmitting}"
    bind:value="{$data.corporateName}"
    placeholder="ex: SARL Dupont"
  />
  <Siret
    id="siret"
    label="SIRET"
    isLoading="{$isSubmitting}"
    bind:value="{$data.siret}"
    placeholder="Votre numéro de SIRET (14 chiffres)"
  />
  <Email
    id="email"
    label="Mail de contact"
    isLoading="{$isSubmitting}"
    bind:value="{$data.email}"
    placeholder="Votre adresse mail"
  />
  <Phone
    id="phone"
    label="Téléphone de contact"
    isLoading="{$isSubmitting}"
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
    <Button class="accent w-full mx-8" disabled="{$isSubmitting}" type="submit"
      >Suivant
    </Button>
  </FormFooter>
</form>
