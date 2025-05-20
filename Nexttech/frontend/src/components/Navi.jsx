import { Link } from 'react-router-dom';

function Navi() {
  return (
    <div className="Navi-Container">
      <h1>Welcome to the Home Page</h1>
      <ul>
        
        <li><Link to="/new-calculation">New Calculation</Link></li>
        <li><Link to="/results">Results</Link></li>
        <li><Link to="/calc-history">Calculation History</Link></li>
        <li><Link to="/settings">Settings</Link></li>
        
      </ul>
    </div>
  );
}

export default Navi;
