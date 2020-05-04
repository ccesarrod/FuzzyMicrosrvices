import { Injectable } from '@angular/core';
import {
    Router, Resolve,
    RouterStateSnapshot,
    ActivatedRouteSnapshot
  }  from '@angular/router'

import { Product } from '@models/product-models';
import { ProductService } from '@services/product.service';

@Injectable({
  providedIn: 'root',
})
export class CategoryProductResolverService  implements Resolve<Product[]>{

  constructor(private productService: ProductService ) { }
    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): import("rxjs").Observable<Product[]> {
        const id= route.params['id'];
      return this.productService.getProductsByCategoryId(id);
    }

}