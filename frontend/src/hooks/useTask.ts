import { useState, useEffect, useCallback } from 'react';
import { TaskDto, TaskStatus } from '../types/task';
import { taskService } from '../services/TaskService';

export const useTasks = (initialStatus?: TaskStatus) => {
  const [tasks, setTasks] = useState<TaskDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const fetchTasks = useCallback(async (status?: TaskStatus) => {
    try {
      setLoading(true);
      setError(null);
      const data = await taskService.getTasks(status);
      setTasks(data);
    } catch (err) {
      setError('Failed to fetch tasks');
      console.error('Error fetching tasks:', err);
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    fetchTasks(initialStatus);
  }, [fetchTasks, initialStatus]);

  const refreshTasks = useCallback(() => {
    fetchTasks(initialStatus);
  }, [fetchTasks, initialStatus]);

  return {
    tasks,
    loading,
    error,
    refreshTasks,
    fetchTasks,
  };
};