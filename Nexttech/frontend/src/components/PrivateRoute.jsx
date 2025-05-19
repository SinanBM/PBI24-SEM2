import { Navigate } from 'react-router-dom';
import { useAuth } from "../context/AuthContext";

function PrivateRoute({ children }) {
  const { user } = useAuth();  // use the custom hook directly

  return user ? children : <Navigate to="/" />;
}

export default PrivateRoute;
