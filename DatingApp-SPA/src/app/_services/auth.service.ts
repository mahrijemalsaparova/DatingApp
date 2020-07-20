import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
import { environment } from 'src/environments/environment';
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

constructor(private http: HttpClient) {}

login(model: any)  {
  return this.http.post(this.baseUrl + 'login', model).pipe(
     map((response: any) => {
      const user = response;
      if (user) {
        localStorage.setItem('token', user.token);
        this.decodedToken = this.jwtHelper.decodeToken(user.token);
        console.log(this.decodedToken);
      }
    })
  );
 }

register(model: any) {
                      // http://localhost:5000/api/register
return this.http.post(this.baseUrl + 'register', model);
}

loggedIn() {
  const token = localStorage.getItem('token');
  return !this.jwtHelper.isTokenExpired(token);
}

}
