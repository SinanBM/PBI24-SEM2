import { useState } from "react";
import { useAuth } from "../context/AuthContext";
import "./LoginForm.css"; 
import { useNavigate } from "react-router-dom";

export default function LoginForm() {
  const { login } = useAuth();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [showPassword, setShowPassword] = useState(false);
  const [error, setError] = useState(null);
  const [loading, setLoading] = useState(false);

  const navigate = useNavigate();

  async function handleSubmit(e) {
    e.preventDefault();
    setLoading(true);
    setError(null);

    try {
      const response = await fetch("http://localhost:5077/api/auth/login", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ email, password }),
      });

      if (!response.ok) {
        const errData = await response.json();
        setError(errData.message || "Login failed");
        setLoading(false);
        return;
      }

      const data = await response.json();

      if (!data.token || !data.refreshToken) {
        setError("Invalid login response");
        setLoading(false);
        return;
      }

      login({
        token: data.token,
        refreshToken: data.refreshToken,
      });

      navigate("/menu");

    } catch {
      setError("Network error, please try again.");
    } finally {
      setLoading(false);
    }
  }

  return (
<div className="form_container">
  <form className="login_form active" onSubmit={handleSubmit}>
    <h1>Login</h1>

    {error && <p style={{ color: "red", textAlign: "center" }}>{error}</p>}

<div className="input_row">
  <input
    type="email"
    placeholder="E-mail"
    value={email}
    onChange={(e) => setEmail(e.target.value)}
    required
  />
  <i className="fa-solid fa-envelope"></i>
</div>

  <div className="input_row">
    <input
      type={showPassword ? "text" : "password"}
      placeholder="Password"
      value={password}
      onChange={(e) => setPassword(e.target.value)}
      required
    />
    <i
      className={`fa-solid ${showPassword ? "fa-lock-open" : "fa-lock"} clickable`}
      onClick={() => setShowPassword(!showPassword)}
      aria-label={showPassword ? "Hide password" : "Show password"}
    ></i>
  </div>

    <button type="submit" className="button" disabled={loading}>
      {loading ? "Logging in..." : "Login"}
    </button>
  </form>
</div>



  );
}
