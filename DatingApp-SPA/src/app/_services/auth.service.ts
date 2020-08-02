import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {BehaviorSubject} from 'rxjs';
import { map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';
// this allows us to inject things into our service.
@Injectable({
  providedIn: 'root'
})
export class AuthService {
  // http://localhost:5000/api/auth/
  baseUrl = environment.apiUrl + 'auth/';
  // This library provides an HttpInterceptor which automatically attaches a JSON Web Token to HttpClient requests.
  jwtHelper = new JwtHelperService();
  decodedToken: any;

  currentUser: User; // nav kısmındaki user için tanımlandı

  // for any to any component
  photoUrl = new BehaviorSubject<string>('../../assets/user.png');
  currentPhotoUrl = this.photoUrl.asObservable();

constructor(private http: HttpClient) {}
 // for any to any component
changeMemberPhoto(photoUrl: string) {
  // next is method of behaviorsubject // burada next metodu photoUrl 'deki adres yerine currentPhotoUrl 'e başka adres update eder
this.photoUrl.next(photoUrl);
}

login(model: any)  {
  return this.http.post(this.baseUrl + 'login', model).pipe(
     map((response: any) => {
      const user = response;
      if (user) {
        localStorage.setItem('token', user.token);
        // nav kısmındaki foto için user getirecek
        localStorage.setItem('user', JSON.stringify(user.user));
        this.decodedToken = this.jwtHelper.decodeToken(user.token);
        //  nav kısmındaki foto için
        this.currentUser = user.user;
     // user login olduğunda nav bardakı fotosu update olucak
        this.changeMemberPhoto(this.currentUser.photoUrl); // giriş yapan kullanıcının fotograf url sini changeMemberPhoto metoduna gönderir
      }
    })
  );
 }

register(user: User) {
                      // http://localhost:5000/api/register
return this.http.post(this.baseUrl + 'register', user);
}

loggedIn() {
  const token = localStorage.getItem('token');
  return !this.jwtHelper.isTokenExpired(token);
}

roleMatch(allowedRoles): boolean {
  let isMatch = false;
  const userRoles = this.decodedToken.role as Array<string>;
  allowedRoles.forEach(element => {
    if (userRoles.includes(element)) {
      isMatch = true;
      return;
    }
  });
  return isMatch;
}

}
