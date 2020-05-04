import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { CategoryComponent } from './category/category.component';
import { CategoryProductComponent } from './category-product/category-product.component'
import { CategoryResolverService } from './category-resolver'
import { CategoryProductResolverService } from './category-product.resolver';

const routes: Routes = [
  {
    path: '',
    component: CategoryComponent,
    resolve: { categories: CategoryResolverService }
  },
  {
    path: 'category-product/:id',
    component: CategoryProductComponent
  }
]

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
  providers: [CategoryResolverService, CategoryProductResolverService]
})
export class CategoryRoutingModule { }
