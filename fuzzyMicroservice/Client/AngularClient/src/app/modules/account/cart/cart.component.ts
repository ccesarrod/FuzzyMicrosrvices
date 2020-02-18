import { Component, OnInit } from '@angular/core';
import { CartService } from '@services/cart.service';
import { ICartItem } from '@models/cartItem-model';
import { AccountService } from '@services/account.service';
import { AppToastService } from '@services/app-toast.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.scss']
})
export class CartComponent implements OnInit {

  public cart:any;
  constructor(private cartService: CartService,
    private authService: AccountService,
    private toastService: AppToastService,
    public router:Router) { }

 

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
      this.toastService.show('Cart updated',{ classname: 'bg-success text-light', delay: 3000 })
      

    });
  }

  get totalCount() {

    return this.cartService.totalCount();    
  }


  get totalPrice() {
    return this.cartService.totalPrice();
  }

  

}
