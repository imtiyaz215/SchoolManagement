import { api } from '../lib/api';

export interface StudentListItem {
  id: string;
  admissionNumber: string;
  fullName: string;
  gender: string;
  dob: string;
  status: string;
}

export const studentService = {
  list: (params?: { search?: string; page?: number; pageSize?: number }) =>
    api.get<{ items: StudentListItem[]; total: number; page: number; pageSize: number }>('/students', { params }).then((r) => r.data),
  get: (id: string) => api.get(`/students/${id}`).then((r) => r.data),
  create: (data: unknown) => api.post('/students', data).then((r) => r.data),
  updateStatus: (id: string, data: unknown) => api.patch(`/students/${id}/status`, data).then((r) => r.data),
};
