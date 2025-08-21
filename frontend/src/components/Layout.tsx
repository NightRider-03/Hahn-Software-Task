import React from 'react';
import { Link, useLocation } from 'react-router-dom';

interface LayoutProps {
  children: React.ReactNode;
}

export const Layout: React.FC<LayoutProps> = ({ children }) => {
  const location = useLocation();

  return (
    <div className="min-vh-100 bg-light">
      <nav className="navbar navbar-expand-lg navbar-dark bg-primary">
        <div className="container">
          <Link className="navbar-brand" to="/">
            <i className="bi bi-check-circle-fill me-2"></i>
            Task Management
          </Link>
          
          <div className="navbar-nav ms-auto">
            <Link 
              className={`nav-link ${location.pathname === '/' ? 'active' : ''}`} 
              to="/"
            >
              <i className="bi bi-list-task me-1"></i>
              Tasks
            </Link>
          </div>
        </div>
      </nav>

      <main className="container py-4">
        {children}
      </main>
    </div>
  );
};