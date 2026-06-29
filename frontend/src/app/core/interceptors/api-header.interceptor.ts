import { HttpInterceptorFn } from '@angular/common/http';

export const apiHeaderInterceptor: HttpInterceptorFn = (req, next) => {

      if (!localStorage.getItem('userId')) {
      localStorage.setItem(
        'userId',
        crypto.randomUUID()
      );
    }
      
  const userId = localStorage.getItem('userId') ?? '';

  const clonedRequest = req.clone({
    setHeaders: {
      'X-Api-Version': '1.0',
      'X-Client': 'TodoApp',
      'X-Correlation-Id': crypto.randomUUID(),
      'X-User-Id': userId
    }
  });

  return next(clonedRequest);
};