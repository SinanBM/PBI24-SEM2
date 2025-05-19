import { useState } from "react";
import { useAuth } from "../context/AuthContext";

export function useAuthFetch() {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  const { user, refreshToken, logout, setUser } = useAuth();

  const fetchWithAuth = async (url, options = {}) => {
    setLoading(true);
    setError(null);

    try {
      let currentUser = user;

      // Proactive token expiry check: refresh if expiring in less than 1 minute
      if (currentUser?.tokenExpiry && Date.now() > currentUser.tokenExpiry - 60 * 1000) {
        const newToken = await refreshToken();
        if (!newToken) {
          throw new Error("Session expired, please log in again.");
        }

        // Update local currentUser with refreshed token info from localStorage
        const storedUser = localStorage.getItem("user");
        currentUser = storedUser ? JSON.parse(storedUser) : currentUser;

        // Update React state too for sync
        setUser(currentUser);
      }

      // Prepare fetch options with Authorization header
      const fetchOptions = {
        ...options,
        headers: {
          ...(options.headers || {}),
          Authorization: `Bearer ${currentUser?.token}`,
        },
      };

      let res = await fetch(url, fetchOptions);

      // If 401 (unauthorized) and we haven't retried yet, try refresh token once
      if (res.status === 401 && currentUser?.refreshToken && !fetchOptions._retry) {
        const newToken = await refreshToken();
        if (!newToken) {
          throw new Error("Session expired, please log in again.");
        }

        // Retry original request with new token
        const retryOptions = {
          ...options,
          _retry: true,
          headers: {
            ...(options.headers || {}),
            Authorization: `Bearer ${newToken}`,
          },
        };

        res = await fetch(url, retryOptions);
      }

      if (!res.ok) throw new Error(`Request failed: ${res.status}`);

      const data = await res.json();
      return data;
    } catch (err) 
    {
      if (err.message === "Session expired, please log in again.") {
        logout();
      }
      setError(err.message);
      throw err;
    } finally {
      setLoading(false);
    }
  };

  return { fetchWithAuth, loading, error };
}


