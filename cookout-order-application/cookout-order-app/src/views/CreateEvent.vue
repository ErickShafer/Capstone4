<template>
  <div id="create form">
    <h2>Create an Event</h2>
    <form id="inputs">
      <div>
        Event Name:
        <input type="text" name="Event Name" v-model="createForm.name" />
        <br />
        <br />
      </div>

      <div>
        Event Time:
        <input type="datetime-local" name="Date" v-model="createForm.date" />
        <br />
        <br />
      </div>

      <div>
        Event Description:
        <input type="text" name="Description" v-model="createForm.description" />
        <br />
        <br />
      </div>
      <button v-on:click.prevent="create" id="submit">Create</button>
    </form>
  </div>
</template>

<script>
import auth from "../auth";
import APIService from "../APIService";
const apiService = new APIService();

export default {
  beforeMount() {
    this.findId();
  },
  name: "CreateEvent",
  data() {
    return {
      createForm: {
        name: "",
        description: "",
        date: "",
        hostId: 0
      }
    };
  },
  methods: {
    async create() {
      this.error = "";
      try {
        let token = await apiService.createEvent(this.createForm);
        this.$router.push({
          path: "/homehost"});
      } catch (error) {
        this.error = error;
      }
      
    },
    findId() {
      let user = auth.getUser();
      fetch("https://localhost:44397/api/login/findId?userName="+user.sub,{method: "GET"})
      .then(Response => Response.json())
      .then(json =>{this.createForm.hostId = json;});
    }
  }
};
</script>

<style scoped>
#submit {
  align-self: center;
}

#inputs {
  display: flex;
  flex-direction: column;
}
</style>
