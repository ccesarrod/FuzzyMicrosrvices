import { Component, OnInit } from '@angular/core';
import { CartService } from '@services/cart.service';
import { ICartItem } from '@models/cartItem-model';
import { AccountService } from '@services/account.service';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.scss']
})
export class CartComponent implements OnInit {

  public cart:any;
  constructor(private cartService: CartService,
    private authService: AccountService) { }

 

  ngOnInit() {
    
    this.cartService.getCart().subscribe(data=> {
      this.cart = data;
    });
  }

  addQuantity(item: ICartItem) {
    this.cartService.addQuantity(item);
  }

  removeItem(item: ICartItem) {
    this.cartService.removeQuantity(item);
  }

  saveCart() {
    this.cartService.save().subscribe(x => {

      this.authService.currentUserValue.cart = this.cart;

    });
  }

  get totalCount() {

    return this.cartService.totalCount();    
  }


  get totalPrice() {
    return this.cartService.totalPrice();
  }

  

}
