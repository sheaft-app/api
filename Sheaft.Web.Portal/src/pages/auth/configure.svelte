<script lang='ts'>
  import { page, goto, params } from '@roxi/routify'
  import Email from '$components/Inputs/Email.svelte'
  import Password from '$components/Inputs/Password.svelte'
  import Button from '$components/Buttons/Button.svelte'
  import { configureCustomer, configureSupplier } from '$stores/auth'

  export let account: {email:string, password:string, confirm:string} = {}
  let isLoading: boolean = false
  let accountType: string;

  const configureAccount = async () => {
    try {
      if(!accountType)
        return;
      
      isLoading = true

      const result = accountType == 'customer' ? await configureCustomer(account) : await configureSupplier(account);
      if (!result){
        isLoading = false;
        return;
      }

      if($params.returnUrl)
        $goto($params.returnUrl);
      else
        $goto('/');
      
    } catch (e) {
      isLoading = false
    }
  }
</script>

<!-- routify:options redirectIfAuthenticated=true -->
<!-- routify:options public=true -->
<!-- routify:options title="Renseigner vos informations" -->

<div class='flex justify-center items-center flex-wrap h-full g-6 text-gray-800'>
  <div class='md:w-8/12 lg:w-6/12 mb-12 md:mb-0'>
    <img data-ujs-name='Sign in' />
  </div>
  <div class='md:w-8/12 lg:w-5/12 lg:ml-20'>
    <h1>{$page.title}</h1>
    <form>
      <Button type='submit' {isLoading} on:click='{configureAccount}' class='primary w-full mt-8'
      >Valider
      </Button
      >
    </form>
  </div>
</div>
