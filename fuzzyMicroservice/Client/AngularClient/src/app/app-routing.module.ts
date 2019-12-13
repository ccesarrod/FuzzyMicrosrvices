import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';


const routes: Routes = [{ path: 'categories', loadChildren: () => import('./modules/category/category.module').then(m => m.CategoryModule) },  
{ path: 'products', loadChildren: () => import('./modules/product/product.module').then(m => m.ProductModule) }];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
