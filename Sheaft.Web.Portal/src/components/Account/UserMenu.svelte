<script lang="ts">
  import Fa from "svelte-fa";
  import { faRightFromBracket, faUserLock } from "@fortawesome/free-solid-svg-icons";
  import { goto } from "@roxi/routify";
  import { mediator } from "$components/mediator";
  import { LogoutUserCommand } from "$components/Account/commands/logoutUser";
  import { authStore } from "$components/Account/store";
  import Button from "$components/Button/Button.svelte";

  const logoutUser = async () => {
    await mediator.send(new LogoutUserCommand());
  };

  const loginUser = () => {
    $goto("/account/login");
  };
</script>

<div class="{$$props.class} text-gray-600">
  {#if $authStore.isAuthenticated}
    <div class="flex items-center justify-between border-t">
      <div class="flex cursor-pointer items-center justify-center my-4 ml-4">
        <img
          class="w-10 h-10 rounded-full border border-gray-100 shadow-sm"
          alt="profile"
          src="https://images.unsplash.com/photo-1652117137751-ed18b1ff20da?ixlib=rb-1.2.1&ixid=MnwxMjA3fDB8MHx0b3BpYy1mZWVkfDE0fHRvd0paRnNrcEdnfHxlbnwwfHx8fA%3D%3D&auto=format&fit=crop&w=200&q=60" />
        <div class="ml-4">
          <p class="overflow-hidden overflow-ellipsis font-medium">
            {$authStore.account?.name}
          </p>
          <small class="overflow-hidden overflow-ellipsis"
            ><i>{$authStore.account?.profile?.name ?? ""}</i></small>
        </div>
      </div>
      <div on:click="{logoutUser}" class="hover:text-primary-700 cursor-pointer">
        <Fa icon="{faRightFromBracket}" class="m-4" />
      </div>
    </div>
  {:else}
    <Button type="button" class="accent" on:click="{loginUser}" icon="{faUserLock}">
      Se connecter
    </Button>
  {/if}
</div>
