import { goto } from '@roxi/routify'

export const goToCreate = () => {
  $goto('/returnables/create');
}

export const goToDetails = (id:string) => {
  $goto(`/returnables/${id}`);
}

export const goToList = () => {
  $goto('/returnables/');
}
