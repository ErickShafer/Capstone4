<template>
  <div>
    <form>
       <h2 >Please Select the Name of Your Event</h2>
      <div class="input-group mb-3">
      <div class="input-group-prepend">
      <div class="dropdown-menu">
        <select id="menu-bits" name="event dropdown-item" v-model="selection">
          <option
            v-for="event in events"
            v-bind:key="event.id"
            v-bind:value="event.id"
          >{{event.name}}</option>
        </select>
        </div>
      </div>
      </div>
    </form>
    
    <form v-show="selection>0 && showClass==''">
      <h2>What would you like to Change?</h2>
      <button style= border-color:black label="Guest-List" class="guest" v-on:click.prevent="update('update-guests')">Guest List</button>
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
      selectionName: String,
      selection: Number,
      showClass: ''
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

    async update(data) {
      this.showClass = data;
      this.selectionName = await apiService.findName(this.selection)
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
  }
};
</script>

<style scoped>
/* .input-group{
  background: #e4836c;
} */

button{
 box-shadow: -1px 1px 3px 0px black;
}

#menu-bits{
  background-color: #e7963d;
  height: 40px;
  border-radius: 5px;
  border-color: #661308;
  font-family: "Amaranth", sans-serif;
  font-size: 16px;
}
</style>