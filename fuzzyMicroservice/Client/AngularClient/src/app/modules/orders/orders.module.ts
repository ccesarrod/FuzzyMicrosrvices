import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrderComponent } from './order/order.component';
import { OrderRoutingModule } from './order-routing';
import { OrderNewComponent } from './order-new/order-new.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ThankyouComponent } from './thankyou/thankyou.component';
import { OrderDetailsComponent } from './order-details/order-details.component';
import {NgbPaginationModule} from '@ng-bootstrap/ng-bootstrap';


@NgModule({
  declarations: [OrderComponent, OrderNewComponent, ThankyouComponent, OrderDetailsComponent],
  imports: [
    CommonModule,
    OrderRoutingModule,
    FormsModule,   
    ReactiveFormsModule,
    NgbPaginationModule
    
  ]
})
export class OrdersModule { }
