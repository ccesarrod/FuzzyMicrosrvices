import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Product } from "@models/product-models";
import { Observable } from "rxjs/Observable";
import { BehaviorSubject, Subject } from "rxjs";
import { ICartItem } from "@models/cartItem-model";
import * as _ from "lodash";
import { environment } from "../../environments/environment";

@Injectable()
export class CartService {
  private itemsInCartSubject: BehaviorSubject<
    ICartItem[]
  > = new BehaviorSubject([]);
  private cart: ICartItem[] = [];

  constructor(private httpclient: HttpClient) {
    this.itemsInCartSubject.subscribe(p => (this.cart = p));
  }

  addProduct(item: Product) {
    this.addNewItemToCart(item);

    this.itemsInCartSubject.next([...this.cart]);
  }

  removeItemFromCart(item: ICartItem) {
    _.remove(this.cart, e => {
      e.id === item.id;
    });
  }

  addNewItemToCart(item: Product) {
    var index = this.cart.findIndex(x => {
      return x.id === item.productID;
    });

    if (index === -1) {
      this.cart.push(this.buildNewCartItem(item));
    } else {
      this.cart[index].quantity++;
    }
  }

  buildNewCartItem(item: Product) {
    const cartItem: ICartItem = {
      name: item.productName,
      price: item.unitPrice,
      id: item.productID,
      quantity: 1
    };
    return cartItem;
  }

  getCart(): Subject<ICartItem[]> {
    if (this.itemsInCartSubject != null)
      this.itemsInCartSubject.next(this.cart);

    return this.itemsInCartSubject;
  }

  totalCount() {
    var count = this.cart
      .map(item => item.quantity)
      .reduce((prev, curr) => prev + curr, 0);
    return count;
  }

  totalPrice() {
    let total = this.cart
      .map(item => item.quantity * item.price)
      .reduce((prev, curr) => prev + curr, 0);

    return total;
  }

  addQuantity(item: ICartItem) {
    let result = this.cart.find(x => {
      return x.id === item.id;
    });

    result.quantity++;
  }

  removeQuantity(item: ICartItem) {
    let result = this.cart.find(x => {
      return x.id === item.id;
    });

    if (result.quantity <= 0) result.quantity = 0;
    else result.quantity--;

    if (result.quantity == 0) {
      this.cart = this.cart.filter(obj => obj.id !== item.id);
      this.itemsInCartSubject.next([...this.cart]);
    }
  }

  save() {    
     if (this.cart) {
      return this.httpclient.post<any>(`${environment.apiUrl}/cart`, this.cart);
      }
  }

  assignCart(cartList: any[]) {
    this.cart = cartList;
    this.itemsInCartSubject.next([...this.cart]);
  }

  clearCart() {
    this.cart = [];
    this.itemsInCartSubject.next([...this.cart]);   
  }

  emptyCart(){
    this.clearCart()
    return this.httpclient.delete(`${environment.apiUrl}/cart`);
  }
}
