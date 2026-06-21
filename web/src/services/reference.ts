import { api } from '../lib/api';

export const referenceService = {
  sessions: () => api.get('/academic/sessions').then((r) => r.data),
  createSession: (data: unknown) => api.post('/academic/sessions', data).then((r) => r.data),
  classGroups: () => api.get('/academic/class-groups').then((r) => r.data),
  createClassGroup: (data: unknown) => api.post('/academic/class-groups', data).then((r) => r.data),
  classes: () => api.get('/academic/classes').then((r) => r.data),
  createClass: (data: unknown) => api.post('/academic/classes', data).then((r) => r.data),
  sections: (classId: string) => api.get(`/academic/classes/${classId}/sections`).then((r) => r.data),
  createSection: (data: unknown) => api.post('/academic/sections', data).then((r) => r.data),
  religions: () => api.get('/reference/religions').then((r) => r.data),
  castes: () => api.get('/reference/castes').then((r) => r.data),
  houses: () => api.get('/reference/houses').then((r) => r.data),
  scholarTypes: () => api.get('/reference/scholar-types').then((r) => r.data),
  qualifications: () => api.get('/reference/qualifications').then((r) => r.data),
  occupations: () => api.get('/reference/occupations').then((r) => r.data),
  designations: () => api.get('/reference/designations').then((r) => r.data),
  states: () => api.get('/reference/states').then((r) => r.data),
  districts: (stateId: string) => api.get(`/reference/states/${stateId}/districts`).then((r) => r.data),
  cities: (districtId: string) => api.get(`/reference/districts/${districtId}/cities`).then((r) => r.data),
};
