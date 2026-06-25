export interface Todo {
  id: string;
  title: string;
  isCompleted: boolean;
  createdAtUtc: string;
  updatedAtUtc?: string | null;
}

export interface CreateTodoRequest { title: string; }
export interface UpdateTodoRequest { title: string; }
