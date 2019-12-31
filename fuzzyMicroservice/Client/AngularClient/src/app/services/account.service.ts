import { Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { User } from '@models/user-model';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private currentUserSubject: BehaviorSubject<User>;
  public currentUser: Observable<User>;
  
  constructor(private http: HttpClient) {
    this.currentUserSubject = new BehaviorSubject<User>(JSON.parse(localStorage.getItem('currentUser')));
    this.currentUser = this.currentUserSubject.asObservable();
  }

  login(username: string, password: string) {  
    this.logout();
 
    if (!localStorage.getItem('currentUser')) {
      return this.http.post<any>(`${environment.apiUrl}/account/login`, { userName: username, password: password })
        .pipe(map(user => {
          // login successful if there's a jwt token in the response
          if (user) {
            // store user details and jwt token in local storage to keep user logged in between page refreshes
            localStorage.setItem('currentUser', JSON.stringify(user));
           
            this.currentUserSubject.next(user);
           // this.cartService.assignCart(user.cart);
          }
          return user;
        }));
    }
  }

  public get currentUserValue() {
    return this.currentUserSubject.value;
  }
  public get authenticatedUser():Observable<User> {
   return  this.currentUser;
  }

  logout() {
      
    localStorage.removeItem('currentUser');
    this.currentUserSubject.next(null);  
  }

  register(user: User) {
    return this.http.post<any>(`${environment.apiUrl}/account/register`, user);
  }
}
