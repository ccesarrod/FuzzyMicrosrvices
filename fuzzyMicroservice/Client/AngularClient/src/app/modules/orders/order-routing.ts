import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { OrderComponent } from '@modules/orders/order/order.component';
import { OrderNewComponent } from './order-new/order-new.component';
import { ThankyouComponent } from './thankyou/thankyou.component';
import { OrderDetailsResolver } from './order-details.resolver';
import { OrderDetailsComponent } from './order-details/order-details.component';

const routes: Routes = [
    { path: '', component: OrderComponent },
    { path: 'order-new', component: OrderNewComponent },
    { path: 'order-detail/:id', 
    component: OrderDetailsComponent,
    resolve: {details:  OrderDetailsResolver  }  },
    {path:'order-complete', component:ThankyouComponent }
    

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class OrderRoutingModule { }
