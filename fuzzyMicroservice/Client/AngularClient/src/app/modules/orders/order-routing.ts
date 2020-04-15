import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { OrderComponent } from '@modules/orders/order/order.component';
import { OrderNewComponent } from './order-new/order-new.component';

const routes: Routes = [
    { path: '', component: OrderComponent },
    { path: 'order-new', component: OrderNewComponent },
    

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class OrderRoutingModule { }
