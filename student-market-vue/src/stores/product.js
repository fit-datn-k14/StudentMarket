import { defineStore } from 'pinia'
import { getProduct } from '../api/product'

export const useProductStore = defineStore('product', {
    state: () => ({
        product: null,
    }),
    actions: {
        async fetchProduct(productId) {
            const product = await getProduct(productId)
            this.product = product
        },
    },
    getters: {
        getProduct() {
            return this.product
        },
    },
})

