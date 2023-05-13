import { createRouter, createWebHistory } from 'vue-router';

import NotFound from '@/views/NotFound.vue'
import NotAuth from '@/views/NotAuth.vue'
import TheHome from '@/views/home/TheHome.vue';
import TheLogin from '@/views/login/TheLogin.vue';
import TheRegister from '@/views/register/TheRegister.vue';
import ThePostDetail from '@/views/post/ThePostDetail.vue';
import ThePostPost from '@/views/post/ThePostPost.vue';
import ThePostEdit from '@/views/post/ThePostEdit.vue';
import TheAdmin from '@/views/admin/TheAdmin.vue'
import UserList from '@/views/admin/user/UserList'
import ManagePosts from '@/views/admin/post/ManagePosts'
import TheMessages from '@/views/messages/TheMessages'
import MessageDetail from '@/views/messages/MessageDetail'
import ChangePassword from '@/views/login/ChangePassword'
import ChangeUserInfo from '@/views/login/ChangeUserInfo'
import UserDetail from '@/views/login/UserDetail'
import MyPostList from '@/views/post/MyPostList'
import FavouritePosts from '@/views/post/FavouritePosts'



const routes = [
    { path: '/', component: TheHome },
    { path: '/trang-chu', component: TheHome },
    { path: '/quan-ly-tin-dang', component: MyPostList, meta: { requiresAuth: true }, },
    { path: '/tin-dang-yeu-thich', component: FavouritePosts, meta: { requiresAuth: true }, },
    { path: '/dang-nhap', component: TheLogin },
    { path: '/dang-ky', component: TheRegister },
    { path: '/doi-mat-khau', component: ChangePassword, meta: { requiresAuth: true }, },
    { path: '/thay-doi-thong-tin-ca-nhan', component: ChangeUserInfo, meta: { requiresAuth: true }, },
    { path: '/cai-dat-tai-khoan', component: UserDetail, meta: { requiresAuth: true }, },
    { path: '/dang-tin', component: ThePostPost, meta: { requiresAuth: true }, },
    {
        path: '/chinh-sua-tin-dang/:id',
        component: ThePostEdit,
        meta: { requiresAuth: true },
        props: true
    },
    { path: '/403', component: NotFound },
    { path: '/:pathMatch(.*)*', component: NotFound },
    {
        path: '/post/:id',
        name: 'the-post-detail',
        component: ThePostDetail,
        props: true
    },
    {
        path: '/admin',
        name: 'admin',
        component: TheAdmin,
        meta: { requiresAuth: true, requiresCensor: true },
        redirect: '/admin/quan-ly-tin-dang',
        children: [
            { path: 'quan-ly-nguoi-dung', component: UserList, meta: { requiresAdmin: true }, },
            { path: 'quan-ly-tin-dang', component: ManagePosts },
            { path: ':pathMatch(.*)*', component: NotFound },
            { path: 'khong-du-quyen', component: NotAuth },
        ]
    },
    {
        path: '/tin-nhan',
        component: TheMessages,
        meta: { requiresAuth: true },
        children: [
            {
                path: '/tin-nhan/:id',
                name: 'the-message',
                component: MessageDetail,
                props: true,
            },
        ]
    },

    // ...
];

const router = createRouter({
    history: createWebHistory(),
    routes
});


import { isLoggedIn, isCensor, isAdmin } from '../stores/utils.js';
router.beforeEach((to, from, next) => {
    const requiresAuth = to.matched.some(record => record.meta.requiresAuth)
    const requiresCensor = to.matched.some(record => record.meta.requiresCensor)
    const requiresAdmin = to.matched.some(record => record.meta.requiresAdmin)

    if (requiresAuth) {
        // Kiểm tra xem người dùng đã đăng nhập chưa
        if (!isLoggedIn()) {
            window.scrollTo(0, 0);
            next('/dang-nhap')
        } else {
            if (requiresCensor && !isCensor()) {
                window.scrollTo(0, 0);
                next('/admin/khong-du-quyen')
            } else {
                // Kiểm tra xem người dùng có quyền admin hay không
                if (requiresAdmin && !isAdmin()) {
                    window.scrollTo(0, 0);
                    next('/admin/khong-du-quyen') // chuyển hướng đến trang lỗi 403
                } else {

                    window.scrollTo(0, 0);
                    next();
                }

                window.scrollTo(0, 0);
                next();
            }
        }
    } else {

        window.scrollTo(0, 0);
        next();
    }
})
export default router