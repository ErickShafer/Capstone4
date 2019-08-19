<template>
  <div>
    <div class="order-card" v-for="(orderVM, index) in orderVMs" :key="orderVM.orderid" v-show="orderVM.complete != true" @dblclick="markComplete(orderVM)">
      <h1 class="order-title">#{{index+1}} Name: {{orderVM.userName}} </h1>
      <ul class="order-details">
        <li v-for="orderItem in orderVM.items" :key="orderItem.itemID">{{orderItem.itemQty}} {{orderItem.itemName}}</li>
      </ul>
    </div>
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
      orderIds: [],
      items:[]
    };
  },

  methods: {
    async itemList(eventId) {
      this.error = "";
      try {
        let itemsL = await apiService.getOrderVM(this.eventId);
        this.orderVMs = itemsL;
      } catch (error) {
        this.error = error;
      }
       this.startHeartBeat();
    },
    async markComplete(vm) {
       try {
        if (vm.complete != true) {
          vm.complete = true;
          apiService.completeOrder(vm.orderId)
        }
       } 
       catch (error) {
        this.error = error;
      }
    },
    startHeartBeat() {
        this.timer = setTimeout(this.itemList, 3000);
    },
    stopHeartBeat() {
        clearTimeout(this.timer);
    }
  },
  created() {
    this.eventId = auth.getEventId();
    this.itemList();
    this.startHeartBeat();
  }
  
}
</script>

<style scoped>
.order-card {
  width: 60%;
  height: auto;
  margin: 25px auto;
  padding: 15px;
  border-radius: 5px;
  background: #fefefd;
  display: inline-flex;
  flex-direction: row;
  flex-wrap: wrap;
  font-family: "Amaranth", sans-serif;
}
.order-title {
  width: 100%;
  background: #e7963d;
  border-radius: 5px;
  margin: 10px 0;
  height: 35px;
  text-align: center;
  padding-top: 15px;
  padding-bottom: 15px;
  font-family: "Amaranth", sans-serif;
  font-weight: bold;
}
.order-details {
  border-radius: 5px;
  border-style: solid;
  border-color: #e7963d;
  background: #fefefd;
  width: 100%;
  height: auto;
  padding: 10px;
  margin: 10px;
  margin-left: unset;
  list-style: none;
  font-family: "Amaranth", sans-serif;
}
  @media only screen and (max-width: 800px) {
    .order-card{
      width: unset;
      /* margin-left: unset;
      margin-right: unset; */
    }
  
}
</style>
