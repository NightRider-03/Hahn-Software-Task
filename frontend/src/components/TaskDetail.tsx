import React, { useState } from 'react';
import { useForm } from 'react-hook-form';
import { yupResolver } from '@hookform/resolvers/yup';
import * as yup from 'yup';
import { TaskDetailDto, TaskStatus, UpdateTaskRequest } from '../types/task';
import { formatDateTime } from '../utils/dateUtils';

interface TaskDetailProps {
  task: TaskDetailDto;
  onUpdate: (data: UpdateTaskRequest) => void;
  onComplete: () => void;
  loading?: boolean;
}

type TaskFormData = {
  title: string;
  description?: string;
  dueDate?: string;
  priority: number;
  status: number;
};

const schema: yup.ObjectSchema<TaskFormData> = yup.object({
  title: yup.string().required('Title is required').max(200, 'Title must not exceed 200 characters'),
  description: yup.string().optional().max(1000, 'Description must not exceed 1000 characters'),
  dueDate: yup.string().optional(),
  priority: yup.number().min(1, 'Priority must be between 1 and 5').max(5, 'Priority must be between 1 and 5').required('Priority is required'),
  status: yup.number().required('Status is required'),
});

export const TaskDetail: React.FC<TaskDetailProps> = ({ task, onUpdate, onComplete, loading = false }) => {
  const [isEditing, setIsEditing] = useState(false);

  const {
    register,
    handleSubmit,
    formState: { errors },
    reset,
  } = useForm<TaskFormData>({
    resolver: yupResolver(schema),
    defaultValues: {
      title: task.title,
      description: task.description || '',
      dueDate: task.dueDate ? task.dueDate.substring(0, 16) : '',
      priority: task.priority,
      status: task.status,
    },
  });

  const handleFormSubmit = (data: TaskFormData) => {
    const submitData: UpdateTaskRequest = {
      title: data.title,
      description: data.description || '',
      priority: data.priority,
      status: data.status,
      dueDate: data.dueDate || undefined,
    };
    onUpdate(submitData);
    setIsEditing(false);
  };

  const handleCancel = () => {
    reset();
    setIsEditing(false);
  };

  const getStatusBadge = (status: TaskStatus) => {
    switch (status) {
      case TaskStatus.Pending:
        return <span className="badge bg-warning fs-6">Pending</span>;
      case TaskStatus.InProgress:
        return <span className="badge bg-info fs-6">In Progress</span>;
      case TaskStatus.Completed:
        return <span className="badge bg-success fs-6">Completed</span>;
      case TaskStatus.Cancelled:
        return <span className="badge bg-secondary fs-6">Cancelled</span>;
      default:
        return <span className="badge bg-secondary fs-6">Unknown</span>;
    }
  };

  const getPriorityBadge = (priority: number) => {
    const colors = ['', 'success', 'info', 'warning', 'danger', 'dark'];
    const labels = ['', 'Very Low', 'Low', 'Medium', 'High', 'Critical'];
    
    return (
      <span className={`badge bg-${colors[priority] || 'secondary'} fs-6`}>
        {labels[priority] || 'Unknown'}
      </span>
    );
  };

  if (isEditing) {
    return (
      <div className="card">
        <div className="card-header">
          <h5 className="card-title mb-0">
            <i className="bi bi-pencil me-2"></i>
            Edit Task
          </h5>
        </div>
        <div className="card-body">
          <form onSubmit={handleSubmit(handleFormSubmit)}>
            <div className="mb-3">
              <label htmlFor="title" className="form-label">
                Title <span className="text-danger">*</span>
              </label>
              <input
                type="text"
                className={`form-control ${errors.title ? 'is-invalid' : ''}`}
                id="title"
                {...register('title')}
              />
              {errors.title && (
                <div className="invalid-feedback">{errors.title.message}</div>
              )}
            </div>

            <div className="mb-3">
              <label htmlFor="description" className="form-label">Description</label>
              <textarea
                className={`form-control ${errors.description ? 'is-invalid' : ''}`}
                id="description"
                rows={4}
                {...register('description')}
              />
              {errors.description && (
                <div className="invalid-feedback">{errors.description.message}</div>
              )}
            </div>

            <div className="row">
              <div className="col-md-4">
                <div className="mb-3">
                  <label htmlFor="status" className="form-label">
                    Status <span className="text-danger">*</span>
                  </label>
                  <select
                    className={`form-select ${errors.status ? 'is-invalid' : ''}`}
                    id="status"
                    {...register('status', { valueAsNumber: true })}
                  >
                    <option value={TaskStatus.Pending}>Pending</option>
                    <option value={TaskStatus.InProgress}>In Progress</option>
                    <option value={TaskStatus.Completed}>Completed</option>
                    <option value={TaskStatus.Cancelled}>Cancelled</option>
                  </select>
                  {errors.status && (
                    <div className="invalid-feedback">{errors.status.message}</div>
                  )}
                </div>
              </div>

              <div className="col-md-4">
                <div className="mb-3">
                  <label htmlFor="priority" className="form-label">
                    Priority <span className="text-danger">*</span>
                  </label>
                  <select
                    className={`form-select ${errors.priority ? 'is-invalid' : ''}`}
                    id="priority"
                    {...register('priority', { valueAsNumber: true })}
                  >
                    <option value={1}>1 - Very Low</option>
                    <option value={2}>2 - Low</option>
                    <option value={3}>3 - Medium</option>
                    <option value={4}>4 - High</option>
                    <option value={5}>5 - Critical</option>
                  </select>
                  {errors.priority && (
                    <div className="invalid-feedback">{errors.priority.message}</div>
                  )}
                </div>
              </div>

              <div className="col-md-4">
                <div className="mb-3">
                  <label htmlFor="dueDate" className="form-label">Due Date</label>
                  <input
                    type="datetime-local"
                    className={`form-control ${errors.dueDate ? 'is-invalid' : ''}`}
                    id="dueDate"
                    {...register('dueDate')}
                  />
                  {errors.dueDate && (
                    <div className="invalid-feedback">{errors.dueDate.message}</div>
                  )}
                </div>
              </div>
            </div>

            <div className="d-flex gap-2">
              <button
                type="submit"
                className="btn btn-primary"
                disabled={loading}
              >
                {loading ? (
                  <>
                    <span className="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>
                    Saving...
                  </>
                ) : (
                  <>
                    <i className="bi bi-check me-2"></i>
                    Save Changes
                  </>
                )}
              </button>
              <button
                type="button"
                className="btn btn-secondary"
                onClick={handleCancel}
                disabled={loading}
              >
                Cancel
              </button>
            </div>
          </form>
        </div>
      </div>
    );
  }

  return (
    <div className="card">
      <div className="card-header">
        <div className="d-flex justify-content-between align-items-start">
          <div>
            <h3 className="card-title mb-2">{task.title}</h3>
            <div className="d-flex gap-2 align-items-center">
              {getStatusBadge(task.status)}
              {getPriorityBadge(task.priority)}
              {task.isOverdue && (
                <span className="badge bg-danger fs-6">
                  <i className="bi bi-exclamation-triangle me-1"></i>
                  Overdue
                </span>
              )}
            </div>
          </div>
          <div className="d-flex gap-2">
            <button
              className="btn btn-outline-primary"
              onClick={() => setIsEditing(true)}
            >
              <i className="bi bi-pencil me-1"></i>
              Edit
            </button>
            {task.status !== TaskStatus.Completed && task.status !== TaskStatus.Cancelled && (
              <button
                className="btn btn-success"
                onClick={onComplete}
                disabled={loading}
              >
                <i className="bi bi-check me-1"></i>
                Complete
              </button>
            )}
          </div>
        </div>
      </div>
      
      <div className="card-body">
        {task.description && (
          <div className="mb-4">
            <h6 className="text-muted mb-2">Description</h6>
            <p className="mb-0" style={{ whiteSpace: 'pre-wrap' }}>
              {task.description}
            </p>
          </div>
        )}

        <div className="row">
          {task.dueDate && (
            <div className="col-md-6 mb-3">
              <h6 className="text-muted mb-1">Due Date</h6>
              <p className={`mb-0 ${task.isOverdue ? 'text-danger' : ''}`}>
                <i className="bi bi-calendar me-2"></i>
                {formatDateTime(task.dueDate)}
              </p>
            </div>
          )}

          <div className="col-md-6 mb-3">
            <h6 className="text-muted mb-1">Created</h6>
            <p className="mb-0">
              <i className="bi bi-plus-circle me-2"></i>
              {formatDateTime(task.createdAt)}
            </p>
          </div>

          {task.updatedAt && (
            <div className="col-md-6 mb-3">
              <h6 className="text-muted mb-1">Last Updated</h6>
              <p className="mb-0">
                <i className="bi bi-pencil me-2"></i>
                {formatDateTime(task.updatedAt)}
              </p>
            </div>
          )}
        </div>
      </div>
    </div>
  );
};
export default TaskDetail;

export {};