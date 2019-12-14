import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Category } from '../modules/category/category/category.component';
import { Observable } from 'rxjs/Observable';
import { environment } from '../../environments/environment';

@Injectable()
export class CategoryService {

  constructor(private http: HttpClient) { }

  getCategories(): Observable<Category[]> {
    let url = `${environment.apiUrl}category/GetCategories`
    return this.http.get<Category[]>(url);
  }
 

}