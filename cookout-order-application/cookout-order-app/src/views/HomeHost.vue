<template>
  <div class="home">
    <h2>Select an Event from the List Below</h2>
    <div class="button-div">
<button class="account-button" v-on:click.left="createEvent">Create Event</button>
<button class="manage-button"><router-link to="/manage">Manage Events</router-link></button>
    </div>
    <div v-for="event in events" v-bind:key="event">
      <div class="event-card">
        <div class="event-title">
          <p class="title">Name:</p>
          <p v-on:click="go(event.id)">{{event.name}}</p>
        </div>
        <div class="event-date">
          <p class="title">Date:</p>
          <p>{{event.date}}</p>
        </div>
        <div class="event-flex">
          <div class="event-details">
            <p class="title">Description:</p>
            <p>{{event.description}}</p>
          </div>
          <div class="detail-divider"></div>
          <div class="event-role">
            <p class="title">Role:</p>
            <p>{{event.role}}</p>
          </div>
        </div>
        <button class="detail-button" v-on:click="go(event.id)">See Details</button>
      </div>
    </div>
    
  </div>
</template>

<script>
import auth from "../auth";
import APIService from "../APIService";
const apiService = new APIService();

export default {
  name: "HomeHost",
  components: {},
  data() {
    return {
      events: []
    };
  },
  methods: {
    async hostEventList(userName) {
      this.error = "";
      try {
        let userinfo = auth.getUserId();
        let eventsH = await apiService.hostEvents(userinfo, userName);
        this.events = eventsH;
      } catch (error) {
        this.error = error;
      }
    },
    go(id) {
      auth.saveEventId(id);
      this.$router.push("/eventhost");
    },

    createEvent() {
      this.$router.push("/create");
    }
  },
  created() {
    let user = auth.getUser();
    this.hostEventList(user.sub);
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
  flex-direction: row;
  flex-wrap: wrap;
 
  font-family: "Amaranth", sans-serif;
}
.event-title {
  width: 100%;
  margin: 10px 0;
  margin-top: unset;
  height: auto;
  text-align: center;
  display: flex;
  background: #e4836c;
  padding: 10px 0;
  margin-bottom: unset;
  border-radius: unset;
  cursor: pointer;
}
.event-title > .title {
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
.event-details > .title {
  font-size: 15px;
  font-weight: bold;
  padding-right: 10px;
  text-align: left;
  margin-top: unset;
  
}
.event-details > p {
  text-align: left;
}
.event-role {
  width: 20%;
  height: auto;
  text-align: center;
}
.event-role > .title {
  font-size: 15px;
  font-weight: bold;
  background: #fefefd;
  margin-top: unset;
}
.detail-divider {
  border-left: 1px solid #e7963d;
  border-right: 1px solid #e7963d;
  height: 80%;
  position: relative;
  right: 20px;
  top: 10px;
}
.detail-button {
  background-color: #e7963d;
  width: 150px;
  height: 40px;
  margin: 15px;
  margin-top: unset;
}
.button-div {
  width: 60%;
  margin: 25px auto;
}

.account-button {
  padding: 10px;
  font-size: 15px;
  font-weight: 400;
  border-radius: 8px;
  background-color: #e4836c;
  color: black;
  cursor: pointer;
  border: none;
  outline: none;
  width: 150px;
  bottom: 20px;
 
}
.manage-button {
  padding: 10px;
  font-size: 15px;
  font-weight: 400;
  border-radius: 8px;
  background-color: #e4836c;
  color: black;
  cursor: pointer;
  border: none;
  outline: none;
  width: 150px;
  bottom: 20px;

}
a {
 color:black;
 text-decoration:none;
}
@media only screen and (max-width: 800px) {
  .event-card{
    width: unset;
  }
  .button-div{
    margin-top: unset;
    margin-bottom: unset;
  }
}
</style>
