<template>
    <Form>
    <h2>Select A New Host</h2>
      <div v-for="guest in guests" v-bind:key="guest.id">
        <input
          type="checkbox"
          v-bind:id= guest.id
          class="box"
          v-bind:value= guest.id
          v-on:click="check"
        />
        {{guest.username}}
        <br />
      </div>
      <br />
      <input type="submit" value="Submit" v-on:click.prevent="addGuests" />
    <button v-on:click.prevent="hideForm">Go Back</button>
    </Form>
</template>

<script>
export default {
    name: "UpdateHost",
    methods: {
    hideForm(){
        this.$emit('remove','');
    }
  }
};

</script>

<style>

</style>