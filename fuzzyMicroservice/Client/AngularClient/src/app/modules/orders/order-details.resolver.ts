import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { ICartItem } from '@models/cartItem-model';
import { OrderService } from '@services/order.service';

@Injectable({ providedIn: 'root' })
export class OrderDetailsResolver implements Resolve<ICartItem> {
  constructor(private service: OrderService) {}

  resolve(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<any>|Promise<any>|any {
debugger
    return this.service.getOrderDetails(route.paramMap.get('id'));
  }
}