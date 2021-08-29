import { Component, OnInit } from '@angular/core';
import { LoginUserDto } from 'src/dto/models';
import { AuthService } from './_services/authentication.service';
import { SessionService } from './_services/session.service';

@Component({
  selector: 'lms-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  constructor(private readonly authService: AuthService, private readonly sessionService: SessionService) {}

  ngOnInit(): void {
    this.setCurrentUser();
  }

  private setCurrentUser(): void {
    if (localStorage.getItem('user')) {
      const user: LoginUserDto = JSON.parse(localStorage.getItem('user')!);

      if (user) {
        this.authService.setCurrentUser(user);
      }
    }
  }
}
