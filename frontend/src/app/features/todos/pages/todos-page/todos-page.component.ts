import { CommonModule } from '@angular/common';
import { Component, OnInit, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { finalize } from 'rxjs';
import { Todo } from '../../models/todo.model';
import { TodoService } from '../../services/todo.service';

@Component({
  selector: 'app-todos-page',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './todos-page.component.html'
})
export class TodosPageComponent implements OnInit {
  private readonly todoService = inject(TodoService);
  private readonly fb = inject(FormBuilder);

  readonly todos = signal<Todo[]>([]);
  readonly loading = signal(false);
  readonly error = signal<string | null>(null);
  readonly editingId = signal<string | null>(null);

  readonly form = this.fb.nonNullable.group({
    title: ['', [Validators.required, Validators.maxLength(200)]]
  });

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.loading.set(true);
    this.error.set(null);
    this.todoService.getAll()
      .pipe(finalize(() => this.loading.set(false)))
      .subscribe({
        next: todos => this.todos.set(todos),
        error: err => this.error.set(err.message)
      });
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const title = this.form.controls.title.value;
    const editId = this.editingId();
    this.loading.set(true);

    const request$ = editId
      ? this.todoService.update(editId, { title })
      : this.todoService.create({ title });

    request$.pipe(finalize(() => this.loading.set(false))).subscribe({
      next: () => {
        this.form.reset();
        this.editingId.set(null);
        this.load();
      },
      error: err => this.error.set(err.message)
    });
  }

  edit(todo: Todo): void {
    this.editingId.set(todo.id);
    this.form.controls.title.setValue(todo.title);
  }

  cancelEdit(): void {
    this.editingId.set(null);
    this.form.reset();
  }

  toggle(todo: Todo): void {
    this.todoService.toggle(todo.id).subscribe({ next: () => this.load(), error: err => this.error.set(err.message) });
  }

  remove(todo: Todo): void {
    this.todoService.delete(todo.id).subscribe({ next: () => this.load(), error: err => this.error.set(err.message) });
  }
}
