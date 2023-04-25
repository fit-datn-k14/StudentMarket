import { createRouter, createWebHistory } from 'vue-router';

import NotFound from '@/views/NotFound.vue'
import TheHome from '@/views/home/TheHome.vue';
import TheLogin from '@/views/login/TheLogin.vue';
import TheRegister from '@/views/register/TheRegister.vue';
import ThePostDetail from '@/views/post/ThePostDetail.vue';

const routes = [
    { path: '/', component: TheHome },
    { path: '/trang-chu', component: TheHome },
    { path: '/dang-nhap', component: TheLogin },
    { path: '/dang-ky', component: TheRegister },
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
        component: ThePostDetail,
        meta: { requiresAuth: true, requiresAdmin: true },
        children: [
            { path: 'aca', component: NotFound },
            { path: 'abc', component: TheLogin }
        ]
    },
    // ...
];

const router = createRouter({
    history: createWebHistory(),
    routes
});


import { isLoggedIn, isAdmin } from '../stores/utils.js';
router.beforeEach((to, from, next) => {
    const requiresAuth = to.matched.some(record => record.meta.requiresAuth)
    const requiresAdmin = to.matched.some(record => record.meta.requiresAdmin)

    if (requiresAuth) {
        // Kiểm tra xem người dùng đã đăng nhập chưa
        if (!isLoggedIn()) {
            next('/dang-nhap')
        } else {
            // Kiểm tra xem người dùng có quyền admin hay không
            if (requiresAdmin && !isAdmin()) {
                next('/403') // chuyển hướng đến trang lỗi 403
            } else {
                next()
            }
        }
    } else {
        next()
    }
})
export default router