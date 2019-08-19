<template>
  <div id="formApp">
    <div id="pageContainerLogin" class="login" v-show="loginVisible">
      <div id="content-wrap">
        <h2>Login</h2>
        <div class="form-group">
          <div class="row">
            <div class="Absolute-Center is-Responsive">
              <div class="col-sm-12 col-md-10 col-md-offset-1">
                <form method="post">
                  <div class="form-group input-group">
                    <span class="input-group-addon">
                      <i class="glyphicon glyphicon-user"></i>
                    </span>
                    <input
                      class="form-control"
                      v-model="loginForm.email"
                      type="email"
                      placeholder="Email"
                    />
                  </div>
                  <div class="form-group input-group">
                    <span class="input-group-addon">
                      <i class="glyphicon glyphicon-lock"></i>
                    </span>
                    <input
                      class="form-control"
                      type="password"
                      v-model="loginForm.password"
                      placeholder="password"
                    />
                  </div>
                  <div class="form-group buttons">
                    <button type="submit" @click.prevent="login" class="btn btn-def btn-block">Submit</button>
                  </div>
                </form>
                <a class="toggleForm" @click.prevent="toggle"> Click Here to Register</a>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div id="pageContainerRegister" class="register" v-show="!loginVisible">
      <div id="content-wrap">
        <h2>Register</h2>
        <div class="form-group">
          <div class="row">
            <div class="Absolute-Center is-Responsive">
              <div class="col-sm-12 col-md-10 col-md-offset-1">
                <form method="post">
                  <div class="form-group input-group">
                    <span class="input-group-addon">
                      <i class="glyphicon glyphicon-envelope"></i>
                    </span>
                    <input
                      class="form-control"
                      v-model="registerForm.username"
                      placeholder="Username"
                    />
                  </div>
                  <div class="form-group input-group">
                    <span class="input-group-addon">
                      <i class="glyphicon glyphicon-envelope"></i>
                    </span>
                    <input
                      class="form-control"
                      v-model="registerForm.email"
                      type="email"
                      placeholder="Email"
                    />
                  </div>
                  <div class="form-group input-group">
                    <span class="input-group-addon">
                      <i class="glyphicon glyphicon-lock"></i>
                    </span>
                    <input
                      class="form-control"
                      type="password"
                      placeholder="Password"
                      v-model="registerForm.password"
                    />
                  </div>
                  <div class="form-group input-group">
                    <span class="input-group-addon">
                      <i class="glyphicon glyphicon-lock"></i>
                    </span>
                    <input
                      class="form-control"
                      type="password"
                      placeholder="Confirm Password"
                      v-model="registerForm.confirmPass"
                    />
                  </div>
                  <div class="form-group buttons">
                    <button
                      type="submit"
                      @click.prevent="register"
                      value="register"
                      class="btn btn-def btn-block"
                    >Submit</button>
                  </div>
                </form>
                <a class="toggleForm" @click.prevent="toggle"> Click to Return to Login</a>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import auth from "../auth";
import APIService from "../APIService";
const apiService = new APIService();

export default {
  name: "LoginRegister",
  data() {
    return {
      loginVisible: true,
      loginForm: {
        email: "",
        password: ""
      },
      registerForm: {
        username: "",
        email: "",
        password: "",
        confirmPass: ""
      }
    };
  },

  methods: {
    toggle() {
      this.loginVisible = !this.loginVisible;
    },
    async login() {
      this.error = "";
      try {
        let token = await apiService.login(this.loginForm);
        const user = auth.getUserWithToken(token);
        let userInfo = await apiService.getHostId(user.sub);
        auth.saveToken(token, userInfo);
        this.goHome(user);
      } catch (error) {
        this.error = error;
      }
    },
    async register() {
      this.error = "";
      try {
        let token = await apiService.register(this.registerForm);
        const user = auth.getUserWithToken(token);
        let userInfo = await apiService.getHostId(user.sub);
        auth.saveToken(token, userInfo);
        this.goHome(user);
      } catch (error) {
        this.error = error;
      }
    },

    goHome(data) {
      this.$emit("loggedIn");
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
  }

}
</script>

<style scoped>
.toggleForm:hover {
  cursor: pointer;
}
button {
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

</style>