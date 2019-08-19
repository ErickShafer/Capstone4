<template>
  <div>
      <div v-for="(orderVM, index) in orderVMs" :class="getOrderClass(orderVM)">#{{index+1}} {{orderVM.userName}}</div>

  </div>
</template>

<script>
import auth from "../auth";
import APIService from "../APIService";
const apiService = new APIService();

export default {
  name: "Orders",

  data() {
    return {
      orderVMs: {},
      eventId: 0,
      userId: 0,
      count: 0,
      timer: {}
    };
  },

  methods: {
    async itemList() {
      this.error = "";
      try {
        let itemsL = await apiService.getOrderVM(this.eventId);
        this.orderVMs = itemsL;
      } catch (error) {
        this.error = error;
      }
      this.startHeartBeat();
    },
    countPlus() {
      this.count = this.count + 1;
    },
    startHeartBeat() {
      this.timer = setTimeout(this.itemList, 3000);
    },
    stopHeartBeat() {
      clearTimeout(this.timer);
    },
    getOrderClass(orderItem) {
      let result = "orderstatus";
      if (orderItem.userId == this.userId) {
        result = "isUser";
      }
      return result;
    }
  },

  created() {
    this.eventId = auth.getEventId();
    this.userId = auth.getUserId();
    this.itemList();
    this.startHeartBeat();
  }
};
</script>

<style scoped>

.Blurbheader{
  height: auto;
  background: #e4836c;
  margin-bottom: unset;
}

 .queue{
  background: #fff;
  margin: 50px auto;
  height:fit-content;
  margin-top: unset;
  margin-bottom: unset;
}
.orderstatus {
  font-size: 10px;
  font-weight: none;
}
.isUser {
  color: green;
  font-size: 1.5rem;
}
</style>
