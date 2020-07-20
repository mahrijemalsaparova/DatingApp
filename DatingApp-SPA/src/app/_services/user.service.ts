import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';



@Injectable({
  providedIn: 'root'
})
export class UserService {
  baseUrl = environment.apiUrl;

constructor(private http: HttpClient) {}

getUsers(): Observable<User[]> {
                              // http://localhost:5000/api/users
  return  this.http.get<User[]>(this.baseUrl + 'users');
}

getUser(id): Observable<User> {
                           // http://localhost:5000/api/users/2
 return this.http.get<User>(this.baseUrl + 'users/' + id);
}

}
