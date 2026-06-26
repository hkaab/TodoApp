import { HttpInterceptorFn } from '@angular/common/http';

export const apiHeaderInterceptor: HttpInterceptorFn = (req, next) => {

  const clonedRequest = req.clone({
    setHeaders: {
      'X-Api-Version': '1.0',
      'X-Client': 'TodoApp',
      'X-Correlation-Id': crypto.randomUUID()
    }
  });

  return next(clonedRequest);
};