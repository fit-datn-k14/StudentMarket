import { defineStore } from 'pinia'

export const useKeyword = defineStore('keywordId', {
    state: () => ({
        keyword: "",
    }),
    actions: {
        setKeyword(keyword) {
            this.keyword = keyword;
        },
    },
    getters: {
        getKeyword() {
            return this.keyword
        },
    },
})
