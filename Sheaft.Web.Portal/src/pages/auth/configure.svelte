<script lang='ts'>
  import { page, goto, params } from '@roxi/routify'
  import Button from '$components/Buttons/Button.svelte'
  import { configureCustomer, configureSupplier } from '$stores/auth'
  import Select from '$components/Inputs/Select.svelte'
  import Text from '$components/Inputs/Text.svelte'
  import Siret from '$components/Inputs/Siret.svelte'
  import Email from '$components/Inputs/Email.svelte'
  import Phone from '$components/Inputs/Phone.svelte'
  import Stepper from '$components/Stepper/Stepper.svelte'
  import Checkbox from '$components/Inputs/Checkbox.svelte'

  export let account: { email: string, password: string, confirm: string } = {}
  let isLoading: boolean = false
  let accountType: string

  let accountTypeOptions = [
    { label: 'Producteur', value: 'supplier' },
    { label: 'Commerçant', value: 'customer' }
  ]

  let steps = [
    {
      name: 'Informations',
      icon: 'fas#AddressCard',
      position: 0
    },
    {
      name: 'Légal',
      icon: 'fas#Gavel',
      position: 1
    },
    {
      name: 'Localisation',
      icon: 'fas#Compass',
      position: 2
    }
  ]

  let currentStep = 0;

  const configureAccount = async () => {
    try {
      if (!accountType)
        return

      isLoading = true

      const result = accountType == 'customer' ? await configureCustomer(account) : await configureSupplier(account)
      if (!result) {
        isLoading = false
        return
      }

      if ($params.returnUrl)
        $goto($params.returnUrl)
      else
        $goto('/')

    } catch (e) {
      isLoading = false
    }
  }
</script>

<!-- routify:options title="Renseigner vos informations" -->

<div class='flex justify-center items-center flex-wrap h-full g-6 text-gray-800'>
  <div class='md:w-8/12 lg:w-6/12 mb-12 md:mb-0'>
    <img data-ujs-name='Sign in' />
  </div>
  <div class='md:w-8/12 lg:w-5/12 lg:ml-20'>
    <h1>{$page.title}</h1>
    <form>
      <Stepper {steps} bind:activeStep={currentStep}></Stepper>
      {#if currentStep == 0}
        <Select label='Test' options={accountTypeOptions} {isLoading} bind:value={accountType} />
        <Text placeholder='Nom commercial' />
        <Email placeholder='Adresse mail de contact' />
        <Phone placeholder='Numéro de téléphone de contact' />
      {:else if currentStep == 1}
        <Text placeholder='Nom légal' />
        <Siret placeholder='Votre SIRET' />
        <Text placeholder='Adresse de votre siège' />
        <Text placeholder="Complément d'adresse" />
        <Text placeholder='Code postal' />
        <Text placeholder='Ville' />
      {:else}
        <Checkbox label="L'adresse de facturation est différente" bind:value={hasDifferentBillingAddress}/>
        <Checkbox label="L'adresse de livraison est différente" bind:value={hasDifferentDeliveryAddress}/>
      {/if}
      <Button type='submit' {isLoading} on:click='{configureAccount}' class='primary w-full mt-8'
      >Valider
      </Button
      >
    </form>
  </div>
</div>
