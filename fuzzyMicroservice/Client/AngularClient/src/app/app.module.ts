import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HTTP_INTERCEPTORS, HttpClientModule } from "@angular/common/http";
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavmenuComponent } from '@shared/navmenu/navmenu.component';
import { CategoryModule } from '@modules/category/category.module';
import { ProductModule } from '@modules/product/product.module';
import { HomeComponent } from '@modules/home/home.component';
import { AccountModule } from '@modules/account/account.module';
import { JwtInterceptor} from './services/jwtInterceptor';
import {CartService } from './services/cart.service';
import {OrderService} from '@services/order.service'
import { AppToastService } from '@services/app-toast.service';
import { AppToastComponent } from './shared/app-toast/app-toast.component';
import {OrdersModule} from '@modules/orders/orders.module';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
@NgModule({
  declarations: [
    AppComponent,
    NavmenuComponent,
    HomeComponent,
    AppToastComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    CategoryModule,
    ProductModule,
    OrdersModule,
    AccountModule,
    NgbModule
  ],
  providers: [CartService,
              OrderService,
             { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
             AppToastService],
  bootstrap: [AppComponent]
})
export class AppModule { }
