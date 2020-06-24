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
  page = 1;
  pageSize = 4;
  constructor(private service: OrderService) { }

  ngOnInit() {
    this.getOrders();
        
  }

  getOrders() {
      this.errorReceived = false;
      this.service.getOrdersByCustomer()
          .pipe(catchError((err) => this.handleError(err)))
          .subscribe(orders => {
            debugger
              this.orders = orders;
              console.log('orders items retrieved: ' + orders.length);
      });
  }

  get sliceOrders(): IOrder[] {
    return this.orders
      .map((country, i) => ({id: i + 1, ...country}))
      .slice((this.page - 1) * this.pageSize, (this.page - 1) * this.pageSize + this.pageSize);
  }
  private handleError(error: any) {
      this.errorReceived = true;
      return Observable.throw(error);
   
  }

  

}
