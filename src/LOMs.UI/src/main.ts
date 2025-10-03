import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { App } from './app/app';
import { importProvidersFrom } from '@angular/core';
import { NgxSpinnerModule } from 'ngx-spinner';
import { provideAnimations } from '@angular/platform-browser/animations';

bootstrapApplication(App, appConfig)
  .catch((err) => console.error(err));
importProvidersFrom(NgxSpinnerModule.forRoot({ type: 'square-jelly-box' }));
provideAnimations();