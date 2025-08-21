import React from 'react';
import { Link } from 'react-router-dom';
import { TaskDto, TaskStatus } from '../types/task';
import { formatDate, isOverdue } from '../utils/dateUtils';

interface TaskCardProps {
  task: TaskDto;
  onComplete: (id: string) => void;
}

const TaskCard: React.FC<TaskCardProps> = ({ task, onComplete }) => {
  const getStatusBadge = (status: TaskStatus) => {
    switch (status) {
      case TaskStatus.Pending:
        return <span className="badge bg-warning">Pending</span>;
      case TaskStatus.InProgress:
        return <span className="badge bg-info">In Progress</span>;
      case TaskStatus.Completed:
        return <span className="badge bg-success">Completed</span>;
      case TaskStatus.Cancelled:
        return <span className="badge bg-secondary">Cancelled</span>;
      default:
        return <span className="badge bg-secondary">Unknown</span>;
    }
  };

  const getPriorityBadge = (priority: number) => {
    const colors = ['', 'success', 'info', 'warning', 'danger', 'dark'];
    const labels = ['', 'Very Low', 'Low', 'Medium', 'High', 'Critical'];
   
    return (
      <span className={`badge bg-${colors[priority] || 'secondary'}`}>
        {labels[priority] || 'Unknown'}
      </span>
    );
  };

  const taskOverdue = task.dueDate && isOverdue(task.dueDate) && task.status !== TaskStatus.Completed;

  return (
    <div className={`card h-100 ${taskOverdue ? 'border-danger' : ''}`}>
      <div className="card-body d-flex flex-column">
        <div className="d-flex justify-content-between align-items-start mb-2">
          <h5 className="card-title mb-0">{task.title}</h5>
          <div className="d-flex gap-1">
            {getStatusBadge(task.status)}
            {getPriorityBadge(task.priority)}
          </div>
        </div>
       
        {task.description && (
          <p className="card-text text-muted small">
            {task.description.length > 100
              ? `${task.description.substring(0, 100)}...`
              : task.description}
          </p>
        )}
       
        {task.dueDate && (
          <div className="mb-2">
            <small className={`text-${taskOverdue ? 'danger' : 'muted'}`}>
              <i className="bi bi-calendar me-1"></i>
              Due: {formatDate(task.dueDate)}
              {taskOverdue && <span className="ms-1">(Overdue)</span>}
            </small>
          </div>
        )}
       
        <div className="mt-auto">
          <div className="d-flex justify-content-between align-items-center">
            <Link to={`/tasks/${task.id}`} className="btn btn-outline-primary btn-sm">
              <i className="bi bi-eye me-1"></i>
              View Details
            </Link>
           
            {task.status !== TaskStatus.Completed && task.status !== TaskStatus.Cancelled && (
              <button
                className="btn btn-success btn-sm"
                onClick={() => onComplete(task.id)}
              >
                <i className="bi bi-check me-1"></i>
                Complete
              </button>
            )}
          </div>
        </div>
      </div>
    </div>
  );
};

export default TaskCard;
export {};