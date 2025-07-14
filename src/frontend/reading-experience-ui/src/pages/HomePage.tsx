import React from 'react';
import { Link } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';
import './HomePage.css';

const HomePage: React.FC = () => {
  const { isAuthenticated, user } = useAuth();

  return (
    <div className="home-page">
      <section className="hero">
        <div className="hero-content">
          <h1 className="hero-title">
            Transform Your Reading Experience
          </h1>
          <p className="hero-subtitle">
            Discover, track, and share your reading journey with our comprehensive platform designed for book lovers.
          </p>
          
          {isAuthenticated ? (
            <div className="welcome-section">
              <h2>Welcome back, {user?.nickname || user?.email}!</h2>
              <div className="action-buttons">
                <Link to="/books" className="btn btn-primary btn-lg">
                  Discover Books
                </Link>
                <Link to="/my-library" className="btn btn-secondary btn-lg">
                  My Library
                </Link>
              </div>
            </div>
          ) : (
            <div className="cta-buttons">
              <Link to="/register" className="btn btn-primary btn-lg">
                Start Your Journey
              </Link>
              <Link to="/login" className="btn btn-secondary btn-lg">
                Sign In
              </Link>
            </div>
          )}
        </div>
      </section>

      <section className="features">
        <div className="features-container">
          <h2 className="features-title">Why Choose Our Platform?</h2>
          
          <div className="features-grid">
            <div className="feature-card">
              <div className="feature-icon">📚</div>
              <h3>Discover Books</h3>
              <p>
                Search through millions of books using our integrated Google Books API. Find your next great read easily.
              </p>
            </div>
            
            <div className="feature-card">
              <div className="feature-icon">📖</div>
              <h3>Track Progress</h3>
              <p>
                Keep track of your reading progress, set goals, and manage your personal library with advanced status tracking.
              </p>
            </div>
            
            <div className="feature-card">
              <div className="feature-icon">⭐</div>
              <h3>Review & Rate</h3>
              <p>
                Share your thoughts with detailed reviews, ratings, and connect with fellow readers in our community.
              </p>
            </div>
            
            <div className="feature-card">
              <div className="feature-icon">👥</div>
              <h3>Join Book Clubs</h3>
              <p>
                Participate in book clubs, join reading events, and engage in meaningful discussions about your favorite books.
              </p>
            </div>
            
            <div className="feature-card">
              <div className="feature-icon">🤖</div>
              <h3>AI Recommendations</h3>
              <p>
                Get personalized book recommendations based on your reading history and preferences using AI technology.
              </p>
            </div>
            
            <div className="feature-card">
              <div className="feature-icon">🌐</div>
              <h3>Community</h3>
              <p>
                Connect with book lovers worldwide, follow your favorite readers, and discover trending books in the community.
              </p>
            </div>
          </div>
        </div>
      </section>

      <section className="stats">
        <div className="stats-container">
          <div className="stat-item">
            <div className="stat-number">1M+</div>
            <div className="stat-label">Books Available</div>
          </div>
          <div className="stat-item">
            <div className="stat-number">50K+</div>
            <div className="stat-label">Active Readers</div>
          </div>
          <div className="stat-item">
            <div className="stat-number">100K+</div>
            <div className="stat-label">Reviews Shared</div>
          </div>
        </div>
      </section>
    </div>
  );
};

export default HomePage;