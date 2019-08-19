<template>
  <Form>
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
        {{item.itemName}}{{eId}}
        <br />
      </div>
    </div>
    <br />
    <button value="Submit" v-on:click.prevent="updateMenu">Submit</button>
    <button v-on:click.prevent="hideForm">Go Back</button>
  </Form>
</template>

<script>
import auth from "../auth";
import APIService from "../APIService";
const apiService = new APIService();
export default {
  name: "UpdateMenu",
 

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
    hideForm() {
      this.$emit("remove", "");
    },

    async updateMenu(menuVM) {
      this.error = "";
      try {
        let token = await apiService.updatemenu(this.menuVM);
        const user = auth.getUserWithToken(token);
        let userInfo = await apiService.userInfo(user.sub);
        auth.saveToken(token, userInfo.id);
        this.goHome(user);
      } catch (error) {
        this.error = error;
      }
      this.$router.push("/homehost");
    },

    updateEvent() {
      this.menuVM.eventId = this.$parent.selection;
    },

    check(e) {
      this.updateEvent();
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
  },

  
};
</script>

<style>
</style>