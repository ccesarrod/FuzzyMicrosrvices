import { Component, OnInit } from '@angular/core';
import { CartService } from '@services/cart.service';
import { ICartItem } from '@models/cartItem-model';
import { IOrder } from '@models/order';
import { OrderService } from '@services/order.service';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.scss']
})
export class OrderComponent implements OnInit {
 
  private interval = null;
  errorReceived: boolean;

  orders: IOrder[];

  constructor(private service: OrderService) { }

  ngOnInit() {
    this.getOrders();
        
  }

  getOrders() {
      this.errorReceived = false;
      this.service.getOrdersByCustomer()
          .pipe(catchError((err) => this.handleError(err)))
          .subscribe(orders => {
              this.orders = orders;
              console.log('orders items retrieved: ' + orders.length);
      });
  }

  private handleError(error: any) {
      this.errorReceived = true;
      return Observable.throw(error);
   
  }

  

}
