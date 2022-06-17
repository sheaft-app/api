<script lang="ts">
  import Select from "$components/Inputs/Select.svelte";
  import Text from "$components/Inputs/Text.svelte";
  import { ProfileKind } from "$components/Auth/profile";
  import { createForm } from "felte";
  import FormFooter from "$components/Form/FormFooter.svelte";
  import Button from "$components/Buttons/Button.svelte";
  import type { ISelectOption } from "$components/Inputs/types/select";
  import type { IAccountInformation } from '$features/account/types'

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
    disabled="{$isSubmitting}"
    bind:value="{$data.accountType}"
    class="mb-6"
  />
  <Text
    id="tradeName"
    label="Nom commercial"
    disabled="{$isSubmitting}"
    bind:value="{$data.tradeName}"
    placeholder="ex: Dupont & Fils"
  />
  <Text
    id="corporateName"
    label="Dénomination légale"
    disabled="{$isSubmitting}"
    bind:value="{$data.corporateName}"
    placeholder="ex: SARL Dupont"
  />
  <Text
    id="siret"
    label="SIRET"
    disabled="{$isSubmitting}"
    bind:value="{$data.siret}"
    placeholder="Votre numéro de SIRET (14 chiffres)"
  />
  <Text
    id="email"
    type='email'
    label="Mail de contact"
    disabled="{$isSubmitting}"
    bind:value="{$data.email}"
    placeholder="Votre adresse mail"
  />
  <Text
    id="phone"
    type='tel'
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
    <Button 
      class="accent w-full mx-8"             
      isLoading="{$isSubmitting}" 
      type="submit"
      >Suivant
    </Button>
  </FormFooter>
</form>
