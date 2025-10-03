import { HttpClient } from '@angular/common/http';
import { environmentDev } from '../../../environments/environment.development';
import { Injectable } from '@angular/core';
import { catchError, map, Observable, tap, throwError } from 'rxjs';
import { SessionManagement } from '../session/session-management';
import {
  IrefreshToken,
  loggedUser,
  LoginResponse,
} from '../session/models/cls-user';
import { frmChangePassword } from '../../models/requests';
import { Adapter } from '../adapter/adapter';
import { FormGroup } from '@angular/forms';
import { enRole } from '../../../shared/enums/roles';

@Injectable({
  providedIn: 'root',
})
export class AuthManagement {
  constructor(
    private http: HttpClient,
    private sessionService: SessionManagement,
    private adapter: Adapter
  ) {}
  baseURL = environmentDev.baseURL;

  login(natId: string, password: string): Observable<void> {
    return this.http
      .post<LoginResponse>(
        `${this.baseURL}/auth/login`,
        {
          username: natId,
          password: password,
        },
        {
          withCredentials: true,
        }
      )
      .pipe(
        map((responses) => {
          if (responses) {
            console.log('the login response');
            const myLoggedUser: loggedUser = {
              username: responses.username,
              accessTokenExpirationDate: responses.accessTokenExpiration,
              refreshTokenExpirationDate: responses.refreshTokenExpiration,
              role: responses.role,
            };
            this.sessionService.setSession(myLoggedUser);
          }
        }),
        map(() => {}),
        catchError((error) => {
          console.error('Error in login pipe:', error);
          return throwError(() => error);
        })
      );
  }

  changePassword(
    frmChangePassword: FormGroup<frmChangePassword>
  ): Observable<void> {
    const body = this.adapter.frmChangePasswordAdapter(frmChangePassword);
    console.log('the change password body', body);
    return this.http
      .patch<any>(`${this.baseURL}/auth/change-password`, body, {
        withCredentials: true,
      })
      .pipe(
        catchError((error) => {
          return throwError(() => error);
        })
      );
  }

  logout(): Observable<void> {
    this.sessionService.endSession();
    return this.http
      .post<any>(`${this.baseURL}/auth/logout`, {
        withCredentials: true,
      })
      .pipe(
        catchError((error) => {
          return throwError(() => error);
        })
      );
  }

  refreshToken(): Observable<void> {
    return this.http
      .post<any>(
        `${this.baseURL}/auth/token/refresh-token`,
        {},
        {
          withCredentials: true,
        }
      )
      .pipe(
        tap((response) => {
          console.log('refresh token is working');
          this.sessionService.updateRefreshToken({
            accessTokenExpirationDate: response.accessTokenExpiration,
            refreshTokenExpirationDate: response.refreshTokenExpiration,
          });
        }),
        map(() => {}),
        catchError((error) => {
          console.log(`error while refreshing the token ${error}`);
          this.logout();
          return throwError(() => error);
        })
      );
  }
}
