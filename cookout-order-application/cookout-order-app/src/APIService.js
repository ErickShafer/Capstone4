import axios from 'axios';

export default class APIService {
    constructor() {
    }
    async login(data) {
        const url = `${process.env.VUE_APP_REMOTE_API}/login/login`;
        let res = await axios.post(url, data);
        if (res.status === 401) {
            throw "Your username and/or password is invalid";
        }
        return res.data;
    }

    async register(data) {
        const url = `${process.env.VUE_APP_REMOTE_API}/login/register`;
        let res = await axios.post(url, data);
        if (res.status === 401) {
            throw "Your username and/or password is invalid";
        }
        return res.data;
    }

    async guestEvents(data) {
        const url = `${process.env.VUE_APP_REMOTE_API}/login/home?username=${data}`;
        let res = await axios.get(url, data);
        if (res.status === 401) {
            throw "There are currently no events";
        }
        return res.data;
    }
    async createEvent(data) {
        const url = `${process.env.VUE_APP_REMOTE_API}/login/create`;
        let res = await axios.post(url, data);
        if (res.status === 401) {
            throw "Your username and/or password is invalid";
        }
        return res.data;
    }

    async hostEvents(id, name) {
        const url = `${process.env.VUE_APP_REMOTE_API}/login/hostevents?id=${id}&name=${name}`;
        let res = await axios.get(url, id, name);
        if (res.status === 401) {
            throw "There are currently no events";
        }
        return res.data;
    }

    async userInfo(data) {
        const url = `${process.env.VUE_APP_REMOTE_API}/login/userinfo?name=${data}`;
        let res = await axios.get(url, data);
        if (res.status === 401) {
            throw "There are currently no events";
        }
        return res.data;
    }

    async items() {
        const url = `${process.env.VUE_APP_REMOTE_API}/login/items`;
        let res = await axios.get(url);
        if (res.status === 401) {
            throw "There are currently no items";
        }
        return res.data;
    }

    async upgradeUser(userId) {
        const url = `${process.env.VUE_APP_REMOTE_API}/login/updateRole?userId=${userId}&newRoleId=2`;
        let res = await axios.put(url, userId);
        if (res.status === 401) {
            throw "There are currently no events";
        }
    }

    async getHostId(data) {
        const url = `${process.env.VUE_APP_REMOTE_API}/login/findId?userName=${data}`;
        let res = await axios.get(url, data);
        if (res.status === 401) {
           throw "There are currently no items";
        }
        return res.data;
    }
    
    async guests() {
        const url = `${process.env.VUE_APP_REMOTE_API}/login/guests`;
        let res = await axios.get(url);
        if (res.status === 401) {
            throw "There are currently no guests";
        }
        return res.data;
    }

    async invite(data) {
        const url = `${process.env.VUE_APP_REMOTE_API}/login/invite`;
        let res = await axios.post(url, data);
        if (res.status === 401) {
            throw "There are currently no guests";
        }
        return res.data;
    }

    async events(data) {
        const url = `${process.env.VUE_APP_REMOTE_API}/login/home?userName=${data}`;
        let res = await axios.get(url, data);
        if (res.status === 401) {
           throw "There are currently no events";
        }
        return res.data;
    }

    async myevents(data) {
        const url = `${process.env.VUE_APP_REMOTE_API}/login/myEvents?id=${data}`;
        let res = await axios.get(url, data);
        if (res.status === 401) {
           throw "There are currently no events";
        }
        return res.data;
    }

    async menu(data) {
        const url = `${process.env.VUE_APP_REMOTE_API}/login/menu`;
        let res = await axios.post(url, data);
        if (res.status === 401) {
            throw "There are currently no items";
        }
        return res.data;
    }
    async guestEventVM(data) {
        const url = `${process.env.VUE_APP_REMOTE_API}/login/getAEvent?id=${data}`;
        let res = await axios.get(url, data);
        if (res.status === 401) {
            throw "There are currently no items";
        }
        return res.data;
    }

    async submitOrder(data) {
        const url = `${process.env.VUE_APP_REMOTE_API}/login/postOrder`;
        let res = await axios.post(url, data);
        if (res.status === 401) {
            throw "Your username and/or password is invalid";
        }
        return res.data;
    }

    async eventGuestListVM(data) {
        const url = `${process.env.VUE_APP_REMOTE_API}/login/getGuests?eventId=${data}`;
        let res = await axios.get(url, data);
        if (res.status === 401) {
            throw "There are currently no items";
        }
        return res.data;
    }
    async getOrderVM(data) {
        const url = `${process.env.VUE_APP_REMOTE_API}/login/getOrders?eventID=${data}`;
        let res = await axios.get(url, data);
        if (res.status === 401) {
            throw "There are currently no items";
        }
        return res.data;
    }

    async getOrderIDs(data) {
        const url = `${process.env.VUE_APP_REMOTE_API}/login/getOrderids?eventID=${data}`;
        let res = await axios.get(url, data);
        if (res.status === 401) {
            throw "There are currently no items";
        }
        return res.data;
    }

   async updatemenu(data) {
        const url = `${process.env.VUE_APP_REMOTE_API}/login/updateMenu?invite=${data}`;
        let res = await axios.post(url, data);
        if (res.status === 401) {
            throw "Your username and/or password is invalid";
        }
        return res.data;
    }

    async getitems(data) {
        const url = `${process.env.VUE_APP_REMOTE_API}/login/getOrderItems=${data}`;
        let res = await axios.get(url, data);
        if (res.status === 401) {
            throw "There are currently no items";
        }
        return res.data;
    }

    async updateguests(data) {
        const url = `${process.env.VUE_APP_REMOTE_API}/login/updateGuests?invite=${data}`;
        let res = await axios.post(url, data);
        if (res.status === 401) {
            throw "Your username and/or password is invalid";
        }
        return res.data;
    }

    async completeOrder(orderId) {
        const url = `${process.env.VUE_APP_REMOTE_API}/login/markComplete?orderId=${orderId}`;
        let res = await axios.put(url, orderId);
        if (res.status === 401) {
            throw "There are currently no events";
        }
    }
    
    async updateInfo(data) {
        const url = `${process.env.VUE_APP_REMOTE_API}/login/updateInfo?uEvent=${data}`;
        let res = await axios.post(url, data);
        if (res.status === 401) {
            throw "Your username and/or password is invalid";
        }
        return res.data;
    }

    async findName(data) {
        const url = `${process.env.VUE_APP_REMOTE_API}/login/findName?id=${data}`;
        let res = await axios.get(url, data);
        if (res.status === 401) {
            throw "There are currently no items";
        }
        return res.data;
    }

}




