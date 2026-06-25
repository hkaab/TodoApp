import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { CreateTodoRequest, Todo, UpdateTodoRequest } from '../models/todo.model';

@Injectable({ providedIn: 'root' })
export class TodoService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/todos`;

  getAll(): Observable<Todo[]> {
    return this.http.get<Todo[]>(this.baseUrl);
  }

  create(request: CreateTodoRequest): Observable<Todo> {
    return this.http.post<Todo>(this.baseUrl, request);
  }

  update(id: string, request: UpdateTodoRequest): Observable<Todo> {
    return this.http.put<Todo>(`${this.baseUrl}/${id}`, request);
  }

  toggle(id: string): Observable<Todo> {
    return this.http.patch<Todo>(`${this.baseUrl}/${id}/toggle`, {});
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
