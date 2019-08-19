<template>
  <div id="app">
    <header>
      <ul class="nav">
        <button class="collapsible"><img class="logo-button" id="logo" src="./assets/images/cookoutLogo.png" /></button>
        <div class="links content">
          <li id="home-li" @click="goHome()">Home</li>

          <li>|</li>
          <li>
            <router-link to="/about">About</router-link>
          </li>
          <li v-show="logoutVisibile">|</li>
          <li>
            <a href="/" v-on:click="logout" v-show="logoutVisibile">Logout</a>
          </li>
        </div>
      </ul>
    </header>

    <router-view @loggedIn="checkLogin" />
  </div>
</template>

<script>
import auth from "./auth";

export default {
  methods: {
    logout() {
      let _token = auth.getToken();
      auth.destroyToken();
      this.logoutVisible = false;
    },

    checkLogin() {
      let i = auth.getUserId();
      if (i != null) {
        this.logoutVisibile = true;
      }
    },

    goHome() {
      this.$emit("loggedIn");
      let data = auth.getUser();
      if (data.rol === "2") {
        this.$router.push({
          path: "/homehost"
        });
      } else if (data.rol === "1") {
        this.$router.push({
          path: "/homebase"
        });
      }
    }
  },

  data() {
    return {
      logoutVisibile: false
    };
  },

  created() {
    this.checkLogin();
  }
};
</script>

<style>
@import url("https://fonts.googleapis.com/css?family=Amaranth|Courgette&display=swap");

.collapsible{
  width: unset;
  background-color: transparent;
  overflow: unset;
  box-shadow: unset;
  margin: unset;
  padding: unset;
}

#app {
  font-family: "Avenir", "Amaranth", sans-serif, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  justify-content: center;
}
body {
  background: #f8d19a;
  color: #661308;
}
.h2 {
  font-family: "Courgette", cursive;
  color: blanchedalmond;
}
button {
  box-shadow: -1px 1px 3px 0px black;
  overflow: hidden;
  padding: 5px;
  margin: 10px;
  font-size: 15px;
  font-weight: 400;
  border-radius: 8px;
  background-color: #e7963d;
  color: black;
  cursor: pointer;
  border: none;
  outline: none;
  width: 120px;
  font-family: "Amaranth", sans-serif;
}
header img {
  height: 40px;
  vertical-align: middle;
  padding-right: 20px;
}

.nav {
  padding: 16px;
  margin: 0;
  flex: 1;
  list-style-type: none;
  display: flex;
  justify-content: center
}

.nav li {
  display: inline;
  padding-right: 20px;
  color: black;
}
.nav a {
  color: black;
}

header {
  height: 70px;
  width: 100%;
  display: flex;
  flex-direction: row;
  background-color: #e7963d;
  font-family: Arial, Helvetica, sans-serif;
}

.nav li {
  cursor: pointer;
}
#home-li {
  text-decoration: underline;
}

.links{
 
  margin-top: 1.5%;
}

@media only screen and (max-width: 600px) {
  
  .links{
    margin: 1.5%;
  }

  .details-div {
    width: unset;
    margin-left: unset;
    margin-right: unset;
  }
  .details-div p {
    height: unset;
  }
  .geust-div {
    width: unset;
  }
  .menu-div {
    width: unset;
  }
  .que-div {
    height: unset;
  }
  .manage-div {
    height: unset;
  }
  .event-card {
    width: unset;
  }

  .nav{
    height: auto;
    display: flex;
    flex-direction: column;
  }

}
</style>
