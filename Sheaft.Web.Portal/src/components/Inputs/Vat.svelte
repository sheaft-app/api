<script lang="ts">
  import "./inputs.scss";
  import { nanoid } from "nanoid";

  export let value: boolean = false;
  export let label: string = null;
  export let isLoading: boolean = false;
  export let required: boolean = true;
  
</script>

<div class="my-4">
    <label class='block mb-2 text-sm font-medium text-gray-700 '>{label} {required ? '*' : ''}</label>
  <div class='flex'>
    <div class='f-radio'>
      <input type='radio' bind:group={value} name="55" id='55' class='peer' value={55} />
      <label
        for='55'
        class='peer-checked:bg-primary-500 peer-checked:font-bold peer-checked:text-white hover:peer-checked:bg-primary-600'
      >5,5%</label>
    </div>

    <div class='f-radio'>
      <input type='radio' bind:group={value} name="100" id='100' class='peer' value={100} />
      <label
        for='100'
        class='peer-checked:bg-primary-500 peer-checked:font-bold peer-checked:text-white hover:peer-checked:bg-primary-600'
      >10%</label>
    </div>

    <div class='f-radio'>
      <input type='radio' bind:group={value} name="200" id='200' class='peer' value={200} />
      <label
        for='200'
        class='peer-checked:bg-primary-500 peer-checked:font-bold peer-checked:text-white hover:peer-checked:bg-primary-600'
      >20%</label>
    </div>
  </div>
</div>
