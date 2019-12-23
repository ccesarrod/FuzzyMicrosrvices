import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductService } from '@services/product.service';
import { ProductRoutingModule } from './product-routing.module';
import { ProductComponent } from './product/product.component';


@NgModule({
  declarations: [ProductComponent],
  imports: [
    CommonModule,
    ProductRoutingModule
  ],
  providers:[ProductService]
})
export class ProductModule { }
