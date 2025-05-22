import { useState, useCallback } from "react";
import { useAuth } from "../context/AuthContext";

export function useAuthFetch() {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  const { user, refreshToken, logout, setUser } = useAuth();

  const fetchWithAuth = useCallback(async (url, options = {}) => {
    setLoading(true);
    setError(null);

    try {
      let currentUser = user;

      if (currentUser?.tokenExpiry && Date.now() > currentUser.tokenExpiry - 60 * 1000) {
        const newToken = await refreshToken();
        if (!newToken) {
          throw new Error("Session expired, please log in again.");
        }

        const storedUser = localStorage.getItem("user");
        currentUser = storedUser ? JSON.parse(storedUser) : currentUser;
        setUser(currentUser);
      }

      const fetchOptions = {
        ...options,
        headers: {
          ...(options.headers || {}),
          Authorization: `Bearer ${currentUser?.token}`,
        },
      };

      let res = await fetch(url, fetchOptions);

      if (res.status === 401 && currentUser?.refreshToken && !fetchOptions._retry) {
        const newToken = await refreshToken();
        if (!newToken) {
          throw new Error("Session expired, please log in again.");
        }

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
    } catch (err) {
      if (err.message === "Session expired, please log in again.") {
        logout();
      }
      setError(err.message);
      throw err;
    } finally {
      setLoading(false);
    }
  }, [user, refreshToken, logout, setUser]);

  return { fetchWithAuth, loading, error };
}
