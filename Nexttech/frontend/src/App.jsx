import { useEffect, useState } from "react";
import { useNavigate, Routes, Route } from "react-router-dom"; // fixed import
import "./App.css";
import LoginForm from "./components/LoginForm";
import PrivateRoute from "./components/PrivateRoute";
import Spinner from "./components/Spinner";
import { useAuth } from "./context/AuthContext";
import { useAuthFetch } from "./hooks/useAuthFetch";
import Navbar from "./components/Navbar";
import Settings from "./components/Settings";
import Material from "./components/Material";
import Printers from "./components/Printer";
import Users from "./components/User";
import CostCalculator from "./components/New-calculation";
import CalculationDetails from "./components/Results";
import CalculationHistory from "./components/Calc-history";

export function Dashboard() {
  return null; // for future dashboard, the first thing user sees after a succesful login
}

function App() {
  const { user, login, logout, loading: authLoading } = useAuth();
  const [protectedData, setProtectedData] = useState(null);
  const [initializing, setInitializing] = useState(true);
  const { fetchWithAuth, loading, error } = useAuthFetch();

  const navigate = useNavigate();

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
    navigate("/login");
  };

  if (initializing || authLoading) {
    return (
      <div className="App">
        <Spinner />
      </div>
    );
  }

  return (
    <>
      {user && <Navbar />}
      <div className="main-content" style={{ padding: "1rem" }}>
        <Routes>
          {/* Root route */}
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
                <LoginForm onLoginSuccess={login} />
              )
            }
          />

          {/* Login route: redirect to Dashboard if logged in */}
          <Route
            path="/login"
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
                <LoginForm onLoginSuccess={login} />
              )
            }
          />

          {/* Protected routes */}
          <Route
            path="/settings"
            element={
              user ? (
                <PrivateRoute>
                  <Settings />
                </PrivateRoute>
              ) : (
                <LoginForm onLoginSuccess={login} />
              )
            }
          />
          <Route
            path="/material"
            element={
              user ? (
                <PrivateRoute>
                  <Material />
                </PrivateRoute>
              ) : (
                <LoginForm onLoginSuccess={login} />
              )
            }
          />
          <Route
            path="/printer"
            element={
              user ? (
                <PrivateRoute>
                  <Printers />
                </PrivateRoute>
              ) : (
                <LoginForm onLoginSuccess={login} />
              )
            }
          />
          <Route
            path="/user"
            element={
              user ? (
                <PrivateRoute>
                  <Users />
                </PrivateRoute>
              ) : (
                <LoginForm onLoginSuccess={login} />
              )
            }
          />
          <Route
            path="/new-calculation"
            element={
              user ? (
                <PrivateRoute>
                  <CostCalculator />
                </PrivateRoute>
              ) : (
                <LoginForm onLoginSuccess={login} />
              )
            }
          />
          <Route
            path="/calculationdetails"
            element={
              user ? (
                <PrivateRoute>
                  <CalculationDetails />
                </PrivateRoute>
              ) : (
                <LoginForm onLoginSuccess={login} />
              )
            }
          />
          <Route
            path="/calc-history"
            element={
              user ? (
                <PrivateRoute>
                  <CalculationHistory />
                </PrivateRoute>
              ) : (
                <LoginForm onLoginSuccess={login} />
              )
            }
          />
        </Routes>
      </div>
    </>
  );
}

export default App;
