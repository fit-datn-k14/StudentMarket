// Lưu thông tin tài khoản người dùng vào localStorage
const saveUserToLocalStorage = (user) => {
    window.localStorage.setItem('user', JSON.stringify(user));
};

// Lấy thông tin tài khoản người dùng từ localStorage
const getUserFromLocalStorage = () => {
    const user = window.localStorage.getItem('user');
    return user ? JSON.parse(user) : null;
};

// Xóa thông tin tài khoản người dùng khỏi localStorage
const removeUserFromLocalStorage = () => {
    window.localStorage.removeItem('user');
};

export { saveUserToLocalStorage, getUserFromLocalStorage, removeUserFromLocalStorage };
