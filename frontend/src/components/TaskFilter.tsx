import React from 'react';
import { TaskStatus } from '../types/task';

interface TaskFilterProps {
  currentStatus?: TaskStatus;
  onStatusChange: (status?: TaskStatus) => void;
}

export const TaskFilter: React.FC<TaskFilterProps> = ({ currentStatus, onStatusChange }) => {
  const statusOptions = [
    { value: undefined, label: 'All Tasks', icon: 'bi-list' },
    { value: TaskStatus.Pending, label: 'Pending', icon: 'bi-clock' },
    { value: TaskStatus.InProgress, label: 'In Progress', icon: 'bi-play-circle' },
    { value: TaskStatus.Completed, label: 'Completed', icon: 'bi-check-circle' },
    { value: TaskStatus.Cancelled, label: 'Cancelled', icon: 'bi-x-circle' },
  ];

  return (
    <div className="mb-4">
      <div className="btn-group" role="group" aria-label="Task status filter">
        {statusOptions.map((option) => (
          <button
            key={option.label}
            type="button"
            className={`btn ${currentStatus === option.value ? 'btn-primary' : 'btn-outline-primary'}`}
            onClick={() => onStatusChange(option.value)}
          >
            <i className={`${option.icon} me-1`}></i>
            {option.label}
          </button>
        ))}
      </div>
    </div>
  );
};
export default TaskFilter;
export {};