import axios from 'axios';
import { TaskDto, TaskDetailDto, CreateTaskRequest, UpdateTaskRequest, TaskStatus } from '../types/task';

const API_BASE_URL = process.env.REACT_APP_API_URL || 'http://localhost:44314/api';

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

export const taskService = {
  async getTasks(status?: TaskStatus): Promise<TaskDto[]> {
    const params = status !== undefined ? { status } : {};
    const response = await api.get<TaskDto[]>('/tasks', { params });
    return response.data;
  },

  async getTaskById(id: string): Promise<TaskDetailDto> {
    const response = await api.get<TaskDetailDto>(`/tasks/${id}`);
    return response.data;
  },

  async createTask(task: CreateTaskRequest): Promise<string> {
    const response = await api.post<string>('/tasks', task);
    return response.data;
  },

  async updateTask(id: string, task: UpdateTaskRequest): Promise<void> {
    await api.put(`/tasks/${id}`, task);
  },

  async completeTask(id: string): Promise<void> {
    await api.patch(`/tasks/${id}/complete`);
  },
};