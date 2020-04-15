import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { IOrder } from '@models/order';

@Injectable()
export class OrderService {
    constructor(private httpclient: HttpClient) {}

    saveOrder(order:IOrder) {
        return this.httpclient.post<any>(`${environment.apiUrl}/order`,order);
    }
}