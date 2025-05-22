export async function authFetch(url, options = {}) {
  const user = JSON.parse(localStorage.getItem("user"));
  let token = user?.token;

  const response = await fetch(url, {
    ...options,
    headers: {
      ...options.headers,
      Authorization: `Bearer ${token}`,
    },
  });

  if (response.status !== 401) return response;

  // Try refreshing token
  const refreshResponse = await fetch("http://localhost:5077/api/auth/refresh", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ refreshToken: user?.refreshToken }),
  });

  if (!refreshResponse.ok) {
    localStorage.removeItem("user");
    window.location.href = "/login";
    return response;
  }

  const newTokens = await refreshResponse.json();
  localStorage.setItem("user", JSON.stringify(newTokens));

  // Retry original request
  return await fetch(url, {
    ...options,
    headers: {
      ...options.headers,
      Authorization: `Bearer ${newTokens.token}`,
    },
  });
}
