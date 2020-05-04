import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { AccountService } from '@services/account.service';

@Injectable({
  providedIn: 'root',
})
export class AuthProductGuard implements CanActivate {
    constructor(private authService:AccountService,private router: Router){}
   
    canActivate(
        next: ActivatedRouteSnapshot,
        state: RouterStateSnapshot): boolean {
        let url: string = state.url;
    
        return this.checkLogin(url);
      }
    
      checkLogin(url: string): boolean {
        if (this.authService.isLoggedIn) { return true; }
    
        // Store the attempted URL for redirecting
        this.authService.redirectUrl = url;
    
        // Navigate to the login page with extras
        this.router.navigate(['/login']);
        return false;
      }
}