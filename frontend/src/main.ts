import { bootstrapApplication } from '@angular/platform-browser';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { provideRouter } from '@angular/router';
import { AppComponent } from './app/app.component';
import { routes } from './app/app.routes';
import { apiErrorInterceptor } from './app/core/interceptors/api-error.interceptor';
import { apiHeaderInterceptor } from './app/core/interceptors/api-header.interceptor';

bootstrapApplication(AppComponent, {
  providers: [
    provideRouter(routes),
    provideHttpClient(withInterceptors([apiHeaderInterceptor,apiErrorInterceptor]))
  ]
}).catch(err => console.error(err));
