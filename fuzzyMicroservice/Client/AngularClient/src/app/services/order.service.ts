import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { IOrder } from '@models/order';
import { Observable } from 'rxjs';

@Injectable()
export class OrderService {
    constructor(private httpclient: HttpClient) {}

    saveOrder(order:IOrder) {
        return this.httpclient.post<any>(`${environment.apiUrl}/order/addorder`,order);
    }

    getOrdersByCustomer(): Observable<IOrder[]> {
        let url = `${environment.apiUrl}/customer/orders`;
         return this.httpclient.get<IOrder[]>(url);
      }
}