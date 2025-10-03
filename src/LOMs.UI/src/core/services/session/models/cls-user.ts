import { enRole } from '../../../../shared/enums/roles';

export interface loggedUser {
  username:string
  accessTokenExpirationDate: Date;
  refreshTokenExpirationDate: Date;
  role: enRole;
}

export interface IrefreshToken {
  accessTokenExpirationDate: Date;
  refreshTokenExpirationDate: Date;
}

export interface LoginResponse {
  username: string;
  role: enRole;
  accessTokenExpiration: Date;
  refreshTokenExpiration: Date;
}
