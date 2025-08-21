export enum TaskStatus {
  Pending = 1,
  InProgress = 2,
  Completed = 3,
  Cancelled = 4
}

export interface TaskDto {
  id: string;
  title: string;
  description: string;
  status: TaskStatus;
  dueDate?: string;
  priority: number;
  createdAt: string;
  updatedAt?: string;
}

export interface TaskDetailDto extends TaskDto {
  isOverdue: boolean;
}

export interface CreateTaskRequest {
  title: string;
  description: string;
  dueDate?: string;
  priority: number;
}

export interface UpdateTaskRequest {
  title: string;
  description: string;
  dueDate?: string;
  priority: number;
  status?: TaskStatus;
}