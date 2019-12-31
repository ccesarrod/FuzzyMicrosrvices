import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { LoginComponent } from './login/login.component';
import { AccountRoutingModule } from './account-routing.module';
import { RegisterComponent } from './register/register.component';
import { CartComponent } from './cart/cart.component';



@NgModule({
  declarations: [LoginComponent, RegisterComponent, CartComponent],
  imports: [
    CommonModule,
    AccountRoutingModule, 
    FormsModule,   
    ReactiveFormsModule
  ]
})
export class AccountModule { }
