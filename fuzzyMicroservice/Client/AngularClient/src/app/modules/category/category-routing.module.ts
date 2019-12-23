import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { CategoryComponent } from './category/category.component';
import {CategoryProductComponent} from './category-product/category-product.component'

const routes: Routes = [
  { path: '', component: CategoryComponent },
  { path: 'category-product/:id', component: CategoryProductComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CategoryRoutingModule { }