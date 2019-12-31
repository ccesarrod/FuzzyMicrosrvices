import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AccountService } from '@services/account.service';
import {ICartItem} from '@models/cartItem-model'
import { CartService } from '@services/cart.service';
@Component({
  selector: 'app-navmenu',
  templateUrl: './navmenu.component.html',
  styleUrls: ['./navmenu.component.sass']
})
export class NavmenuComponent implements OnInit {

  isExpanded = false;
  shoppingCartItems: Observable<ICartItem[]>;
  userName: string = "";
  loginAction: string = "Login";
  isLogin: boolean = false;

  constructor( private router:Router, 
    private authenticationService:AccountService,
    private cartService:CartService) {}

  collapse() {
    this.isExpanded = false;
  }

  ngOnInit() {

    this.shoppingCartItems = this.cartService.getCart();
    this.shoppingCartItems.subscribe(p => p);    
     this.authenticationService.currentUser.subscribe(currentUser =>
    {
      if (currentUser !== null && currentUser.userName !== undefined) {
        const user = currentUser;
        this.userName = user.userName;
        this.isLogin = true;
        this.loginAction = 'Log out'
      }
    },
      err => {
        console.log(err);
      }); 
  }
  toggle() {
    this.isExpanded = !this.isExpanded;
  }

   login() {
    if (this.isLogin) {
      this.authenticationService.logout();
      this.isLogin = false;
      this.loginAction = 'Login';
      this.userName = '';
      this.cartService.clearCart();
      this.router.navigate(['/']);
    }
    else {
      this.router.navigate(['/account/login']);
    }
  } 

}
