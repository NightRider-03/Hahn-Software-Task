import React, { useState } from 'react';
import { TaskFilter } from '../components/TaskFilter';
import { TaskList } from '../components/TaskList';
import { TaskForm } from '../components/TaskForm';
import { useTasks } from '../hooks/useTask';
import { TaskStatus, CreateTaskRequest } from '../types/task';
import { taskService } from '../services/TaskService';

export const TasksPage: React.FC = () => {
  const [currentStatus, setCurrentStatus] = useState<TaskStatus | undefined>();
  const [showForm, setShowForm] = useState(false);
  const [submitting, setSubmitting] = useState(false);
  const { tasks, loading, error, refreshTasks, fetchTasks } = useTasks();

  const handleStatusChange = (status?: TaskStatus) => {
    setCurrentStatus(status);
    fetchTasks(status);
  };

  const handleCreateTask = async (data: CreateTaskRequest) => {
    try {
      setSubmitting(true);
      await taskService.createTask(data);
      setShowForm(false);
      refreshTasks();
    } catch (err) {
      console.error('Error creating task:', err);
      alert('Failed to create task. Please try again.');
    } finally {
      setSubmitting(false);
    }
  };

  const handleCompleteTask = async (id: string) => {
    try {
      await taskService.completeTask(id);
      refreshTasks();
    } catch (err) {
      console.error('Error completing task:', err);
      alert('Failed to complete task. Please try again.');
    }
  };

  return (
    <div>
      <div className="d-flex justify-content-between align-items-center mb-4">
        <h1 className="h2 mb-0">
          <i className="bi bi-list-task me-2"></i>
          Task Management
        </h1>
        <button
          className="btn btn-primary"
          onClick={() => setShowForm(!showForm)}
        >
          <i className={`bi ${showForm ? 'bi-x' : 'bi-plus'} me-1`}></i>
          {showForm ? 'Cancel' : 'New Task'}
        </button>
      </div>

      {showForm && (
        <div className="mb-4">
          <TaskForm onSubmit={handleCreateTask} loading={submitting} />
        </div>
      )}

      <TaskFilter currentStatus={currentStatus} onStatusChange={handleStatusChange} />

      <TaskList
        tasks={tasks}
        loading={loading}
        error={error}
        onComplete={handleCompleteTask}
      />
    </div>
  );
};