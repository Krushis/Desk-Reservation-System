import React, { useState } from 'react';
import { deskApi } from '../../services/api';
import './DeskCard.css';

const DeskStatus = {
  Open: 0,
  Maintenance: 1,
  Reserved: 2
};

function DeskCard({ desk, userId, onActionComplete }) {
  const [showReserveModal, setShowReserveModal] = useState(false);
  const [startDate, setStartDate] = useState('');
  const [endDate, setEndDate] = useState('');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  const getStatusClass = () => {
    switch (desk.status) {
      case DeskStatus.Open:
        return 'desk-card-open';
      case DeskStatus.Reserved:
        return 'desk-card-reserved';
      case DeskStatus.Maintenance:
        return 'desk-card-maintenance';
      default:
        return '';
    }
  };

  const getStatusLabel = () => {
    switch (desk.status) {
      case DeskStatus.Open:
        return 'Available';
      case DeskStatus.Reserved:
        return 'Reserved';
      case DeskStatus.Maintenance:
        return 'Maintenance';
      default:
        return 'Unknown';
    }
  };

  const handleReserve = async () => {
    if (!startDate || !endDate) {
      setError('Please select both start and end dates');
      return;
    }

    setLoading(true);
    setError(null);

    try {
      await deskApi.reserveDesk({
        deskId: desk.id,
        userId,
        startDate: new Date(startDate).toISOString(),
        endDate: new Date(endDate).toISOString(),
      });
      setShowReserveModal(false);
      onActionComplete();
    } catch (err) {
      setError(err.response?.data?.message || 'Failed to reserve desk');
    } finally {
      setLoading(false);
    }
  };

  const handleCancelReservation = async () => {
    if (!desk.reservationId) return;

    if (!window.confirm('Cancel entire reservation?')) return;

    setLoading(true);
    try {
      await deskApi.cancelReservation(desk.reservationId, userId);
      onActionComplete();
    } catch (err) {
      alert('Failed to cancel reservation');
    } finally {
      setLoading(false);
    }
  };

  const handleCancelDay = async () => {
    if (!desk.reservationId) return;

    const dateStr = prompt('Enter date to cancel (YYYY-MM-DD):');
    if (!dateStr) return;

    setLoading(true);
    try {
      await deskApi.cancelReservationForDay(
        desk.reservationId,
        userId,
        new Date(dateStr).toISOString()
      );
      onActionComplete();
    } catch (err) {
      alert('Failed to cancel day');
    } finally {
      setLoading(false);
    }
  };

  return (
    <>
      <div className={`desk-card ${getStatusClass()}`}>
        <div className="desk-number">Desk #{desk.number}</div>
        <div className="desk-status">{getStatusLabel()}</div>

        {desk.status === DeskStatus.Reserved && (
          <div className="desk-info">
            <p className="reserved-by">Reserved by: {desk.reservedBy}</p>
            <p className="reservation-dates">
              {new Date(desk.reservationStart).toLocaleDateString()} - 
              {new Date(desk.reservationEnd).toLocaleDateString()}
            </p>
          </div>
        )}

        {desk.status === DeskStatus.Maintenance && (
          <div className="desk-info">
            <p className="maintenance-msg">{desk.maintenanceMessage}</p>
          </div>
        )}

        <div className="desk-actions">
          {desk.status === DeskStatus.Open && (
            <button
              className="btn btn-primary"
              onClick={() => setShowReserveModal(true)}
            >
              Reserve
            </button>
          )}

          {desk.status === DeskStatus.Reserved && desk.reservedByCurrentUser && (
            <>
              <button
                className="btn btn-danger btn-small"
                onClick={handleCancelReservation}
                disabled={loading}
              >
                Cancel All
              </button>
              <button
                className="btn btn-warning btn-small"
                onClick={handleCancelDay}
                disabled={loading}
              >
                Cancel Day
              </button>
            </>
          )}
        </div>
      </div>

      {showReserveModal && (
        <div className="modal-overlay" onClick={() => setShowReserveModal(false)}>
          <div className="modal" onClick={(e) => e.stopPropagation()}>
            <h3>Reserve Desk #{desk.number}</h3>
            
            {error && <div className="error">{error}</div>}

            <div className="form-group">
              <label>Start Date:</label>
              <input
                type="date"
                value={startDate}
                onChange={(e) => setStartDate(e.target.value)}
                min={new Date().toISOString().split('T')[0]}
              />
            </div>

            <div className="form-group">
              <label>End Date:</label>
              <input
                type="date"
                value={endDate}
                onChange={(e) => setEndDate(e.target.value)}
                min={startDate || new Date().toISOString().split('T')[0]}
              />
            </div>

            <div className="modal-actions">
              <button
                className="btn btn-secondary"
                onClick={() => setShowReserveModal(false)}
              >
                Cancel
              </button>
              <button
                className="btn btn-primary"
                onClick={handleReserve}
                disabled={loading || !startDate || !endDate}
              >
                {loading ? 'Reserving...' : 'Confirm'}
              </button>
            </div>
          </div>
        </div>
      )}
    </>
  );
}

export default DeskCard;