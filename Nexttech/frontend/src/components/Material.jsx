import React, { useState, useEffect } from "react";
import axios from "axios";
import "./Material.css";

function Material() {
  const [materialId, setMaterialId] = useState(0);
  const [name, setName] = useState("");
  const [materialCost, setMaterialCost] = useState("");
  const [materialDensity, setMaterialDensity] = useState("");
  const [materials, setMaterials] = useState([]);

  useEffect(() => {
    fetchMaterials();
  }, []);

  const fetchMaterials = () => {
    axios.get("http://localhost:5077/api/materials")
      .then(res => setMaterials(res.data))
      .catch(err => console.error(err));
  };

  const editMaterial = (id) => {
    axios.get(`http://localhost:5077/api/materials/${id}`)
      .then(res => {
        const m = res.data;
        setMaterialId(m.id);
        setName(m.name);
        setMaterialCost(m.material_cost);
        setMaterialDensity(m.material_density);
      })
      .catch(err => console.error(err));
  };

  const saveMaterial = (e) => {
    e.preventDefault();

    const material = {
      Id: materialId,
      Name: name,
      Material_cost: parseFloat(materialCost),
      Material_density: parseFloat(materialDensity),
    };

    const url = materialId
      ? `http://localhost:5077/api/materials/${materialId}`
      : "http://localhost:5077/api/materials";

    const method = materialId ? "put" : "post";

    axios[method](url, material)
      .then(() => {
        fetchMaterials();
        resetForm();
      })
      .catch(err => console.error(err));
  };

  const resetForm = () => {
    setMaterialId(0);
    setName("");
    setMaterialCost("");
    setMaterialDensity("");
  };

  const deleteMaterial = (id) => {
    axios.delete(`http://localhost:5077/api/materials/${id}`)
      .then(() => fetchMaterials())
      .catch(err => console.error(err));
  };

  return (
    <div className="material-container">
      <h1>Materials</h1>

      <h3>Add / Edit Material</h3>
      <form onSubmit={saveMaterial} className="material-form">
        <label>Name:</label>
        <input
          type="text"
          value={name}
          onChange={(e) => setName(e.target.value)}
          required
        />

        <label>Material Cost:</label>
        <input
          type="number"
          step="any"
          value={materialCost}
          onChange={(e) => setMaterialCost(e.target.value)}
          required
        />

        <label>Material Density:</label>
        <input
          type="number"
          step="any"
          value={materialDensity}
          onChange={(e) => setMaterialDensity(e.target.value)}
          required
        />

        <button type="submit">Save Material</button>
        <button type="button" onClick={resetForm} className="clear-button">
          Clear
        </button>
      </form>

      <hr />

      <h3>Material List</h3>
      <table className="material-table">
        <thead>
          <tr>
            <th>#</th>
            <th>Name</th>
            <th>Cost</th>
            <th>Density</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {materials.map((m, index) => (
            <tr key={m.id}>
              <td>{index + 1}</td>
              <td>{m.name}</td>
              <td>{m.material_cost}</td>
              <td>{m.material_density}</td>
              <td>
                <button onClick={() => editMaterial(m.id)}>Edit</button>
                <button
                  onClick={() => deleteMaterial(m.id)}
                  className="delete-button"
                >
                  Delete
                </button>
              </td>
            </tr>
          ))}
          {materials.length === 0 && (
            <tr>
              <td colSpan="5" className="no-data">
                No materials found.
              </td>
            </tr>
          )}
        </tbody>
      </table>
    </div>
  );
}

export default Material;
