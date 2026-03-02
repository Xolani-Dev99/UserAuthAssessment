const API_URL = "http://localhost:5144/api/users"; // backend URL

// ---------------- LOGIN ----------------
export const login = async (email, password) => {
  const res = await fetch(`${API_URL}/login`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ email, password }),
  });

  if (!res.ok) throw new Error("Invalid credentials");

  const data = await res.json(); // { user, token }

  // STORE USER & TOKEN
  localStorage.setItem("token", data.token);
  localStorage.setItem("user", JSON.stringify(data.user));

  return data;
};

// ---------------- REGISTER ----------------
export const register = async (firstName, lastName, email, password) => {
  const res = await fetch(`${API_URL}/register`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ firstName, lastName, email, password }),
  });

  if (!res.ok) {
    const errorText = await res.text();
    throw new Error(errorText || "Registration failed");
  }

  return res.json();
};

// ---------------- LOGOUT ----------------
export const logout = () => {
  localStorage.removeItem("token");
  localStorage.removeItem("user");
};

// ---------------- CURRENT USER ----------------
export const getCurrentUser = () => {
  const user = localStorage.getItem("user");
  return user ? JSON.parse(user) : null;
};

// ---------------- CHECK AUTH ----------------
export const isAuthenticated = () => {
  const token = localStorage.getItem("token");
  if (!token) return false;

  try {
    const payload = JSON.parse(atob(token.split(".")[1]));
    return payload.exp * 1000 > Date.now(); // token not expired
  } catch {
    return false;
  }
};