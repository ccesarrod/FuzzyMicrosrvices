import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CategoryRoutingModule } from './category-routing.module';
import { CategoryComponent } from './category/category.component';
import { CategoryProductComponent } from './category-product/category-product.component';
import { CategoryService } from '@services/category.service';
import { ProductService } from '@services/product.service';


@NgModule({
  declarations: [CategoryComponent, CategoryProductComponent],
  imports: [
    CommonModule,
    CategoryRoutingModule
  ],
  providers:[CategoryService,ProductService]
})
export class CategoryModule { }
