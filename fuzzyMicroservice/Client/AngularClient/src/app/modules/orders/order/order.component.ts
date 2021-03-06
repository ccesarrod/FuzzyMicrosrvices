import { Component, OnInit, ViewChildren, QueryList } from '@angular/core';
import { CartService } from '@services/cart.service';
import { ICartItem } from '@models/cartItem-model';
import { IOrder } from '@models/order';
import { OrderService } from '@services/order.service';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { SortableHeadesDirective, SortEvent, SortDirection } from '@shared/sortable-heades.directive';
import * as moment from 'moment'

const rotate: {[key: string]: SortDirection} = { 'asc': 'desc', 'desc': '', '': 'asc' };
const compare = (v1: string, v2: string) => v1 < v2 ? -1 : v1 > v2 ? 1 : 0;


@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.scss']
})
export class OrderComponent implements OnInit {
 
  private interval = null;
  errorReceived: boolean;

  orders: IOrder[]=[];
  page = 1;
  pageSize = 4;
  @ViewChildren(SortableHeadesDirective) headers: QueryList<SortableHeadesDirective>;
  
  constructor(private service: OrderService) { }

  ngOnInit() {
    this.getOrders();
        
  }

  getOrders() {
      this.errorReceived = false;
      this.service.getOrdersByCustomer()
          .pipe(catchError((err) => this.handleError(err)))
          .subscribe(orders => {        
             
              // orders.map(order=>{
              //   debugger
              //   order.orderDate=order.orderDate? moment(order.orderDate).format("MM/DD/YYYY"):order.orderDate;
              // })
              this.orders = orders;
              console.log('orders items retrieved: ' + orders.length);
      });
  }

  get sliceOrders(): IOrder[] {
   
    if ( !this.orders) return  [];
    return this.orders
      .map((country, i) => ({id: i + 1, ...country}))
      .slice((this.page - 1) * this.pageSize, (this.page - 1) * this.pageSize + this.pageSize);
  }

  onSort1({column, direction}: SortEvent) {

    // resetting other headers
    this.headers.forEach(header => {
      if (header.sortable !== column) {
        header.direction = '';
      }
    });

    // sorting countries
    if (direction === '' || column === '') {
      this.getOrders();
    } else {
      this.orders = [...this.orders].sort((a, b) => {
        const res = compare(`${a[column]}`, `${b[column]}`);
        return direction === 'asc' ? res : -res;
      });
    }
  }
  private handleError(error: any) {
      this.errorReceived = true;
      return Observable.throw(error);
   
  }

  

}
