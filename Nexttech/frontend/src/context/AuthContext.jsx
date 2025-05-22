import { createContext, useContext, useState, useEffect } from "react";

const AuthContext = createContext();

export function AuthProvider({ children }) {
  const [user, setUser] = useState(() => {
    const stored = localStorage.getItem("user");
    return stored ? JSON.parse(stored) : null;
  });

  const [loading, setLoading] = useState(true);

  // Fetch profile if user exists but profile is missing
  useEffect(() => {
    async function fetchProfile() {
      if (user && !user.profile) {
        try {
          const response = await fetch("http://localhost:5077/api/user/me", {
            headers: { Authorization: `Bearer ${user.token}` },
          });
          if (!response.ok) throw new Error("Failed to fetch profile");
          const profile = await response.json();

          // Update user state with profile info
          const userWithProfile = { ...user, profile };
          setUser(userWithProfile);
          localStorage.setItem("user", JSON.stringify(userWithProfile));
        } catch (error) {
          console.error("Failed to fetch profile:", error);
          // Optionally logout if profile fetch fails
          // logout();
        }
      }
      setLoading(false);
    }
    fetchProfile();
  }, [user]);

  const login = async (userData) => {
    const expiryTime = Date.now() + 15 * 60 * 1000; // 15 minutes from now

    // Save tokens first
    const userWithExpiry = {
      ...userData,
      tokenExpiry: expiryTime,
    };
    setUser(userWithExpiry);
    localStorage.setItem("user", JSON.stringify(userWithExpiry));

    // Then fetch profile and update state
    try {
      const response = await fetch("http://localhost:5077/api/user/me", {
        headers: { Authorization: `Bearer ${userData.token}` },
      });
      if (!response.ok) throw new Error("Failed to fetch profile");
      const profile = await response.json();

      const userWithProfile = {
        ...userWithExpiry,
        profile,
      };

      setUser(userWithProfile);
      localStorage.setItem("user", JSON.stringify(userWithProfile));
    } catch (error) {
      console.error("Failed to fetch profile after login:", error);
      // You may choose to logout here or keep going
    }
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

      // Preserve existing profile when refreshing tokens
      const updatedUser = {
        ...newTokens,
        tokenExpiry: newExpiry,
        profile: user.profile || null,
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
