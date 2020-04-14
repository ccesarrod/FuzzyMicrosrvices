import { Component, OnInit } from '@angular/core';
import { CartService } from '@services/cart.service';
import { ICartItem } from '@models/cartItem-model';

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.sass']
})
export class OrderComponent implements OnInit {
cart:ICartItem[];

  constructor(private cartService: CartService,) { }

  ngOnInit() {
    this.cartService.getCart().subscribe(data=> {
      this.cart = data;
    });
  }

  get totalCount() {

    return this.cartService.totalCount();    
  }

  get totalPrice() {
    return this.cartService.totalPrice();
  }

}
