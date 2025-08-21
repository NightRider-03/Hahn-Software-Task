import React, { useState, useEffect } from 'react';
import { useParams, useNavigate, Link } from 'react-router-dom';
import { TaskDetail } from '../components/TaskDetail';
import { TaskDetailDto, UpdateTaskRequest } from '../types/task';
import { taskService } from '../services/TaskService';

export const TaskDetailPage: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const [task, setTask] = useState<TaskDetailDto | null>(null);
  const [loading, setLoading] = useState(true);
  const [updating, setUpdating] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchTask = async () => {
      if (!id) {
        navigate('/');
        return;
      }

      try {
        setLoading(true);
        setError(null);
        const data = await taskService.getTaskById(id);
        setTask(data);
      } catch (err) {
        setError('Failed to fetch task details');
        console.error('Error fetching task:', err);
      } finally {
        setLoading(false);
      }
    };

    fetchTask();
  }, [id, navigate]);

  const handleUpdateTask = async (data: UpdateTaskRequest) => {
    if (!id) return;

    try {
      setUpdating(true);
      await taskService.updateTask(id, data);
      
      // Refresh task data
      const updatedTask = await taskService.getTaskById(id);
      setTask(updatedTask);
    } catch (err) {
      console.error('Error updating task:', err);
      alert('Failed to update task. Please try again.');
    } finally {
      setUpdating(false);
    }
  };

  const handleCompleteTask = async () => {
    if (!id) return;

    try {
      setUpdating(true);
      await taskService.completeTask(id);
      
      // Refresh task data
      const updatedTask = await taskService.getTaskById(id);
      setTask(updatedTask);
    } catch (err) {
      console.error('Error completing task:', err);
      alert('Failed to complete task. Please try again.');
    } finally {
      setUpdating(false);
    }
  };

  if (loading) {
    return (
      <div className="text-center py-5">
        <div className="spinner-border text-primary" role="status">
          <span className="visually-hidden">Loading...</span>
        </div>
        <p className="mt-2 text-muted">Loading task details...</p>
      </div>
    );
  }

  if (error || !task) {
    return (
      <div>
        <nav aria-label="breadcrumb" className="mb-4">
          <ol className="breadcrumb">
            <li className="breadcrumb-item">
              <Link to="/">Tasks</Link>
            </li>
            <li className="breadcrumb-item active" aria-current="page">
              Task Details
            </li>
          </ol>
        </nav>

        <div className="alert alert-danger" role="alert">
          <i className="bi bi-exclamation-triangle me-2"></i>
          {error || 'Task not found'}
        </div>

        <Link to="/" className="btn btn-primary">
          <i className="bi bi-arrow-left me-1"></i>
          Back to Tasks
        </Link>
      </div>
    );
  }

  return (
    <div>
      <nav aria-label="breadcrumb" className="mb-4">
        <ol className="breadcrumb">
          <li className="breadcrumb-item">
            <Link to="/">Tasks</Link>
          </li>
          <li className="breadcrumb-item active" aria-current="page">
            {task.title}
          </li>
        </ol>
      </nav>

      <div className="mb-3">
        <Link to="/" className="btn btn-outline-secondary">
          <i className="bi bi-arrow-left me-1"></i>
          Back to Tasks
        </Link>
      </div>

      <TaskDetail
        task={task}
        onUpdate={handleUpdateTask}
        onComplete={handleCompleteTask}
        loading={updating}
      />
    </div>
  );
};