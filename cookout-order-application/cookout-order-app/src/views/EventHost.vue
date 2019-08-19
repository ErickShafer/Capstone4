<template>
  <div>
    
     <div class="routes">
    <div class="que-div pseudoButton">
      <h2 ><router-link to="/orders">Order Queue</router-link></h2>
    </div>
   
    <div class="manage-div pseudoButton">
      <h2><router-link to="/manageevent">Manage Event</router-link></h2>
    </div>
    </div>
    <div class="details-div">
      <div class="header-div">
        <h2>Event Name: {{event.name}}</h2>
      </div>
      <div class="details-content">
      <h3>Date:</h3>
      <p class="event-date">{{event.date}}</p>
      <h3>Description:</h3>
      <p class="event-description">{{event.description}}</p>
      </div>
    </div>
    <div class="guest-div">
      <div class="header-div">
        <h2>Guest List</h2>
      </div>
      
      <div class="guest-list">
        <ul v-for="guest in this.Guests.guestName" v-bind:key="guest">
          <li>{{guest}}</li>
        </ul>
      </div>
    </div>
    <div class="menu-div">
      <div class="header-div">
        <h2>Menu</h2>
      </div>
      <div class="menu-list">
        <ul v-for="item in event.menu" v-bind:key="item">
          <li>{{item.name}}</li>
        </ul>
      </div>
    </div>
   
  </div>
</template>

<script>
import auth from "../auth";
import APIService from "../APIService";
const apiService = new APIService();

export default {
  name: "EventHost",
  userId: "",
  data() {
    return {
      event: {},
      Guests: {}
    };
  },
  methods: {
    async addGuest(eventId) {
      this.error = "";
      try {
        let guests = await apiService.eventGuestListVM(eventId);
        this.Guests = guests;
      } catch (error) {
        this.error = error;
      }
    },

    async itemList(eventId) {
      this.error = "";
      try {
        let itemsL = await apiService.guestEventVM(eventId);
        this.event = itemsL;
      } catch (error) {
        this.error = error;
      }
    }
  },
  beforeMount() {
    const eventId = auth.getEventId();
    this.itemList(eventId);
    this.addGuest(eventId);
  }
};
</script>

<style scoped>
.pseudoButton{
  box-shadow: -1px 1px 3px 0px black;
}
.routes{
  display : flex;
  padding-top: 5%;
  justify-content: center;
}
.details-div {
  width: 60%;
  margin-left: 20%;
  margin-right: 15%;
  margin-top: 25px;
  padding-bottom: 10px;
  background: white;
}
.details-div h2 {
  margin: unset;
  padding-top: 7px;
}
.details-div h3 {
  width: 70px;
  margin: unset;
  padding: 5px;
}
.details-div p {
  position: relative;
  padding: 10px;
  margin: unset;
  padding-top: unset;
  width: 400px;
}
.details-content {
  padding: 10px;
}
.menu-div {
  width: 60%;
  margin-left: 20%;
  margin-right: 15%;
  padding: unset;
  background: white;
}
/* .menu-div li {
  width: 60%;
  margin-left: 20%;
  margin-right: 15%;
} */
.header-div {
  height: auto;
  background: #e4836c;
  margin: unset;
}
.menu-div h2 {
  margin: unset;
  padding-top: 7px;
}

.menu-list {
  background: white;
  margin-left: unset;
  margin-right: unset;
  width: 70%;
}

li {
  list-style-type: none;
  font-size: 16px;
  border-bottom: 1px solid #f2f2f2;
  padding: 10px 20px;
  padding-top: 5px;
  cursor: pointer;
  text-align: left;
}
.guest-div {
  width: 60%;
  margin-left: 20%;
  margin-right: 15%;
  margin-top: 25px;
  padding: unset;
  background: white;
}
.guest-div h2 {
  margin: unset;
  padding-top: 7px;
}
.guest-list {
  background: white;
  width: 70%;
}
.que-div {
  background: #e4836c;
  width: 30%;
  padding: 5px;
  height: auto;
  margin-right: 5%;
  margin-top: 10px;
  margin-bottom: 10px;
  
  cursor:pointer;
  border-radius: 3px;
}
.que-div h2 {
  margin-top: unset;
  margin-bottom: unset;
  
}
.que-div h2.active {
  color: #661308;
}
.place-order-div {
  background: #e4836c;
  width: 30%;
  padding: 5px;
  margin-left: 35%;
  margin-right: 30%;
  margin-top: 10px;
  margin-bottom: 10px;
  height: 30px;
  cursor:pointer;
  border-radius: 3px;
}
.place-order-div h2 {
  margin-top: unset;
  margin-bottom: unset;
}
.manage-div {
  background: #e4836c;
  width: 30%;
  padding: 5px;
  margin-left: 5%;
  
  margin-top: 10px;
  margin-bottom: 10px;
  
  cursor:pointer;
  border-radius: 3px;
}
.manage-div h2 {
  margin-top: unset;
  margin-bottom: unset;
}
.manage-div h2 a {
  color: #661308;
}
.que-div h2 a {
  color: #661308;
}
@media only screen and (max-width: 800px) {
  .details-div {
    width: unset;
    margin-left:unset;
    margin-right: unset;
  }
  .details-div p{
    height: unset;
    width: unset;
  }
  .que-div{
    height: unset;
  }
  .manage-div{
    height: unset;
  }
  .guest-div{
    width: unset;
    margin-left:unset;
    margin-right: unset;
  }
  .menu-div{
    width: unset;
    margin-left:unset;
    margin-right: unset;
  }
}
</style>
