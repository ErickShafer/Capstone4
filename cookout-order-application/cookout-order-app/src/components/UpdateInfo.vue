<template>
  <form id="inputs">
      <br>
      <div>
        Event Time:
        <input type="datetime-local" name="Date" v-model="createForm.date" />
        <br />
        <br />
      </div>

      <div>
        Event Description:
        <input type="text" name="Description" :onblur="updateName()" v-model="createForm.description" />
        <br />
        <br />
      </div>
      <button @click.prevent="updateInfo()" id="submit">Update</button>
    
    <button v-on:click.prevent="hideForm">Go Back</button>
    </form>
</template>

<script>
import auth from "../auth";
import APIService from "../APIService";
const apiService = new APIService();
export default {
  name: "UpdateInfo",
  
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
    hideForm() {
      this.$emit("remove", "");
    },

    updateForm(){
      this.createForm.hostId = auth.getUserId();
    },

    updateName(){
      this.createForm.name = this.$parent.selectionName;
    },

    async updateInfo() {
      this.error = "";
      try {
        let token = await apiService.updateInfo(this.createForm);
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
  },

  created(){
    this.updateForm();
  }
};
</script>

<style>
</style>