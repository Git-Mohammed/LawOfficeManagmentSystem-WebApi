import { environmentDev } from "../../environments/environment.development";

export class AppService {
  baseURL = environmentDev.baseURL;
}
