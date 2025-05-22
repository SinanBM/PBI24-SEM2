import React, { useEffect, useState } from "react";
import axios from "axios";
import "./Printer.css";

const initialPrinter = {
  id: 0,
  name: "",
  purchase_cost: "",
  machine_lifetime: "",
  cost_Of_Capital: "",
  infrastructure_Cost: "",
  maintenance: "",
  machine_Build_Area: "",
  machine_Build_Height: "",
  machine_Build_Volume: "",
  machine_Build_Rate: "",
  machine_Uptime: "",
  packing_policy: "",
  packing_fraction: "",
  recycling_fraction: "",
  additional_operating_cost: "",
  consumable_cost_per_build: "",
  first_time_build_preparation: "",
  subsequent_build_preparation: "",
  time_per_build_setup: "",
  time_per_build_removal: "",
  time_per_machine_warm_up: "",
  time_per_machine_cool_down: "",
  support_removal_time_labor_constant: "",
  hours_per_day: "",
  days_per_week: "",
  fte_per_machine_supervised: "",
  fte_for_build_exchange: "",
  fte_for_support_removal: "",
  fte_salary_engineer: "",
  fte_salary_operator: "",
  fte_salary_technician: "",
};

export default function Printers() {
  const [printers, setPrinters] = useState([]);
  const [printer, setPrinter] = useState(initialPrinter);

  useEffect(() => {
    fetchPrinters();
  }, []);

  const fetchPrinters = () => {
    axios.get("http://localhost:5077/api/printers").then((res) => {
      setPrinters(res.data);
    });
  };

  const handleChange = (e) => {
    const { id, value } = e.target;
    setPrinter((prev) => ({
      ...prev,
      [id.toLowerCase()]: value,
    }));
  };

  const handleSubmit = (e) => {
    e.preventDefault();

    const dataToSend = {
      ...printer,
      id: parseInt(printer.id || 0),
    };

    axios
      .post("http://localhost:5077/api/printers", dataToSend)
      .then(() => {
        fetchPrinters();
        setPrinter(initialPrinter);
      });
  };

  const handleEdit = (id) => {
    axios.get(`http://localhost:5077/api/printers/${id}`).then((res) => {
      setPrinter(res.data);
    });
  };

  const handleDelete = (id) => {
    axios.delete(`http://localhost:5077/api/printers/${id}`).then(() => {
      fetchPrinters();
    });
  };

  return (
    <div>
      <h1>Printers</h1>

      <h3>Add / Edit Printer</h3>
      <form onSubmit={handleSubmit}>
        <input type="hidden" id="id" value={printer.id || ""} />

        {Object.keys(initialPrinter)
          .filter((key) => key !== "id")
          .map((key) => (
            <div key={key}>
              <label>{key.replace(/_/g, " ").replace(/\b\w/g, l => l.toUpperCase())}:</label>
              <input
                type="number"
                id={key}
                value={printer[key]}
                step="any"
                onChange={handleChange}
                required
              />
              <br />
            </div>
          ))}

        <button type="submit">Save Printer</button>
      </form>

      <hr />

      <h3>Printer List</h3>
      <table id="printersTable" border="1">
        <thead>
          <tr>
            <th>#</th>
            <th>Name</th>
            <th>Purchase Cost</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {printers.map((p, i) => (
            <tr key={p.id} onClick={() => handleEdit(p.id)}>
              <td>{i + 1}</td>
              <td>{p.name}</td>
              <td>{p.purchase_cost}</td>
              <td>
                <button type="button" onClick={(e) => { e.stopPropagation(); handleEdit(p.id); }}>Edit</button>
                <button type="button" onClick={(e) => { e.stopPropagation(); handleDelete(p.id); }}>Delete</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
