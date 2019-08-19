<template>
  <div class="order-div">
      <h1>Place Your Order</h1>
    <div class="menu-box">
      <h2>Menu</h2>
      <ul class="menu-select">
      <div  v-for="(item,index) in eventVM.menu" v-bind:key="item">
          <li class="itemLi">
            <button class="plusminus" v-bind:id="index" v-on:click="qty" value="-">-</button>
            <button class="plusminus" v-bind:id="index" v-on:click="qty" value="+">+</button>
            {{item.name}} {{item.itemQty}}
            <br>
          </li>
      </div>
      </ul>
    </div>
    <button class="order-button" v-on:click="submitOrder">Place Order</button>
  </div>
</template>

<script>
import auth from "../auth";
import APIService from "../APIService";
const apiService = new APIService();
export default {
  name: "EventGuest",
  userId: "",
  start: 0,
  data() {
    return {
      orderItems: [],
      eventVM: {},
      count: []
    };
  },

  methods: {
    qty(e){
      if (e.target.value == "+"){
        this.eventVM.menu[e.target.id].itemQty =  this.eventVM.menu[e.target.id].itemQty+1;
      }
      else if (this.eventVM.menu[e.target.id].itemQty > 0) {
        this.eventVM.menu[e.target.id].itemQty =  this.eventVM.menu[e.target.id].itemQty-1;
      }
    },
    async itemList(eventId) {
      this.error = "";
      try {
        let itemsL = await apiService.guestEventVM(eventId);
        this.eventVM = itemsL;
        this.makecount(this.eventVM.menu.length)
      } catch (error) {
        this.error = error;
      }
    },
    async submitOrder() {
      this.eventVM.userId = auth.getUserId();
      apiService.submitOrder(this.eventVM);
      this.$router.push("/queue");


    },

    addToOrder(item) {
      this.orderItems.push(item);
    }
  },

  created() {
    let eventId = auth.getEventId();
    this.itemList(eventId);
    
  }
};
</script>

<style scoped>
li {
  list-style-type: none;
}

.itemLi{
  text-align: unset;
}
h1{
    width: 100%;
    font-family: "Courgette", cursive;
}
.order-div{
    display: flex;
    flex-wrap: wrap;
}
.menu-box{
    display: flex;
    width: 90%;
    background:blanchedalmond;
    margin: 10px;
    flex-wrap: wrap;
}
.menu-box h2 {
  width: 100%;
  margin-top: unset;
  height: 2rem;
  background-color: #E4836C;
}
.menu-box .menu-select {
    width: 100%;
    align-items: flex-start;
    margin-left: 10px;
}

ul{
  padding: unset;
  align-content: flex-start;
}
.your-order {

width: 47%;
background:blanchedalmond;
margin: 10px;
}
.your-order h2 {
background-color: #E4836C;
}
.order-button{
  
  align-self: flex-end;
}
.plusminus{
  width: 48px;
}
</style>