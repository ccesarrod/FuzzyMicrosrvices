import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators, FormControl } from '@angular/forms';
import { CartService } from '@services/cart.service';
import { ICartItem } from '@models/cartItem-model';
import { IOrder } from '@models/order';
import { OrderService } from '@services/order.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-order-new',
  templateUrl: './order-new.component.html',
  styleUrls: ['./order-new.component.scss']
})
export class OrderNewComponent implements OnInit {
  newOrderForm: FormGroup;  // new order form
  isOrderProcessing: boolean;
  errorReceived: boolean;
  cart: ICartItem[];

  constructor(private cartService: CartService, 
        private orderService: OrderService,
        private router:Router) { }



  ngOnInit() {
    this.cartService.getCart().subscribe(data => {    
      this.cart = data;
    });
    this.newOrderForm = new FormGroup({
      street: new FormControl({ value: '', disabled: true }, [Validators.required]),
      city: new FormControl({ value: '', disabled: true }, [Validators.required]),
      state: new FormControl({ value: '', disabled: true }, [Validators.required]),
      country: new FormControl({ value: '', disabled: true }, [Validators.required])
      /* cardnumber: new FormControl({ value: '', disabled: true }, [Validators.required]),
      cardholdername: new FormControl({ value: '', disabled: true }, [Validators.required]),
      expirationdate: new FormControl({ value: '', disabled: true }, [Validators.required]),
      securitycode: new FormControl({ value: '', disabled: true }, [Validators.required]) */
    });
    this.newOrderForm.enable();
  }

  submitForm(orderForm) {
    const raw = this.newOrderForm.getRawValue();
    const order: IOrder = {
      ShipCity: raw.city,
      ShipAddress: raw.street,
      ShipRegion: raw.state,
      ShipCountry: raw.country,
      ShipPostalCode : raw.zipcode,
     /*  cardnumber: raw.cardnumber,
      cardexpiration: raw.expirationdate,
      expiration: raw.expirationdate,
      cardsecuritynumber: raw.cardsecuritynumber,
      cardholdername: raw.cardholdername,
      cardtypeid: raw.cardtypeid,
      total: this.cartService.totalPrice(),      */
      Order_Detail:this.cart
    };

    this.orderService.saveOrder(order).subscribe(x => {
      this.cartService.emptyCart().subscribe(y=>{
      this.router.navigate(['order-complete']);
      })
    })

  }

  get total() {
    return this.cartService.totalPrice();
  }

}
