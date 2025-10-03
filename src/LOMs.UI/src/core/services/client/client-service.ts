import { Injectable } from '@angular/core';
import { AppService } from '../../../shared/common/app-service';
import { HttpClient, HttpContext } from '@angular/common/http';
import { INewClientForm } from '../../models/requests';
import { FormGroup } from '@angular/forms';
import { Adapter } from '../adapter/adapter';
import { map, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ClientService extends AppService {
  /**
   *
   */
  constructor(private http: HttpClient, private adapter:Adapter) {
    super();
  }

  add(client: FormGroup<INewClientForm>) : Observable<void> {
    return this.http.post(`${this.baseURL}/clients`, this.adapter.toAddClientAPI(client)).pipe(map(() => {}));
  }
}
