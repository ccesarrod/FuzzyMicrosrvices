import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductService } from '@services/product.service';
import { ProductRoutingModule } from './product-routing.module';
import { ProductComponent } from './product/product.component';
import {ProductResolverService} from './product.resolver'

@NgModule({
  declarations: [ProductComponent],
  imports: [
    CommonModule,
    ProductRoutingModule
  ],
  providers:[ProductService,ProductService]
})
export class ProductModule { }
