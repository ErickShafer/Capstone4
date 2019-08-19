import Vue from 'vue';
import Router from 'vue-router';
import HomeBase from './views/HomeBase.vue';
import HomeHost from './views/HomeHost.vue';
import LandingPage from './views/LandingPage.vue';
import Orders from './views/Orders.vue';

import EventHost from './views/EventHost.vue';
import EventGuest from './views/EventGuest';
import CreateEvent from './views/CreateEvent.vue';
import MenuSelect from './views/MenuSelect.vue';
import Invite from './views/Invite.vue';
import Manage from './views/Manage.vue';
import auth from './auth';
import Queue from './views/Queue.vue';
import About from './views/About.vue';
import ManageEvent from './views/ManageEvent.vue';
Vue.use(Router)



const router = new Router({
  mode: 'history',
  base: process.env.BASE_URL,
  routes: [
    {
      path: '/create',
      name: 'create',
      component: CreateEvent,
      meta: {
        requiresAuth: true
      }
    },
    {
      path: '/manage',
      name: 'manage',
      component: Manage,
      meta: {
        requiresAuth: true
      }
    },
    {
      path: '/',
      name: 'landing',
      component: LandingPage,
      meta: {
        requiresAuth: false
      }
    },
    {
      path: '/invite',
      name: 'invite',
      component: Invite,
      meta: {
        requiresAuth: true
      }
    },
    {
      path: '/homebase',
      name: 'homebase',
      component: HomeBase,
      meta: {
         requiresAuth: true
      }
    },
    {
      path: '/homehost',
      name: 'homehost',
      component: HomeHost,
      meta: {
      requiresAuth: true
      }
    },
    {
      path: '/event',
      name: 'event',
      component: Event,
      meta: {
      requiresAuth: true
      }
    },
    {
      path: '/eventhost',
      name: 'eventhost',
      component: EventHost,
      meta: {
      requiresAuth: true
      }
    },
    {
      path: '/eventguest',
      name: 'eventguest',
      component: EventGuest,
      meta: {
         requiresAuth: true
      },
    },
    {
      path: '/orders',
      name: 'orders',
      component: Orders,
      meta: {
      requiresAuth: true
      },
    },
    {
      path: '/menu',
      name: 'menu',
      component: MenuSelect,
      meta: {
      requiresAuth: true
      },
    },
      {
        path: '/queue',
        name: 'queue',
        component: Queue,
        meta: {
        requiresAuth: true
        },
    },
    {
      path: '/about',
      name: 'about',
      component: About,
      meta: {
      requiresAuth: false
      },
  },
  {
    path: '/manageevent',
    name: 'manageevent',
    component: ManageEvent,
    meta: {
    requiresAuth: true
    },
},
    
  ]
})

router.beforeEach((to, from, next) => {
  // Determine if the route requires Authentication
  const requiresAuth = to.matched.some(x => x.meta.requiresAuth);
  const user = auth.getUser();

  if (requiresAuth && !user) {
    next("/");
  } else {
    // Else let them go to their next destination
    next();
  }
});
export default router
