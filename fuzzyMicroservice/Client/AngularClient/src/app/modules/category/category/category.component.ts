import { Component, OnInit } from '@angular/core';
import { CategoryService } from '../../../services/category.service';
@Component({
  selector: 'app-category',
  templateUrl: './category.component.html',
  styleUrls: ['./category.component.css']
})
export class CategoryComponent implements OnInit {

  categories: Category[];
  constructor(private service: CategoryService ) {}

  ngOnInit() {
    this.service.getCategories().subscribe(result => {
      this.categories = result;
    }, error => console.error(error));
  }


}
export interface Category {
  categoryId: number;
  categoryName: string;
  Description: string;
}