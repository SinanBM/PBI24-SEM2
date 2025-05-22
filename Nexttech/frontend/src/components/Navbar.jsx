import { Link, NavLink, useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import { useAuthFetch } from '../hooks/useAuthFetch'; // adjust path if needed
import { useEffect, useState } from 'react';
import './Navbar.css';

export default function Navbar() {
  const { user, logout } = useAuth();
  const { fetchWithAuth, loading: fetchLoading, error } = useAuthFetch();
  const [profile, setProfile] = useState(user?.profile || null);
  const navigate = useNavigate();

  useEffect(() => {
    async function fetchProfile() {
      if (!user?.token) return;
      try {
        const data = await fetchWithAuth("http://localhost:5077/api/user/me");
        setProfile(data);
      } catch (err) {
        console.error("Failed to load user profile:", err);
      }
    }
    fetchProfile();
  }, [user?.token, fetchWithAuth]);


  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  if (!user) return <p>Not logged in</p>;

  // Show loading if fetching profile or auth context is loading
  if (fetchLoading) return <p>Loading profile...</p>;

  return (
    <nav className="navbar">
      <ul className="nav-links">
        <li>
          <NavLink to="/" className={({ isActive }) => isActive ? 'active-link' : ''}>
            Home
          </NavLink>
        </li>
        <li>
          <NavLink to="/profile" className={({ isActive }) => isActive ? 'active-link' : ''}>
            Profile
          </NavLink>
        </li>
        <li>
          <NavLink to="/new-calculation" className={({ isActive }) => isActive ? 'active-link' : ''}>
            New Calculation
          </NavLink>
        </li>
        <li>
          <NavLink to="/calc-history" className={({ isActive }) => isActive ? 'active-link' : ''}>
            Calculation History
          </NavLink>
        </li>
        <li className="dropdown">
          <span className="dropbtn">Settings</span>
          <div className="dropdown-content">
            <Link to="/printer">Printer Settings</Link>
            <Link to="/material">Material Settings</Link>
            <Link to="/user">User Settings</Link>
          </div>
        </li>
      </ul>

      {user && (
        <div className="user-section">
          <span>Welcome, {profile?.firstName || "User"}!</span>
          <button onClick={handleLogout} className="logout-button">
            Logout
          </button>
        </div>
      )}
      {error && <p className="error-message">{error}</p>}
    </nav>
  );
}
