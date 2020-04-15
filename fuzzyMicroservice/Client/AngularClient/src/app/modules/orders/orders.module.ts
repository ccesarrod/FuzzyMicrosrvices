import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrderComponent } from './order/order.component';
import { OrderRoutingModule } from './order-routing';
import { OrderNewComponent } from './order-new/order-new.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';


@NgModule({
  declarations: [OrderComponent, OrderNewComponent],
  imports: [
    CommonModule,
    OrderRoutingModule,
    FormsModule,   
    ReactiveFormsModule
  ]
})
export class OrdersModule { }
