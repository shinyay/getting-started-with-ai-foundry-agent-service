import React from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';
import './Header.css';

const Header: React.FC = () => {
  const { user, isAuthenticated, logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate('/');
  };

  return (
    <header className="header">
      <div className="header-container">
        <Link to="/" className="logo">
          📚 Reading Experience
        </Link>
        
        <nav className="nav">
          <Link to="/books" className="nav-link">
            Discover Books
          </Link>
          {isAuthenticated && (
            <>
              <Link to="/my-library" className="nav-link">
                My Library
              </Link>
              <Link to="/book-clubs" className="nav-link">
                Book Clubs
              </Link>
            </>
          )}
        </nav>

        <div className="user-section">
          {isAuthenticated ? (
            <div className="user-menu">
              <span className="user-greeting">
                Hello, {user?.nickname || user?.email}!
              </span>
              <button 
                onClick={handleLogout}
                className="btn btn-secondary"
              >
                Logout
              </button>
            </div>
          ) : (
            <div className="auth-buttons">
              <Link to="/login" className="btn btn-secondary">
                Login
              </Link>
              <Link to="/register" className="btn btn-primary">
                Sign Up
              </Link>
            </div>
          )}
        </div>
      </div>
    </header>
  );
};

export default Header;