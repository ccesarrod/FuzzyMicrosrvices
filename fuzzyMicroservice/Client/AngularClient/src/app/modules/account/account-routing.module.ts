import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { LoginComponent } from "@modules/account/login/login.component";


const routes: Routes = [
  { path: "login", component: LoginComponent }
 // { path: "register", component: RegisterComponent },
 // { path: "cart", component: CartComponent 
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AccountRoutingModule { }
