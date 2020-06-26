import { Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { User } from '@models/user-model';
import { environment } from 'src/environments/environment';
import { CartService } from './cart.service';
//import { userInfo } from '@models/user-model';

@Injectable({
  providedIn: 'root'
})
export class AccountService{
  private currentUserSubject: BehaviorSubject<User>;
  public currentUser: Observable<User>;
   // store the URL so we can redirect after logging in
   redirectUrl: string;
   userData:User;
   isLoginSubject = new BehaviorSubject<boolean>(this.hasData());

  constructor(private http: HttpClient, private cartService:CartService) {
    this.currentUserSubject = new BehaviorSubject<User>(JSON.parse(localStorage.getItem('currentUser')));
    this.currentUser = this.currentUserSubject.asObservable();
  }

 

  login(username: string, password: string) {  
    this.logout();
 
    if (!localStorage.getItem('currentUser')) {
      return this.http.post<User>(`${environment.apiUrl}/account/login`, { userName: username, password: password })
        .pipe(map(user => {
          // login successful if there's a jwt token in the response
          if (user) {
            // store user details and jwt token in local storage to keep user logged in between page refreshes
            localStorage.setItem('currentUser', JSON.stringify(user));         
            this.userData = user;
            this.currentUserSubject.next(user);    
            this.cartService.assignCart(user.cart);
          }
          return user;
        }));
    }
  }

  public get currentUserValue() {

    if (this.currentUserSubject)
      return this.currentUserSubject.value;
    else
      return null;
  }

  // public get isLoggedIn(){
  //   return !this.currentUserValue? false:true;
  // }

  isLoggedIn() :boolean {
    return this.hasData();
  }

  public get authenticatedUser():Observable<User> {

    if (this.userData){
      //const user = localStorage.getItem('currentUser')
      const x = JSON.parse(localStorage.getItem('currentUser'));
      return of(this.userData);
    }
   return  this.currentUser;
  }

  logout() {
    
    localStorage.removeItem('currentUser');
    this.userData = null;
      this.currentUserSubject.next(null);  
    
  }

  register(user: User) {
    return this.http.post<any>(`${environment.apiUrl}/account/register`, user);
  }

  private hasData() : boolean {
    return !!localStorage.getItem('currentUser');
  }
}
