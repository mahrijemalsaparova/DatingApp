import { Component, OnInit } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { User } from './_models/user';
import { AuthService } from './_services/auth.service';
// This class is responsible to providing the data for our view
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  jwtHelper = new JwtHelperService();

  constructor(private authService: AuthService) {}

  ngOnInit() {
    const token = localStorage.getItem('token');
    // nav içindeki foto için
    const user: User = JSON.parse(localStorage.getItem('user'));
    if (token) {
      this.authService.decodedToken = this.jwtHelper.decodeToken(token);
    }
    if (user) {
      // localstrorage dan çekilen userı authServisteki currentUsera ata
      this.authService.currentUser = user;
      // navdaki foto için // authMemberdaki changeMemberPhoto metonu çagır ve user.photoUrl'ı ata
      // Bu da demektir ki bütün componentler subscribe oldugunda update edilen bu fotoyu çagrır
      this.authService.changeMemberPhoto(user.photoUrl);
    }
  }
}
