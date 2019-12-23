export interface Product {
    productName: string;
    productID: number;
    categoryId?: number;
    quantityPerUnit: string;
    unitPrice?: number;
    unitsInStock?: number;
    unitsOnOrder?: number;
    reorderLevel?: number;
    discontinued: boolean;
    }