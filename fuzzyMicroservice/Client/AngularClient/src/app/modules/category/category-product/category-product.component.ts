
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { ProductService } from '@services/product.service';
import { Product } from '@models/product-models';
import { CartService } from '@services/cart.service';

@Component({
  selector: 'app-category-product',
  templateUrl: './category-product.component.html',
  styleUrls: ['./category-product.component.sass']
})
export class CategoryProductComponent implements OnInit {
  products: Product[];

  constructor(private route: ActivatedRoute,
    private productService: ProductService,
    private cartService: CartService
   ) { }

  ngOnInit() {
    this.getProducts();
  }
 
  getProducts(){
   // this.products= this.route.snapshot.data.products;
   var id = this.route.snapshot.paramMap.get('id');
    this.productService.getProductsByCategoryId(id)
      .subscribe(result => {
        this.products = result;
      }, error => console.log(error));
  }

  addToCart(item: Product) {
    this.cartService.addProduct(item);
  }

}
