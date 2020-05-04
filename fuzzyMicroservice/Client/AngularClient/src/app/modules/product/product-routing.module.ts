import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ProductComponent } from './product/product.component';
import { AuthProductGuard } from './product.guard';
import { ProductResolverService } from './product.resolver';

const routes: Routes = [{ path: '', 
                  component: ProductComponent,
                  canActivate: [AuthProductGuard],
                  resolve: {products: ProductResolverService } }
                
                ];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
  providers: [ProductResolverService]
})
export class ProductRoutingModule { }
