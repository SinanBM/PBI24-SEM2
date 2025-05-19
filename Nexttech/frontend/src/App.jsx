import React, { useEffect, useState } from "react";
import "./App.css";
import LoginForm from "./components/LoginForm";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import PrivateRoute from "./components/PrivateRoute";
import Spinner from "./components/Spinner";
import { useAuth } from "./context/AuthContext";
import { useAuthFetch } from "./hooks/useAuthFetch";

function Dashboard({ protectedData, loading, error, onLogout }) {
  const { user } = useAuth();

  return (
    <div className="App">
      <h1>Welcome, {user?.userName || "User"}!</h1>
      <button onClick={onLogout}>Logout</button>

      {loading ? (
        <p>Loading protected data...</p>
      ) : error ? (
        <p style={{ color: "red" }}>Error: {error}</p>
      ) : (
        <pre>{JSON.stringify(protectedData, null, 2)}</pre>
      )}
    </div>
  );
}

function App() {
  const { user, login, logout, loading: authLoading } = useAuth();
  const [protectedData, setProtectedData] = useState(null);
  const [initializing, setInitializing] = useState(true);
  const { fetchWithAuth, loading, error } = useAuthFetch();

  useEffect(() => {
    if (user) {
      fetchProtectedData().finally(() => setInitializing(false));
    } else {
      setInitializing(false);
    }
  }, [user]);

  const fetchProtectedData = async () => {
    try {
      const data = await fetchWithAuth("http://localhost:5077/api/admin/admin");
      console.log("Protected response:", data.message);
      setProtectedData(data);
    } catch (err) {
      console.error("Failed to fetch protected data:", err);
    }
  };

  const handleLogout = () => {
    logout();
    setProtectedData(null);
  };

  if (initializing || authLoading) {
    return (
      <div className="App">
        <Spinner />
      </div>
    );
  }

return (
  <Router>
    <Routes>
      <Route
        path="/"
        element={
          user ? (
            <PrivateRoute>
              <Dashboard
                protectedData={protectedData}
                loading={loading}
                error={error}
                onLogout={handleLogout}
              />
            </PrivateRoute>
          ) : (
            <div
              style={{
                display: "flex",
                justifyContent: "center",
                alignItems: "center",
                height: "100vh",
                backgroundColor: "#f0f0f0",
              }}
            >
              <LoginForm onLoginSuccess={login} />
            </div>
          )
        }
      />
    </Routes>
  </Router>
);

}

export default App;
