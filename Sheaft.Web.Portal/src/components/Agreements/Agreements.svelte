<script lang='ts'>
  import { afterPageLoad, params } from '@roxi/routify'
  import PageHeader from '$components/Page/PageHeader.svelte'
  import PageContent from '$components/Page/PageContent.svelte'
  import type { Components } from '$types/api'
  import { mediator } from '$components/mediator'
  import { ListActiveAgreementsQuery } from '$components/Agreements/queries/listActiveAgreements'
  import Radio from '$components/Radio/Radio.svelte'
  import { AgreementTab } from '$components/Agreements/enums'
  import { ListSentAgreementsQuery } from '$components/Agreements/queries/listSentAgreements'
  import { ListReceivedAgreementsQuery } from '$components/Agreements/queries/listReceivedAgreements'
  import { formatInnerHtml } from '$actions/format'
  import { dateDistance, dateStr } from '$utils/dates'
  import type { IAgreementModule } from '$components/Agreements/module'
  import { ProfileKind } from '$components/Account/enums'

  export let module: IAgreementModule
  export let pageNumber: number = 1,
    take: number = 10,
    title: string = '',
    profileKind: ProfileKind

  let isLoading = true
  let selectedTab:AgreementTab;
  let agreements: Components.Schemas.AgreementDto[] = []
  let tableCreatedOnHeader:string;
  let noResultsMessage:string;

  const listAgreements = async (query: ListActiveAgreementsQuery | ListSentAgreementsQuery | ListReceivedAgreementsQuery): Promise<void> => {
    try {
      isLoading = true
      agreements = await mediator.send(query)
      isLoading = false
    } catch (exc) {
      module.goToHome()
    }
  }
  
  const actions = [
    {
      name: 'Nouvelle demande',
      disabled: false,
      visible: true,
      color: 'accent',
      action: () => module.goToSearch()
    }
  ]

  $: kind = profileKind == ProfileKind.Customer ? 'magasin' : 'producteur';

  $afterPageLoad(async (page) => {
    selectedTab = <AgreementTab>$params.tab ?? AgreementTab.Active
    
    if (selectedTab == AgreementTab.Active) {
      pageNumber = 1
      tableCreatedOnHeader = 'Depuis le';
      noResultsMessage = `Aucun ${kind} disponible`;
      await listAgreements(new ListActiveAgreementsQuery(pageNumber, take))
    } else if (selectedTab == AgreementTab.Sent) {
      pageNumber = 1
      tableCreatedOnHeader = 'Envoyée le';
      noResultsMessage = `Aucune demande envoyée`;
      await listAgreements(new ListSentAgreementsQuery(pageNumber, take))
    } else if (selectedTab == AgreementTab.Received) {
      pageNumber = 1
      tableCreatedOnHeader = 'Reçue le';
      noResultsMessage = `Aucun demande reçue`;
      await listAgreements(new ListReceivedAgreementsQuery(pageNumber, take))
    }
  })

  $:if (selectedTab)
    module.goToList({ tab: selectedTab })
</script>

<PageHeader title='{title}' actions='{actions}' />
<PageContent isLoading='{isLoading}'>
  <Radio bind:value={selectedTab} values='{[AgreementTab.Active, AgreementTab.Sent, AgreementTab.Received]}' />

  <div class='relative overflow-x-auto shadow-md sm:rounded-lg'>
    <table>
      <thead>
      <tr>
        <th>{kind}</th>
        <th>{tableCreatedOnHeader}</th>
        {#if selectedTab == AgreementTab.Active}
          <th>Mis à jour</th>
        {/if}
      </tr>
      </thead>
      <tbody>
      {#each agreements as agreement}
        <tr on:click='{() => module.goToDetails(agreement.id)}'>
          <th>{agreement.targetName}</th>
          <td>{dateStr(agreement.createdOn, "dd/MM/yyyy")}</td>
          {#if selectedTab == AgreementTab.Active}
            <td use:formatInnerHtml='{dateDistance}'>{agreement.updatedOn}</td>
          {/if}
        </tr>
      {/each}
      {#if agreements?.length < 1}
        <tr>
          <td colspan='3' class='text-center'>{noResultsMessage}</td>
        </tr>
      {/if}
      </tbody>
    </table>
  </div>
</PageContent>
