import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CategoryRoutingModule } from './category-routing.module';
import { CategoryComponent } from './category.component';
import { CategoryProductComponent } from './category-product/category-product.component';


@NgModule({
  declarations: [CategoryComponent, CategoryProductComponent],
  imports: [
    CommonModule,
    CategoryRoutingModule
  ]
})
export class CategoryModule { }
