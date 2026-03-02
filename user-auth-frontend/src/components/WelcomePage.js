import React from "react";
import { useNavigate } from "react-router-dom";
import { getCurrentUser, logout } from "../services/authService";
import "../App.css"; // global CSS

export default function WelcomePage() {
  const navigate = useNavigate();
  const user = getCurrentUser();

  if (!user) {
    navigate("/"); // redirect if not logged in
    return null;
  }

  const handleLogout = () => {
    logout();
    navigate("/");
  };

  return (
    <div className="page-container">
      <h1 className="welcome-header">
        Welcome, {user.firstName} {user.lastName}!
      </h1>
      <p className="user-email">Email: {user.email}</p>
      <button onClick={handleLogout} className="button button-logout">
        Logout
      </button>
    </div>
  );
}