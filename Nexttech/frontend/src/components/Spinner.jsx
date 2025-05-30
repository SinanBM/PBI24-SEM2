import React from 'react';
import './Spinner.css'; // Optional styles

function Spinner() {
  return (
    <div className="spinner-container">
      <div className="spinner" />
      <p>Loading...</p>
    </div>
  );
}

export default Spinner;
