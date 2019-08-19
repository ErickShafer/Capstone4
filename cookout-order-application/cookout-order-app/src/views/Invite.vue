<template>
  <div>
    <form>
      <h2>Please Enter the Name of Your Event</h2>
      <div class="dropdown">
        <label for="dropdown">Your Events:</label>
        <select name="event" v-model="inviteVM.eventId">
          <option v-for="event in events" v-bind:key="event.id" v-bind:value="event.id">{{event.name}}</option>
        </select>
      </div>

      <h2>Select form guests Below</h2>
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
    </form>
  </div>
</template>

<script>
import auth from "../auth";
import APIService from "../APIService";
const apiService = new APIService();
export default {
  name: "invite",
  data() {
    return {
      events: [],
      guests: [],
      inviteVM: {
        guestIds: [],
        eventId: Number
      }
    };
  },

  methods: {
    async addGuests(inviteVM) {
      this.error = "";
      try {
        let token = await apiService.invite(this.inviteVM);
        const user = auth.getUserWithToken(token);
        let userInfo = await apiService.userInfo(user.sub);
        auth.saveToken(token, userInfo.id);

       
      } catch (error) {
        this.error = error;
      }
      this.$router.push("/homehost");

    },

    check(e) {
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

  created() {
    let user = auth.getUser();
    let id = auth.getUserId();
    this.guestList();
    this.eventList(id);
  }
};
</script>

<style>
</style>
