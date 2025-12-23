import { useState, useEffect } from 'react';
import { deskApi } from '../services/api';
import DeskGrid from '../components/desks/DeskGrid';
import './DeskPage.css';

function DesksPage({ userId }) {
  const [desks, setDesks] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  
  const [startDate, setStartDate] = useState('');
  const [endDate, setEndDate] = useState('');

  const loadDesks = async () => {
    try {
      setLoading(true);
      setError(null);
      const data = await deskApi.getDesks(
        startDate || undefined, endDate || undefined, userId
      );
      setDesks(data);
    } catch (err) {
      setError('Failed to load desks. Please try again.');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadDesks();
  }, [startDate, endDate]);


  return (
    <div className="page-container">
      <h2>Available Desks</h2>
      
      <div className="date-filter">
        <div className="date-input-group">
          <label>
            Start Date:
            <input
              type="date"
              value={startDate}
              onChange={(e) => {
                setStartDate(e.target.value);
              }}
            />
          </label>
          <label>
            End Date:
            <input
              type="date"
              value={endDate}
              min={startDate}
              onChange={(e) => {
                setEndDate(e.target.value);
              }}
            />
          </label>
        </div>
      </div>

      {loading && <div className="loading">Loading desks...</div>}
      {error && <div className="error">{error}</div>}
      

      {!loading && !error && (
        <DeskGrid 
          desks={desks} 
          userId={userId}
          onRefresh={loadDesks}
        />
      )}
    </div>
  );
}

export default DesksPage;