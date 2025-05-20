import { Link } from 'react-router-dom';

function Settings(){
    return (
        <div className="Settings-Container">
            <h1>Settings Page</h1>
            <ul>
                <li><Link to="/Printer">Printer Settings</Link></li> 
                <li><Link to="/Material">Material Settings</Link></li>
                <li><Link to="/User">User Settings</Link></li>
                <li><Link to="/Menu">Go Home</Link></li>
            </ul>
        </div>
    );
}

export default Settings;




