import { createApp } from 'vue'
import App from './App.vue'
import router from '@/Router'
import { createPinia } from 'pinia'
import vClickOutside from "click-outside-vue3"
import 'bootstrap/dist/css/bootstrap.css'
import 'bootstrap/dist/js/bootstrap.js'
import jQuery from 'jquery'
global.jQuery = jQuery
var $ = jQuery
window.$ = $
import HCommon from "./js/common.js"
import HEnum from "./js/enum"
import 'popper.js'
import HButton from './components/button/HButton.vue'
import HCheckbox from './components/checkbox/HCheckbox.vue'
import HInput from './components/input/HInput.vue'
import HIcon from './components/icon/HIcon.vue'
import HCombobox from './components/combobox/HCombobox.vue'
import HTable from './components/table/HTable.vue'
import HGridTree from './components/table/HGridTree.vue'
import HPopup from './components/popup/HPopup.vue'
import HDialog from './components/dialog/HDialog.vue'
import HDatePicker from './components/datepicker/HDatePicker.vue'
import HLoading from "@/components/loading/HLoading.vue"
import Paginate from "vuejs-paginate-next";
import PostList from "@/components/postlist/HPostList.vue";
import HToast from "@/components/toast/HToast.vue"
import axios from 'axios';
import { keywordStore } from '@/stores/keyword'
const store = createPinia()


const app = createApp(App)

app.use(createPinia());

app.use(store)
app.provide('keywordStore', keywordStore)

app.use(router)
app.use(vClickOutside)
app.config.globalProperties.HCommon = HCommon;
app.config.globalProperties.HEnum = HEnum;
app.config.globalProperties.axios = axios;

app.component('HButton', HButton)
app.component('HToast', HToast)
app.component('HCheckbox', HCheckbox)
app.component('HCombobox', HCombobox)
app.component('HDatePicker', HDatePicker)
app.component('HDialog', HDialog)
app.component('HInput', HInput)
app.component('HIcon', HIcon)
app.component('HLoading', HLoading)
app.component('HPaging', Paginate)
app.component('HPostList', PostList)
app.component('HPopup', HPopup)
app.component('HTable', HTable)
app.component('HGridTree', HGridTree)

app.mount('#app')
