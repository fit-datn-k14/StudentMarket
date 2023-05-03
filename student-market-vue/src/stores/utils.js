// Import các hàm từ file utils.js
import { getUserFromLocalStorage } from './localStorage.js';

const isLoggedIn = () => {
    // Kiểm tra xem người dùng đã lưu thông tin tài khoản trong Local Storage hay chưa
    const user = getUserFromLocalStorage();
    // Trả về true nếu người dùng đã đăng nhập và có thông tin tài khoản trong Local Storage
    return user != null;
};


const isCensor = () => {
    // Lấy thông tin tài khoản người dùng từ Local Storage
    const user = getUserFromLocalStorage();
    // Trả về true nếu người dùng đã đăng nhập và có quyền admin
    return user !== null && user.Role > 0;
};
const isAdmin = () => {
    // Lấy thông tin tài khoản người dùng từ Local Storage
    const user = getUserFromLocalStorage();
    // Trả về true nếu người dùng đã đăng nhập và có quyền admin
    return user !== null && user.Role === 2;
};

export { isLoggedIn, isCensor, isAdmin }