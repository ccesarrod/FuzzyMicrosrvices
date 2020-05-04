import { Injectable } from '@angular/core';
import {
    Router, Resolve,
    RouterStateSnapshot,
    ActivatedRouteSnapshot
  }  from '@angular/router'
import { Category } from '@models/category-model';
import { CategoryService } from '@services/category.service';

@Injectable({
  providedIn: 'root',
})
export class CategoryResolverService  implements Resolve<Category[]>{

  constructor(private service: CategoryService ) { }
    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): import("rxjs").Observable<Category[]> {
      
      return this.service.getCategories()
    }

}