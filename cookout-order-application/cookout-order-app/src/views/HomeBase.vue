<template>
  <div class="home">
    <h2>Select an Event from the List Below</h2>

    <div v-for="event in events" v-bind:key="event.name">
      <div class="event-card">
        <div class="event-title">
          <p class="title">Name:</p>
          <p v-on:click="go(event.id)">{{event.name}}</p>
          </div>
          <div class="event-date"><p class="title">Date:</p>
          <p>{{event.date}}</p>
          </div>
          <div class="event-flex">
            <div class="event-details">
            <p class="title">Description:</p><p>{{event.description}}</p>
        </div>
        <div class="detail-divider"></div>
        <div class="event-role"><p class="title">Role:</p><p>Guest</p></div>
        </div>
        <button class="detail-button" v-on:click="goOrder(event.id)">Place Order</button>
        <button class="detail-button" v-on:click="goQueue(event.id)">View Order Queue</button>

      </div>
    </div>
    <button class="account-button" v-on:click.left="upgradeRole">Upgrade to Host Account</button>
  </div>
</template>

<script>
import auth from "../auth";
import APIService from "../APIService";
import { ok } from 'assert';
const apiService = new APIService();

export default {
  name: "HomeBase",
  components: {},
  methods: {
    goOrder(id){
      auth.saveEventId(id);
      this.$router.push("/eventguest");
    },
    goQueue(id){
      auth.saveEventId(id);
      this.$router.push("/queue");
    },
    goGuest(id){
      auth.saveEventId(id);
      this.$router.push("/eventguest");
    },
    async eventList(userName) {
      this.error = "";
      try {
        let eventsL = await apiService.guestEvents(userName);
        this.events = eventsL;
        
      } catch (error) {
        this.error = error;
      }
    },
   async upgradeRole() {
    let id = auth.getUserId() 
    let result = await apiService.upgradeUser(id);
    this.$router.push("/homehost");
   }
  },
  data() {
    return {
      events: [
      ]
    };
  },
  beforeMount() {
    let user = auth.getUser();
    this.eventList(user.sub);
  }
};
</script>

<style scoped>
body {
  background: #f8d19a;
  color: #661308;
}
h2 {
  color: #661308;
  font-family: "Courgette", cursive;
}
.event-card {
  width: 60%;
  height: auto;
  margin: 25px auto;
  padding: unset;
  border-radius: unset;
  background: white;
  display: inline-flex;
  flex-direction: vertical;
  flex-wrap: wrap;
  left: 18%;
  font-family: "Amaranth", sans-serif;
}
.event-title {
  width: 100%;
  margin: 10px 0;
  margin-top: unset;
  height: 50px;
  text-align: center;
  display: flex;
  background: #E4836C;
  padding: 10px 0;
  margin-bottom: unset;
  border-radius: unset;
  cursor: pointer;
}
.event-title >  .title {
  font-size: 18px;
  font-weight: bold;
  font-family: "Amaranth", sans-serif;
  padding-right: 10px;
  padding-left: 10px;
  
}
.event-title > p {
  font-size: 18px;
}

.event-date {
  width: 100%;
  padding: 10px 0;
  padding-top: 15px;
  display: flex;
  padding-bottom: unset;
}
.event-date > .title {
  font-size: 15px;
  font-weight: bold;
  padding-right: 10px;
  padding-left: 15px;
  margin-bottom: unset;
  margin-top: unset;
}
.event-date > p {
  margin-bottom: unset;
  margin-top: unset;
}
.event-flex {
  width: 100%;
  display: flex;
  background: white;
  margin: 15px;
  margin-top: unset;
  margin-bottom: unset;
}
.event-details {
width: 75%;
  height: auto;
  margin-left: unset;
  border-style: none;
  margin-bottom: unset;
  margin-top: unset;
  padding-left: unset;
}
.event-details >  .title {
  font-size: 15px;
  font-weight: bold;
  padding-right: 10px;
  text-align: left;
  margin-top: unset;
}
.event-details > p {
  text-align: left;
}
.event-role{
  width: 20%;
  height: auto;
  text-align: center;
  
}
.event-role >  .title {
  font-size: 15px;
  font-weight: bold;
 background: #FEFEFD;
 margin-top: unset;
}
.detail-divider {
     border-left:1px solid #e7963d; 
     border-right:1px solid #e7963d; 
     height:80%;
     position:relative;
     right:20px;
     top:10px; 
}
.detail-button {
  background-color: #e7963d;
  width: 150px;
  height: 40px;
  margin: 15px;
  margin-top: unset;
}

.account-button {
  padding: 5px;
  font-size: 15px;
  font-weight: 400;
  border-radius: 8px;
  background-color: #e7963d;
  color: black;
  cursor: pointer;
  border: none;
  outline: none;
  width: 150px;
  bottom: 20px;
  left: 15%;
  
  
}
@media only screen and (max-width: 800px) {
  .event-card{
    width: unset;
  }
  .detail-divider{
    right: 5px;
  }
 
}


</style>
