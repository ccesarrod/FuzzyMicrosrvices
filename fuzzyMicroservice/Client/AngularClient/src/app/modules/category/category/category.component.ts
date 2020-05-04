import { Component, OnInit } from '@angular/core';
import { CategoryService } from '@services/category.service';
import {Category} from '@models/category-model';
import { ActivatedRoute } from '@angular/router';
@Component({
  selector: 'app-category',
  templateUrl: './category.component.html',
  styleUrls: ['./category.component.css']
})
export class CategoryComponent implements OnInit {

  categories: Category[];
  constructor(private service: CategoryService, private route: ActivatedRoute ) {}

  ngOnInit() {  
    /* this.service.getCategories().subscribe(result => {
      this.categories = result;
    }, error => console.error(error)); */

    this.categories= this.route.snapshot.data.categories
   
  }


}
