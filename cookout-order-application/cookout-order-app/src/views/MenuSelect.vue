<template>
  <div>
    <form>
      <h2>Please Select the Name of Your Event</h2>
      <div class="dropdown">
        <label for="dropdown">Your Events:</label>
        <select name="event" v-model="menuVM.eventId">
          <option
            v-for="event in events"
            v-bind:key="event.id"
            v-bind:value="event.id"
          >{{event.name}}</option>
        </select>
      </div>

      <h2 class="menu-title-card">Select from the Menu Items Below</h2>
      <div class="menu-list">
        <div class="menu-item" v-for="item in Items" v-bind:key="item.id">
          <input
            type="checkbox"
            v-bind:id="item.itemID"
            class="box"
            v-bind:value="item.itemID"
            v-on:click="check"
          />
          {{item.itemName}}
          <br />
        </div>
      </div>
      <br />
      <button value="Submit" v-on:click.prevent="addMenu">Submit</button>
    </form>
  </div>
</template>

<script>
import auth from "../auth";
import APIService from "../APIService";
const apiService = new APIService();
export default {
  name: "menuSelect",
  data() {
    return {
      events: [],
      Items: [],
      menuVM: {
        itemIds: [],
        eventId: Number
      }
    };
  },

  methods: {
    async addMenu(menuVM) {
      this.error = "";
      try {
        let token = await apiService.menu(this.menuVM);
        const user = auth.getUserWithToken(token);
        let userInfo = await apiService.userInfo(user.sub);
        auth.saveToken(token, userInfo.id);
        this.goHome(user);
      } catch (error) {
        this.error = error;
      }
     this.$router.push("/homehost");

    },

    check(e) {
      if (e.target.checked) {
        this.menuVM.itemIds.push(parseInt(e.target.id));
      } else {
        this.menuVM.itemIds = this.menuVM.itemIds.filter(item => {
          return item != e.target.id;
        });
      }
    },

    async eventList(data) {
      this.error = "";
      try {
        let eventsL = await apiService.myevents(data);
        this.events = eventsL;
      } catch (error) {
        this.error = error;
      }
    },

    async itemList() {
      this.error = "";
      try {
        let itemsL = await apiService.items();
        this.Items = itemsL;
      } catch (error) {
        this.error = error;
      }
    }
  },

  created() {
    let user = auth.getUser();
    let id = auth.getUserId();
    this.itemList();
    this.eventList(id);
  }
};
</script>

<style>
.menu-list {
  width: 450px;
  background: #fff;
  margin: 50px auto;
  margin-top: unset;
  margin-bottom: unset;
}
.menu-item {
  font-size: 16px;
  border-bottom: 1px solid #f2f2f2;
  padding: 10px 20px;
  cursor: pointer;
  text-align: left;
}
.menu-title-card {
  width: 450px;
  height: 50px;
  background: #e4836c;
  margin: 50px auto;
  margin-top: 20px;
  margin-bottom: unset;
  padding-top: 10px;
}
label {
font-size: 19px;
}
select {
  margin: 10px;
  
}
</style>
