import { createPinia } from 'pinia'
import { useProductStore } from './product'
import { useKeyword } from './keyword'

export const store = createPinia()

export function useStore() {
    return store
}

export const productStore = useProductStore(store)

export const keywordStore = useKeyword(store)