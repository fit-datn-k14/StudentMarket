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

// Lưu danh sách id tin đăng yêu thích vào localStorage
const saveFavouritePostsToLocalStorage = (postIds) => {
    window.localStorage.setItem('FavouritePosts', JSON.stringify(postIds));
};

// Lấy danh sách id tin đăng yêu thích từ localStorage
const getFavouritePostsFromLocalStorage = () => {
    const FavouritePosts = window.localStorage.getItem('FavouritePosts');
    return FavouritePosts ? JSON.parse(FavouritePosts) : [];
};

// Thêm id của một tin đăng yêu thích vào danh sách trong localStorage
const addFavouritePostIdToLocalStorage = (postId) => {
    const FavouritePosts = getFavouritePostsFromLocalStorage();
    FavouritePosts.push(postId);
    saveFavouritePostsToLocalStorage(FavouritePosts);
};

// Xóa id của một tin đăng yêu thích khỏi danh sách trong localStorage
const removeFavouritePostIdFromLocalStorage = (postId) => {
    const FavouritePosts = getFavouritePostsFromLocalStorage();
    const index = FavouritePosts.indexOf(postId);
    if (index > -1) {
        FavouritePosts.splice(index, 1);
    }
    saveFavouritePostsToLocalStorage(FavouritePosts);
};

const removeAllFavouritesFromLocalStorage = () => {
    saveFavouritePostsToLocalStorage([]);
};

export {
    saveUserToLocalStorage, getUserFromLocalStorage, removeUserFromLocalStorage,
    saveFavouritePostsToLocalStorage, getFavouritePostsFromLocalStorage, addFavouritePostIdToLocalStorage,
    removeFavouritePostIdFromLocalStorage, removeAllFavouritesFromLocalStorage
};
