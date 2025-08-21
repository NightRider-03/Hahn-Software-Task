import { format, isValid, parseISO } from 'date-fns';

export const formatDate = (dateString?: string): string => {
  if (!dateString) return '';
  
  try {
    const date = parseISO(dateString);
    return isValid(date) ? format(date, 'MMM dd, yyyy') : '';
  } catch {
    return '';
  }
};

export const formatDateTime = (dateString?: string): string => {
  if (!dateString) return '';
  
  try {
    const date = parseISO(dateString);
    return isValid(date) ? format(date, 'MMM dd, yyyy HH:mm') : '';
  } catch {
    return '';
  }
};

export const isOverdue = (dueDate?: string): boolean => {
  if (!dueDate) return false;
  
  try {
    const date = parseISO(dueDate);
    return isValid(date) && date < new Date();
  } catch {
    return false;
  }
};