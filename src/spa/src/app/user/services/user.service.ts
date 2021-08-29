import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { AuthService } from 'src/app/_services/authentication.service';
import { PhotoResponseDto, UserForDetailedDto, UserForUpdateDto, UserPasswordResetRequest } from 'src/dto/models';
import { environment } from 'src/environments/environment';

@Injectable()
export class UserService {
  baseUrl = environment.apiUrl;
  userUrl = this.baseUrl + 'user';

  constructor(private readonly httpService: HttpClient, private readonly authService: AuthService) {}

  getUser(userId: number): Observable<UserForDetailedDto> {
    return this.httpService.get<UserForDetailedDto>(`${this.userUrl}/${userId}`);
  }

  updatedUserProfile(userForUpdate: UserForUpdateDto): Observable<UserForDetailedDto> {
    return this.httpService
      .put<UserForDetailedDto>(`${this.userUrl}`, userForUpdate)
      .pipe(tap(returnedUser => this.authService.changeUserDetails(returnedUser)));
  }

  resetUserPassword(resetRequest: UserPasswordResetRequest): Observable<void> {
    return this.httpService.post<void>(`${this.userUrl}`, resetRequest);
  }

  changeUserPhoto(userId: number, formData: FormData): Observable<PhotoResponseDto> {
    return this.httpService
      .post<PhotoResponseDto>(`${this.baseUrl}photo/userPhoto/${userId}`, formData)
      .pipe(tap(returnedPhoto => this.authService.changeMemberPhoto(returnedPhoto.url)));
  }
}
