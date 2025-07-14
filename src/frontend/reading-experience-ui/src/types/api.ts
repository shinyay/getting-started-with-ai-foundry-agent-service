// API Types
export interface User {
  id: string;
  email: string;
  nickname?: string;
  bio?: string;
  avatarUrl?: string;
  createdAt: string;
}

export interface AuthResponse {
  token: string;
  user: User;
}

export interface Book {
  id: string;
  title: string;
  subtitle?: string;
  authors: string[];
  description?: string;
  isbn10?: string;
  isbn13?: string;
  publisher?: string;
  publishedDate?: string;
  pageCount?: number;
  language?: string;
  coverImageUrl?: string;
  categories: string[];
  averageRating?: number;
  ratingCount: number;
}

export interface BookSearchCriteria {
  query?: string;
  author?: string;
  isbn?: string;
  categories?: string[];
  page?: number;
  pageSize?: number;
}

export interface UserRegistration {
  email: string;
  password: string;
  nickname?: string;
}

export interface UserLogin {
  email: string;
  password: string;
}

export interface CreateBook {
  title: string;
  subtitle?: string;
  authors: string[];
  description?: string;
  isbn10?: string;
  isbn13?: string;
  publisher?: string;
  publishedDate?: string;
  pageCount?: number;
  language?: string;
  coverImageUrl?: string;
  categories: string[];
}

export interface ImportBook {
  externalId: string;
  source: string;
}