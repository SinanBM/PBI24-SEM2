import React, { useEffect, useState } from "react";
import axios from "axios";
import "./User.css";

export default function Users() {
  const [users, setUsers] = useState([]);
  const [formData, setFormData] = useState({
    userId: "",
    firstName: "",
    lastName: "",
    email: "",
    password: "", // <-- added
    role: "user",
  });

  const [passwordData, setPasswordData] = useState({
  currentPassword: "",
  newPassword: "",
  confirmNewPassword: "",
  });

  const [passwordMessage, setPasswordMessage] = useState(""); // feedback message

  function handlePasswordChange(e) {
  const { id, value } = e.target;
  setPasswordData(prev => ({ ...prev, [id]: value }));
  }

  async function handleChangePassword(e) {
  e.preventDefault();

  if (passwordData.newPassword !== passwordData.confirmNewPassword) {
    setPasswordMessage("New password and confirmation do not match.");
    return;
  }

  const userString = localStorage.getItem("user");
  if (!userString) {
    setPasswordMessage("User not logged in.");
    return;
  }

  const user = JSON.parse(userString);
  const token = user?.token;
  if (!token) {
    setPasswordMessage("No token found.");
    return;
  }

  try {
    const response = axios.post(
      `http://localhost:5077/api/user/change-password`,
      {
        currentPassword: passwordData.currentPassword,
        newPassword: passwordData.newPassword,
      },
      { headers: { Authorization: `Bearer ${token}` } }
    );

    setPasswordMessage(response.data.message || "Password changed successfully.");
    setPasswordData({ currentPassword: "", newPassword: "", confirmNewPassword: "" }); // reset form
  } catch (error) {
    if (error.response?.data) {
      setPasswordMessage(
        Array.isArray(error.response.data)
          ? error.response.data.join(", ")
          : error.response.data
      );
    } else {
      setPasswordMessage("Error changing password.");
    }
  }
}

  useEffect(() => {
    fetchUsers();
  }, []);


  function fetchUsers() {
    const userString = localStorage.getItem('user');
    if (!userString) {
      console.error('No user found, user not logged in');
      return;
    }

    const user = JSON.parse(userString);
    const token = user?.token;
    if (!token) {
      console.error('No token found inside user data');
      return;
    }

    axios.get('http://localhost:5077/api/user', {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    })
    .then(response => {
      console.log('Users:', response.data);
      setUsers([...response.data]);
      setUsers(response.data); 
      // handle data
    })
    .catch(error => {
      console.error('Error fetching users:', error);
      if (error.response && error.response.status === 401) {
        // handle unauthorized
      }
    });
  }

  function editUser(id) {
    const userString = localStorage.getItem('user');
    if (!userString) {
      console.error('No user found, user not logged in');
      return;
    }
    const user = JSON.parse(userString);
    const token = user?.token;
    if (!token) {
      console.error('No token found inside user data');
      return;
    }

    axios.get(`http://localhost:5077/api/user/${id}`, {
      headers: {
        Authorization: `Bearer ${token}`,
      }
    })
    .then(res => {
      const user = res.data;
      setFormData({
        userId: user.id,
        firstName: user.firstName || "",
        lastName: user.lastName || "",
        email: user.email || "",
        password: "", // clear password for editing
        role: user.role || "user",
      });
    })
    .catch(console.error);
  }


  function deleteUser(id) {
    if (!window.confirm("Are you sure you want to delete this user?")) return;

    const userString = localStorage.getItem('user');
    if (!userString) {
      console.error('User not logged in');
      return;
    }
    const user = JSON.parse(userString);
    const token = user?.token;
    if (!token) {
      console.error('No token found');
      return;
    }

    axios.delete(`http://localhost:5077/api/user/${id}`, {
      headers: {
        Authorization: `Bearer ${token}`,
      }
    })
    .then(() => fetchUsers())
    .catch(console.error);
  }


  function handleChange(e) {
    const { id, value } = e.target;
    setFormData(prev => ({ ...prev, [id]: value }));
  }

  function handleSubmit(event) {
    event.preventDefault();

    const userString = localStorage.getItem('user');
    if (!userString) {
      console.error('User not logged in');
      return;
    }
    const user = JSON.parse(userString);
    const token = user?.token;
    if (!token) {
      console.error('No token found');
      return;
    }

    const updatedUser = {
      firstName: formData.firstName,
      lastName: formData.lastName,
      email: formData.email,
      password: formData.password,
      role: formData.role,
    };

    axios.put(`http://localhost:5077/api/user/${formData.userId}`, updatedUser, {
      headers: {
        Authorization: `Bearer ${token}`,
      }
    })
    .then(res => {
      fetchUsers();
      // Reset formData to initial values (with all keys present)
      setFormData({
        userId: "",
        firstName: "",
        lastName: "",
        email: "",
        password: "",
        role: "user",
          });
})
    .catch(err => {
      console.error(err);
      // handle error, e.g. show message to user
    });
  }

return (
  <div className="container">
    <h1>Users</h1>

    <h3>Add / Edit User</h3>
    <form id="userForm" onSubmit={handleSubmit}>
      <input type="hidden" id="userId" value={formData.userId || ""} />

      <label htmlFor="firstName">First Name:</label>
      <input
        type="text"
        id="firstName"
        value={formData.firstName}
        onChange={handleChange}
        required
      />
      <br />

      <label htmlFor="lastName">Last Name:</label>
      <input
        type="text"
        id="lastName"
        value={formData.lastName}
        onChange={handleChange}
        required
      />
      <br />

      <label htmlFor="email">Email:</label>
      <input
        type="email"
        id="email"
        value={formData.email}
        onChange={handleChange}
        required
      />
      <br />

      {/* Password field only for user creation */}
      {!formData.userId && (
        <>
          <label htmlFor="password">Password:</label>
          <input
            type="password"
            id="password"
            value={formData.password}
            onChange={handleChange}
            required
          />
          <br />
        </>
      )}

      <label htmlFor="role">Role:</label>
      <select id="role" value={formData.role} onChange={handleChange} required>
        <option value="user">User</option>
        <option value="admin">Admin</option>
      </select>

      <button type="submit">Save User</button>
    </form>

    <hr />

    {/* Change Password Section */}
    <div className="changePasswordSection">
      <h3>Change Password</h3>
      <form onSubmit={handleChangePassword}>
        <label htmlFor="currentPassword">Current Password:</label>
        <input
          type="password"
          id="currentPassword"
          value={passwordData.currentPassword}
          onChange={handlePasswordChange}
          required
        />
        <br />

        <label htmlFor="newPassword">New Password:</label>
        <input
          type="password"
          id="newPassword"
          value={passwordData.newPassword}
          onChange={handlePasswordChange}
          required
        />
        <br />

        <label htmlFor="confirmNewPassword">Confirm New Password:</label>
        <input
          type="password"
          id="confirmNewPassword"
          value={passwordData.confirmNewPassword}
          onChange={handlePasswordChange}
          required
        />
        <br />

        <button type="submit">Change Password</button>
      </form>

      {passwordMessage && (
        <p style={{ color: passwordMessage.toLowerCase().includes("success") ? "green" : "red" }}>
          {passwordMessage}
        </p>
      )}
    </div>

    <hr />

    <h3>User List</h3>
    <table id="usersTable">
      <thead>
        <tr>
          <th>#</th>
          <th>First Name</th>
          <th>Last Name</th>
          <th>Email</th>
          <th>Role</th>
          <th>Actions</th>
        </tr>
      </thead>
      <tbody>
        {users.length === 0 && (
          <tr>
            <td colSpan="6" style={{ textAlign: "center" }}>
              No users found
            </td>
          </tr>
        )}
        {users.map((u, i) => (
          <tr key={u.id}>
            <td>{i + 1}</td>
            {/* If firstName or lastName are missing, fallback to email or userName */}
            <td>{u.firstName ?? u.userName ?? "-"}</td>
            <td>{u.lastName ?? "-"}</td>
            <td>{u.email || u.userName}</td>
            <td>{u.roles ? u.roles.join(", ") : "user"}</td>
            <td>
              <button className="actionButton" onClick={() => editUser(u.id)}>
                Edit
              </button>
              <button className="actionButton" onClick={() => deleteUser(u.id)}>
                Delete
              </button>
            </td>
          </tr>
        ))}
      </tbody>
    </table>
  </div>
)};
