import React, { useState, useEffect } from 'react';
import { deskApi } from '../services/api';
import './ProfilePage.css';

function ProfilePage({ userId }) {
  const [profile, setProfile] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const loadProfile = async () => {
    try {
      setLoading(true);
      setError(null);
      const data = await deskApi.getUserProfile(userId);
      setProfile(data);
    } catch (err) {
      setError('Failed to load profile. Please try again.');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadProfile();
  }, [userId]);

  if (loading) {
    return <div className="loading">Loading profile...</div>;
  }

  if (error) {
    return <div className="error">{error}</div>;
  }

  if (!profile) {
    return <div className="error">Profile not found</div>;
  }

  return (
    <div className="page-container">
      <div className="profile-header">
        <div className="profile-avatar">
          {profile.firstName[0]}{profile.lastName[0]}
        </div>
        <div className="profile-info">
          <h2>{profile.firstName} {profile.lastName}</h2>
          <p className="profile-email">{profile.email}</p>
        </div>
      </div>

      <div className="reservations-section">
        <h3>Current Reservations</h3>
        {profile.currentReservations.length === 0 ? (
          <p className="empty-state">No current reservations</p>
        ) : (
          <div className="reservations-grid">
            {profile.currentReservations.map((reservation) => (
              <div key={reservation.reservationId} className="reservation-card current">
                <div className="reservation-header">
                  <span className="desk-badge">Desk #{reservation.deskNumber}</span>
                  <span className="status-badge status-active">Active</span>
                </div>
                <div className="reservation-dates">
                  <div className="date-item">
                    <span className="date-label">From:</span>
                    <span className="date-value">
                      {new Date(reservation.startDate).toLocaleDateString()}
                    </span>
                  </div>
                  <div className="date-item">
                    <span className="date-label">To:</span>
                    <span className="date-value">
                      {new Date(reservation.endDate).toLocaleDateString()}
                    </span>
                  </div>
                </div>
              </div>
            ))}
          </div>
        )}
      </div>

      <div className="reservations-section">
        <h3>Past Reservations</h3>
        {profile.pastReservations.length === 0 ? (
          <p className="empty-state">No past reservations</p>
        ) : (
          <div className="reservations-list">
            {profile.pastReservations.map((reservation) => (
              <div key={reservation.reservationId} className="reservation-row">
                <span className="desk-number">Desk #{reservation.deskNumber}</span>
                <span className="reservation-dates-compact">
                  {new Date(reservation.startDate).toLocaleDateString()} - 
                  {new Date(reservation.endDate).toLocaleDateString()}
                </span>
                <span className={`status-badge ${reservation.wasCancelled ? 'status-cancelled' : 'status-completed'}`}>
                  {reservation.wasCancelled ? 'Cancelled' : 'Completed'}
                </span>
              </div>
            ))}
          </div>
        )}
      </div>
    </div>
  );
}

export default ProfilePage;