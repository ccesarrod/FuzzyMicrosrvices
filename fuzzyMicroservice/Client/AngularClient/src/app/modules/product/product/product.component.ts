import { Component, OnInit } from '@angular/core';
import {ProductService } from '@services/product.service';
import { Product } from '@models/product-models';
import { ActivatedRoute } from '@angular/router';


@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.sass']
})
export class ProductComponent implements OnInit {
  public products: Product[];
  constructor(private service: ProductService,private route: ActivatedRoute ) { }

  ngOnInit() {
    // this.service.getProducts().subscribe(result => {
    //   this.products = result;
    // }, error => console.error(error));
    this.products = this.route.snapshot.data.products;
  }

}
