import axios from 'axios';

const api = axios.create({
  baseURL: 'https://localhost:7056/api',
  withCredentials: false,
});

export default api;
