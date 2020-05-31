import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Product } from '@models/product-models';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class ProductService {

  constructor(private http: HttpClient) { }

  getProducts(): Observable<Product[]> {
    let url = `${environment.apiUrl}/product/getAll`;
     return this.http.get<Product[]>(url);
  }

  getProductsByCategoryId(categoryId: string): Observable<Product[]> {
    let url = `${environment.apiUrl}/catalog/ProductsByCategoryId/${categoryId}`;
    return this.http.get<Product[]>(url);
  }
}
