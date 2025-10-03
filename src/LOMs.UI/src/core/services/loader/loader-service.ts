import { Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable({
  providedIn: 'root',
})
export class LoaderService {
  constructor(private _service: NgxSpinnerService) {}
  loading() {
    this._service.show(undefined, {
      bdColor: 'rgba(0, 0, 0, 0.8)',
      size: 'medium',
      color: '#fff',
      zIndex: 9999,
      type: 'square-jelly-box',
      fullScreen: true,
    });
    this._service.show();
  }

  hideLoader() {
    this._service.hide();
  }
}
