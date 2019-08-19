<template>
  <div id="Landing">
    <div class="hero-section">
      <div class="shadow-box">
        <h1>Cookout Catering</h1>
        <h3>Kickstart your cookout like a pro!</h3>
        </div>
    </div>
    <login-register @loggedIn="listenLoggedIn"></login-register>
  </div>
</template>

<script>
import LoginRegister from "../components/LoginRegister";
export default {
  name: "LandingPage",
  components: {
    LoginRegister
  },
  methods: {
    listenLoggedIn() {
      this.$emit('loggedIn')
    }
  }, 
}

</script>

<style scoped>
.hero-section {
  background: url("https://drive.google.com/uc?export=view&id=1Bu8oLcPPuqV-TbAraWSZ0eMDxU2PHlrS")
    50% no-repeat;
  background-size: cover;
  height: 40vh;
  text-align: center;
  display: flex;
  align-items: center;
  justify-content: center;
  margin-top: 5px;
}

.hero-section h1 {
  opacity: 1.0;
  font-weight: bold;
  color: #f8d19a;
  font-family: "Courgette", cursive;
}
.hero-section h3 {
  opacity: 1.0;
  font-weight: bold;
  color:#f8d19a;
  font-family: "Courgette", cursive;
}

.shadow-box{
  height: 25vh;
  width: 90%;
  background-color: rgba(29, 28, 28, 0.6);
  
}
</style>
