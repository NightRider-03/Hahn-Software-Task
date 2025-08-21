import React from 'react';
import { TaskDto } from '../types/task';
import TaskCard from './TaskCard';

interface TaskListProps {
  tasks: TaskDto[];
  loading: boolean;
  error: string | null;
  onComplete: (id: string) => void;
}

export const TaskList: React.FC<TaskListProps> = ({ tasks, loading, error, onComplete }) => {
  if (loading) {
    return (
      <div className="text-center py-5">
        <div className="spinner-border text-primary" role="status">
          <span className="visually-hidden">Loading...</span>
        </div>
        <p className="mt-2 text-muted">Loading tasks...</p>
      </div>
    );
  }

  if (error) {
    return (
      <div className="alert alert-danger" role="alert">
        <i className="bi bi-exclamation-triangle me-2"></i>
        {error}
      </div>
    );
  }

  if (tasks.length === 0) {
    return (
      <div className="text-center py-5">
        <i className="bi bi-inbox display-1 text-muted"></i>
        <h4 className="mt-3 text-muted">No tasks found</h4>
        <p className="text-muted">Create your first task to get started!</p>
      </div>
    );
  }

  return (
    <div className="row">
      {tasks.map((task) => (
        <div key={task.id} className="col-md-6 col-lg-4 mb-4">
          <TaskCard task={task} onComplete={onComplete} />
        </div>
      ))}
    </div>
  );
};
export default TaskList;
export {};