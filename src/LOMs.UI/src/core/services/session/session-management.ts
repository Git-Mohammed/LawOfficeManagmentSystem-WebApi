import { Injectable } from '@angular/core';
import { IrefreshToken, loggedUser } from './models/cls-user';

@Injectable({
  providedIn: 'root',
})
export class SessionManagement {
  private sessionKey = 'user_session';

  // Set the session data in localStorage
  setSession(sessionData: loggedUser): void {
    localStorage.setItem(this.sessionKey, JSON.stringify(sessionData));
  }
  // Retrieve session data from localStorage
  getSession(): loggedUser | null {
    const session = localStorage.getItem(this.sessionKey);
    return session ? JSON.parse(session) : null;
  }
  // End the session by removing the session data
  endSession(): void {
    localStorage.removeItem(this.sessionKey);
  }
  // Check if the user is authenticated
  isAuthenticated(): boolean {
    return !!this.getSession();
  }

  isAccessTokenExpired(): boolean {
    const currentSession = this.getSession();
    if (currentSession) {
      return (
        Number(currentSession.accessTokenExpirationDate) < Number(Date.now())
      );
    } else {
      return true;
    }
  }

  isRefreshTokenExpired(): boolean {
    const currentSession = this.getSession();
    if (currentSession) {
      return (
        Number(currentSession.refreshTokenExpirationDate) > Number(Date.now())
      );
    } else {
      return false;
    }
  }

  updateRefreshToken(refreshToken: IrefreshToken) {
    const currentSession = this.getSession();
    if (currentSession) {
      console.log('logged user before refresh', currentSession);
      currentSession.accessTokenExpirationDate =
        refreshToken.accessTokenExpirationDate;
      currentSession.refreshTokenExpirationDate =
        refreshToken.refreshTokenExpirationDate;
      this.setSession(currentSession);
    }
  }
}
