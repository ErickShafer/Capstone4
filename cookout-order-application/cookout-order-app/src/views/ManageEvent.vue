<template>
  <div>
    
      <h2 >Your Event: {{selectionName}}</h2>
    <form>
      <h2>What would you like to Change?</h2>
      <button label="Guest-List" class="guest" v-on:click.prevent="update('update-guests')">Guest List</button>
      <button label="Menu" class="menu" v-on:click.prevent="update('update-menu')">Menu</button>
      <button label="Date and Time" class="info" v-on:click.prevent="update('update-info')">Date/Time</button>
    </form>
    <component v-bind:is="showClass" @remove="setNone()"></component>
    
  </div>
</template>

<script>
import auth from "../auth";
import APIService from "../APIService";
const apiService = new APIService();
import UpdateHost from "../components/UpdateHost";
import UpdateGuests from "../components/UpdateGuests.vue";
import UpdateMenu from "../components/UpdateMenu.vue";
import UpdateInfo from "../components/UpdateInfo.vue";
export default {
  name: "manage",
  data() {
    return {
      events: [],
      selection: Number,
      selectionName: String,
      showClass: '',
    };
  },
  components: {
    UpdateHost,
    UpdateGuests,
    UpdateMenu,
    UpdateInfo
  },
  methods: {
    setNone(){
      this.showClass = '';
    },
    update(data) {
      this.showClass = data;
    },

    async slectEvent(){
        this.selection = auth.getEventId();
        this.selectionName = await apiService.findName(this.selection);
    },

    async eventList(userName) {
      this.error = "";
      try {
        let eventsL = await apiService.myevents(userName);
        this.events = eventsL;
      } catch (error) {
        this.error = error;
      }
    }
  },
  created() {
    let user = auth.getUser();
    let id = auth.getUserId();
    this.eventList(id);
    this.slectEvent();
  }
  
};
</script>

<style>
</style>