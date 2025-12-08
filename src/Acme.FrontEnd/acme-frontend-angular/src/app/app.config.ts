import { ApplicationConfig, InjectionToken, provideBrowserGlobalErrorListeners, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';


import { routes } from './app.routes';

export const API_BASE_URL = new InjectionToken<string>('API_BASE_URL');
export const USER_API_BASE_URL = new InjectionToken<string>('USER_API_BASE_URL');
export const RECOMMENDATION_API_BASE_URL = new InjectionToken<string>('RECOMMENDATION_API_BASE_URL');


export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(),
    { provide: API_BASE_URL, useValue: 'http://localhost:5014' },
    { provide: USER_API_BASE_URL, useValue: "http://localhost:5014" },
    { provide: RECOMMENDATION_API_BASE_URL, useValue: "http://localhost:5014" }
  ]
};
