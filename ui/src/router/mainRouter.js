import { createRouter, createWebHistory } from "vue-router";
import LoginForm from '@/views/Pages/LoginForm.vue';
import RegisterForm from '@/views/Pages/RegisterForm.vue';
import MainPage from '@/views/Pages/MainPage.vue';
import HomePage from '@/views/Pages/HomePage.vue';
import FilterPage from '@/views/Pages/FilterPage.vue';
import ManagerPage from '@/views/Pages/ManagerPage.vue';
import ProductList from '@/views/ManageProduct/ProductList.vue';
import AccountList from '@/views/ManageAccount/AccountList.vue';
import OrderList from '@/views/ManageOrder/OrderList.vue';
import ImportProductList from '@/views/ManageImport/ImportProductList.vue';
import GiftList from '@/views/ManageGift/GiftList.vue';
import PromotionList from '@/views/ManagePromotion/PromotionList.vue';
import ProductDetailPage from '@/views/Pages/ProductDetailPage.vue';
import CartPage from '@/views/Pages/CartPage.vue';
import InfoAccount from '@/views/ManageInformation/InfoAccount.vue';
import ChangePassword from '@/views/ManageInformation/ChangePassword.vue';
import StatisticPage from '@/views/ReportAndStatistic/StatisticPage.vue';
import RateList from '@/views/ManageRate/RateList.vue';
import HistoryShopping from '@/views/Pages/HistoryShopping.vue';
import CommonFn from '@/utils/commonFuncion';
import store from "@/utils/vuex";
import axios from "axios";

//Khai báo router
const routes = [
  {
    path: "/",
    name: 'MainPage',
    component: MainPage,
    children: [
      {
        path: '/',
        name: 'HomePage',
        component: HomePage,
        meta: {
          isManager: false,
          loginRequired: false
        }
      },
      {
        path: '/cart',
        name: 'CartPage',
        component: CartPage,
        meta: {
          isManager: false,
          loginRequired: false
        }
      },
      {
        path: '/productDetail',
        name: 'ProductDetailPage',
        component: ProductDetailPage,
        meta: {
          isManager: false,
          loginRequired: false
        }
      },
      {
        path: 'filter',
        name: 'FilterPage',
        component: FilterPage,
        meta: {
          isManager: false,
          loginRequired: false
        }
      },
      {
        path: 'manager',
        name: 'ManagerPage',
        component: ManagerPage,
        meta: {
          isManager: true,
          loginRequired: true
        },
        children: [
          {
            path: '/info-user',
            name: 'InfoUser',
            component: InfoAccount,
            meta: {
              isManager: true,
              loginRequired: true
            }
          },
          {
            path: '/change-password',
            name: 'ChangePassword',
            component: ChangePassword,
            meta: {
              isManager: true,
              loginRequired: true
            }
          },
          {
            path: '/history-shopping',
            name: 'HistoryShopping',
            component: HistoryShopping,
            meta: {
              isManager: true,
              loginRequired: true
            }
          },
          {
            path: '/product',
            name: 'ProductList',
            component: ProductList,
            meta: {
              isManager: true,
              loginRequired: true
            }
          },
          {
            path: '/account',
            name: 'AccountList',
            component: AccountList,
            meta: {
              isManager: true,
              loginRequired: true
            }
          },
          {
            path: '/order',
            name: 'OrderList',
            component: OrderList,
            meta: {
              isManager: true,
              loginRequired: true
            }
          },
          {
            path: '/import',
            name: 'ImportProductList',
            component: ImportProductList,
            meta: {
              isManager: true,
              loginRequired: true
            }
          },
          {
            path: '/gift',
            name: 'GiftList',
            component: GiftList,
            meta: {
              isManager: true,
              loginRequired: true
            }
          },
          {
            path: '/promotion',
            name: 'PromotionList',
            component: PromotionList,
            meta: {
              isManager: true,
              loginRequired: true
            }
          },
          {
            path: '/replyRate',
            name: 'RateList',
            component: RateList,
            meta: {
              isManager: true,
              loginRequired: true
            }
          },
          {
            path: '/statistic',
            name: 'StatisticPage',
            component: StatisticPage,
            meta: {
              isManager: true,
              loginRequired: true
            }
          },

        ]
      }
    ],
    meta: {
      isManager: false,
      loginRequired: false
    }
  },
  {
    path: "/login",
    name: 'LoginForm',
    component: LoginForm,
    meta: {
      isManager: false,
      loginRequired: false
    }
  },
  {
    path: "/register",
    name: 'RegisterForm',
    component: RegisterForm,
    meta: {
      isManager: false,
      loginRequired: false
    }
  },
];
// Khởi tạo router
const router = createRouter({
  history: createWebHistory(),
  routes: routes,
});

router.beforeEach(async (to, from, next) => {
  debugger
  await axios.get('Account/checkLogin').
    then((res) => {
      store.dispatch('setUser', res.data);
      // return {name: to.name}
      next();

    }).
    catch(async () => {
      var tokenMoel = {
        refreshToken: CommonFn.getCookie('RefreshToken'),
        accessToken: CommonFn.getCookie('Token')
      };
       await axios.post('Account/renewToken', tokenMoel).
        then((response) => {
          CommonFn.setCookie('RefreshToken', response.data.refreshToken, 60);
          CommonFn.setCookie('Token', response.data.accessToken, 60);
          store.dispatch('setUser', response.data.infoUser);
          axios.defaults.headers.common["Authorization"] = "Bearer " + response.data.accessToken;
          // return {name: to.name}
          next();
        }).
        catch(() => {
          store.dispatch('setUser', null);
          if (to.meta.loginRequired) {
            next({name: 'LoginForm'});
            // return {path: "/login"}
          }
          else {
            // return {name: to.name}
            next();
          }
          
        });

    });
    // next(); 
});

export default router;