import React from 'react';
import DeskCard from './DeskCard';
import './DeskGrid.css';

const DeskStatus = {
  Open: 0,
  Maintenance: 1,
  Reserved: 2
};

function DeskGrid({ desks, userId, onRefresh }) {
  return (
    <div className="desk-grid-container">
      <div className="desk-grid">
        {desks.map((desk) => (
          <DeskCard
            key={desk.id}
            desk={desk}
            userId={userId}
            onActionComplete={onRefresh}
          />
        ))}
      </div>
    </div>
  );
}

export default DeskGrid;