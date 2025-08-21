import React from 'react';
import { useForm } from 'react-hook-form';
import { yupResolver } from '@hookform/resolvers/yup';
import * as yup from 'yup';
import { CreateTaskRequest } from '../types/task';

interface TaskFormProps {
  onSubmit: (data: CreateTaskRequest) => void;
  loading?: boolean;
}

// Create a form-specific type that matches the schema
type TaskCreateFormData = {
  title: string;
  description?: string;
  dueDate?: string;
  priority: number;
};

const schema: yup.ObjectSchema<TaskCreateFormData> = yup.object({
  title: yup.string().required('Title is required').max(200, 'Title must not exceed 200 characters'),
  description: yup.string().optional().max(1000, 'Description must not exceed 1000 characters'),
  dueDate: yup.string().optional(),
  priority: yup.number().min(1, 'Priority must be between 1 and 5').max(5, 'Priority must be between 1 and 5').required('Priority is required'),
});

export const TaskForm: React.FC<TaskFormProps> = ({ onSubmit, loading = false }) => {
  const {
    register,
    handleSubmit,
    formState: { errors },
    reset,
  } = useForm<TaskCreateFormData>({
    resolver: yupResolver(schema),
    defaultValues: {
      title: '',
      description: '',
      dueDate: '',
      priority: 3,
    },
  });

  const handleFormSubmit = (data: TaskCreateFormData) => {
    const submitData: CreateTaskRequest = {
      title: data.title,
      description: data.description || '',
      priority: data.priority,
      dueDate: data.dueDate ? new Date(data.dueDate).toISOString() : undefined,
        };
    onSubmit(submitData);
    reset();
  };

  return (
    <div className="card">
      <div className="card-header">
        <h5 className="card-title mb-0">
          <i className="bi bi-plus-circle me-2"></i>
          Create New Task
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
              placeholder="Enter task title"
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
              rows={3}
              {...register('description')}
              placeholder="Enter task description"
            />
            {errors.description && (
              <div className="invalid-feedback">{errors.description.message}</div>
            )}
          </div>

          <div className="row">
            <div className="col-md-6">
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

            <div className="col-md-6">
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
          </div>

          <div className="d-grid">
            <button
              type="submit"
              className="btn btn-primary"
              disabled={loading}
            >
              {loading ? (
                <>
                  <span className="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>
                  Creating...
                </>
              ) : (
                <>
                  <i className="bi bi-plus me-2"></i>
                  Create Task
                </>
              )}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};
export default TaskForm;
export {};