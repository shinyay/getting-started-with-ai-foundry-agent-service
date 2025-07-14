import axios, { AxiosResponse } from 'axios';
import {
  User,
  AuthResponse,
  Book,
  BookSearchCriteria,
  UserRegistration,
  UserLogin,
  CreateBook,
  ImportBook,
} from '../types/api';

const API_BASE_URL = process.env.REACT_APP_API_URL || 'http://localhost:5087/api';

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Request interceptor to add auth token
api.interceptors.request.use((config) => {
  const token = localStorage.getItem('authToken');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

// Response interceptor to handle errors
api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      localStorage.removeItem('authToken');
      localStorage.removeItem('user');
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);

// Auth API
export const authApi = {
  register: async (userData: UserRegistration): Promise<AuthResponse> => {
    const response: AxiosResponse<AuthResponse> = await api.post('/auth/register', userData);
    return response.data;
  },

  login: async (credentials: UserLogin): Promise<AuthResponse> => {
    const response: AxiosResponse<AuthResponse> = await api.post('/auth/login', credentials);
    return response.data;
  },

  logout: () => {
    localStorage.removeItem('authToken');
    localStorage.removeItem('user');
  },

  getCurrentUser: (): User | null => {
    const userStr = localStorage.getItem('user');
    return userStr ? JSON.parse(userStr) : null;
  },

  isAuthenticated: (): boolean => {
    return !!localStorage.getItem('authToken');
  },
};

// Books API
export const booksApi = {
  search: async (criteria: BookSearchCriteria): Promise<Book[]> => {
    const params = new URLSearchParams();
    if (criteria.query) params.append('Query', criteria.query);
    if (criteria.author) params.append('Author', criteria.author);
    if (criteria.isbn) params.append('ISBN', criteria.isbn);
    if (criteria.categories) {
      criteria.categories.forEach(cat => params.append('Categories', cat));
    }
    if (criteria.page) params.append('Page', criteria.page.toString());
    if (criteria.pageSize) params.append('PageSize', criteria.pageSize.toString());

    const response: AxiosResponse<Book[]> = await api.get(`/books/search?${params}`);
    return response.data;
  },

  getById: async (id: string): Promise<Book> => {
    const response: AxiosResponse<Book> = await api.get(`/books/${id}`);
    return response.data;
  },

  searchExternal: async (query: string): Promise<Book[]> => {
    const response: AxiosResponse<Book[]> = await api.get(`/books/external/search?query=${encodeURIComponent(query)}`);
    return response.data;
  },

  create: async (bookData: CreateBook): Promise<Book> => {
    const response: AxiosResponse<Book> = await api.post('/books', bookData);
    return response.data;
  },

  import: async (importData: ImportBook): Promise<Book> => {
    const response: AxiosResponse<Book> = await api.post('/books/import', importData);
    return response.data;
  },
};

export default api;