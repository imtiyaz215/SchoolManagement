import Constants from 'expo-constants';
import axios from 'axios';
import * as SecureStore from 'expo-secure-store';

const baseURL = (Constants.expoConfig?.extra as { apiUrl?: string })?.apiUrl
  ?? 'http://localhost:5000/api/v1';

export const api = axios.create({ baseURL });

api.interceptors.request.use(async (config) => {
  const token = await SecureStore.getItemAsync('access_token');
  if (token) config.headers.Authorization = `Bearer ${token}`;
  return config;
});
