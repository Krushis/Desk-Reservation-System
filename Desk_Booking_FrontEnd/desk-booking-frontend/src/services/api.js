import axios from 'axios';

const API_BASE_URL = 'http://localhost:5229/api';

const apiClient = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

export const deskApi = {
  getDesks: async (startDate, endDate, userId) => {
    const params = { currentUserId: userId };
    if (startDate) params.startDate = startDate;
    if (endDate) params.endDate = endDate;

    const response = await apiClient.get('/desks', { params });
    return response.data;
  },

  getUserProfile: async (userId) => {
    const response = await apiClient.get(`/users/${userId}/profile`);
    return response.data;
  },

  reserveDesk: async (request) => {
    const response = await apiClient.post('/reservations', request);
    return response.data;
  },

  cancelReservation: async (reservationId, userId) => {
    await apiClient.delete(`/reservations/${reservationId}`, {
      params: { userId }
    });
  },

  cancelReservationForDay: async (reservationId, userId, date) => {
    await apiClient.delete(`/reservations/${reservationId}/day`, {
      params: { userId, date }
    });
  },
};
