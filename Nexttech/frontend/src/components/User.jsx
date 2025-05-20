import React, { useEffect, useState } from "react";
import axios from "axios";
import "./User.css";  // Import the CSS file

export default function Users() {
  const [users, setUsers] = useState([]);
  const [formData, setFormData] = useState({
    userId: 0,
    Username: "",
    Pwd: "",
    E_mail: "",
    Role: "user",
  });

  useEffect(() => {
    fetchUsers();
  }, []);

  function fetchUsers() {
    axios.get("http://localhost:5077/api/users")
      .then(res => setUsers(res.data))
      .catch(console.error);
  }

  function editUser(id) {
    axios.get(`http://localhost:5077/api/users/${id}`)
      .then(res => {
        const user = res.data;
        setFormData({
          userId: user.id,
          Username: user.username,
          Pwd: user.pwd,
          E_mail: user.e_mail,
          Role: user.role,
        });
      })
      .catch(console.error);
  }

  function deleteUser(id) {
    axios.delete(`http://localhost:5077/api/users/${id}`)
      .then(() => fetchUsers())
      .catch(console.error);
  }

  function handleChange(e) {
    const { id, value } = e.target;
    setFormData(prev => ({ ...prev, [id]: value }));
  }

  function handleSubmit(e) {
    e.preventDefault();

    const user = {
      Id: formData.userId,
      Username: formData.Username,
      Pwd: formData.Pwd,
      E_mail: formData.E_mail,
      Role: formData.Role,
    };

    const url = formData.userId 
      ? `http://localhost:5077/api/users/${formData.userId}`
      : "http://localhost:5077/api/users";
    const method = formData.userId ? "put" : "post";

    axios[method](url, user)
      .then(() => {
        fetchUsers();
        setFormData({
          userId: 0,
          Username: "",
          Pwd: "",
          E_mail: "",
          Role: "user",
        });
      })
      .catch(console.error);
  }

  return (
    <div className="container">
      <h1>Users</h1>

      <h3>Add / Edit User</h3>
      <form id="userForm" onSubmit={handleSubmit}>
        <input type="hidden" id="userId" value={formData.userId} />

        <label htmlFor="Username">Username:</label>
        <input
          type="text"
          id="Username"
          value={formData.Username}
          onChange={handleChange}
          required
        /><br />

        <label htmlFor="Pwd">Password:</label>
        <input
          type="password"
          id="Pwd"
          value={formData.Pwd}
          onChange={handleChange}
          required
        /><br />

        <label htmlFor="E_mail">Email:</label>
        <input
          type="email"
          id="E_mail"
          value={formData.E_mail}
          onChange={handleChange}
          required
        /><br />

        <label htmlFor="Role">Role:</label>
        <select
          id="Role"
          value={formData.Role}
          onChange={handleChange}
          required
        >
          <option value="user">User</option>
          <option value="admin">Admin</option>
        </select>

        <button type="submit">Save User</button>
      </form>

      <hr />

      <h3>User List</h3>
      <table id="usersTable">
        <thead>
          <tr>
            <th>#</th>
            <th>Username</th>
            <th>Email</th>
            <th>Role</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {users.map((u, i) => (
            <tr key={u.id}>
              <td>{i + 1}</td>
              <td>{u.username}</td>
              <td>{u.e_mail}</td>
              <td>{u.role}</td>
              <td>
                <button className="actionButton" onClick={() => editUser(u.id)}>Edit</button>
                <button className="actionButton" onClick={() => deleteUser(u.id)}>Delete</button>
              </td>
            </tr>
          ))}
          {users.length === 0 && (
            <tr><td colSpan="5" style={{textAlign: "center"}}>No users found</td></tr>
          )}
        </tbody>
      </table>
    </div>
  );
}
