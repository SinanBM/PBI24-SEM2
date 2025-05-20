import React, { createContext, useContext, useState, useEffect } from "react";

const AuthContext = createContext();

export function AuthProvider({ children }) {
  const [user, setUser] = useState(() => {
    const stored = localStorage.getItem("user");
    return stored ? JSON.parse(stored) : null;
  });

  const [loading, setLoading] = useState(true);

  useEffect(() => {
    setLoading(false);
  }, []);

  const login = (userData) => {
    const expiryTime = Date.now() + 15 * 60 * 1000; // 15 minutes from now

    const userWithExpiry = {
      ...userData,
      tokenExpiry: expiryTime,
    };

    localStorage.setItem("user", JSON.stringify(userWithExpiry));
    setUser(userWithExpiry);
  };

  const logout = () => {
    localStorage.removeItem("user");
    setUser(null);
  };

  const refreshToken = async () => {
    if (!user?.refreshToken) {
      logout();
      return null;
    }

    try {
      const response = await fetch("http://localhost:5077/api/auth/refresh", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ refreshToken: user.refreshToken }),
      });

      if (!response.ok) {
        logout();
        return null;
      }

      const newTokens = await response.json();
      const newExpiry = Date.now() + 15 * 60 * 1000; // reset expiry for 15 min

      const updatedUser = {
        ...newTokens,
        tokenExpiry: newExpiry,
      };

      localStorage.setItem("user", JSON.stringify(updatedUser));
      setUser(updatedUser);

      return updatedUser.token;
    } catch (err) {
      console.error("Token refresh error:", err);
      logout();
      return null;
    }
  };

  return (
    <AuthContext.Provider value={{ user, login, logout, refreshToken, loading, setUser }}>
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  return useContext(AuthContext);
}
