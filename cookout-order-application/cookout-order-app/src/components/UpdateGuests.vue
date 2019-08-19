<template>
    <form>
      <h2 class="menu-title-card">Select from guests Below</h2>
      <div class="menu-list">
      <div class="menu-item" v-for="guest in guests" v-bind:key="guest.id">
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
      </div>
      <br />

      <button v-on:click.prevent="updateGuests" >Submit</button>
    <button v-on:click.prevent="hideForm">Go Back</button>
  </Form>
</template>
<script>

import auth from "../auth";
import APIService from "../APIService";
const apiService = new APIService();
export default {
  name: "UpdateGuest",
  data() {
    return {
      events: [],
      guests: [],
      inviteVM: {
        guestIds: [],
        eventId: Number
      }
    }
  },
  
  methods: {
    hideForm(){
        this.$emit('remove','');
    },

    updateEvent() {
      this.inviteVM.eventId = this.$parent.selection;
    },

    async updateGuests(inviteVM) {
      this.error = "";
      try {
        let token = await apiService.updateguests(this.inviteVM);
        const user = auth.getUserWithToken(token);
        let userInfo = await apiService.userInfo(user.sub);
        auth.saveToken(token, userInfo.id);       
      } catch (error) {
        this.error = error;
      }
      this.$router.push("/homehost");
    },

    check(e) {
      this.updateEvent();
      if (e.target.checked) {
        this.inviteVM.guestIds.push(parseInt(e.target.id));
      } else {
        this.inviteVM.guestIds = this.inviteVM.guestIds.filter(item => {
          return item != e.target.id;
        });
      }
    },

    async eventList(userName) {
      this.error = "";
      try {
        let eventsL = await apiService.myevents(userName);
        this.events = eventsL;
      } catch (error) {
        this.error = error;
      }
    },

    async guestList() {
      this.error = "";
      try {
        let itemsL = await apiService.guests();
        this.guests = itemsL;
      } catch (error) {
        this.error = error;
      }
    }
  },

  created(){
    let user = auth.getUser();
    let id = auth.getUserId();
    this.guestList();
    this.eventList(id);
  }
};
</script>

<style>
</style>