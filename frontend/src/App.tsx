import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { Layout } from './components/Layout';
import { TasksPage } from './pages/TaskPage';
import { TaskDetailPage } from './pages/TaskDetailPage';
import './App.css';

function App() {
  return (
    <Router>
      <Layout>
        <Routes>
          <Route path="/" element={<TasksPage />} />
          <Route path="/tasks/:id" element={<TaskDetailPage />} />
        </Routes>
      </Layout>
    </Router>
  );
}

export default App;