import React, { useEffect, useState } from "react";
import { Link } from 'react-router-dom';
import { Routes, Route } from 'react-router-dom';
import "./Calc-history.css";
import CalculationDetails from "./Results";

const CalculationHistory = () => {
  const [calculations, setCalculations] = useState([]);
  const [selectedRowId, setSelectedRowId] = useState(null);
  const [sortConfig, setSortConfig] = useState({ column: null, asc: true });
  const [searchTerm, setSearchTerm] = useState("");

  // Fetch calculations data from API
  useEffect(() => {
    fetch("http://localhost:5077/api/calculations")
      .then((res) => res.json())
      .then((data) => setCalculations(data))
      .catch((err) => console.error("Failed to load calculations:", err));
  }, []);

  // Sorting function
  const getSortValue = (item, key) => {
    switch (key) {
      case "calc.id":
        return item.id;
      case "printer":
        return item.printer?.name || "";
      case "material":
        return item.material?.name || "";
      case "createdAt":
        return new Date(item.createdAt).getTime();
      default:
        return item[key] ?? "";
    }
  };

  const sortedCalculations = React.useMemo(() => {
    if (!sortConfig.column) return calculations;

    const sorted = [...calculations].sort((a, b) => {
      let valA = getSortValue(a, sortConfig.column);
      let valB = getSortValue(b, sortConfig.column);
      if (typeof valA === "string") valA = valA.toLowerCase();
      if (typeof valB === "string") valB = valB.toLowerCase();

      if (valA < valB) return sortConfig.asc ? -1 : 1;
      if (valA > valB) return sortConfig.asc ? 1 : -1;
      return 0;
    });

    return sorted;
  }, [calculations, sortConfig]);

  // Search filter
  const normalize = (str) => str.toLowerCase().trim();

  const filteredCalculations = React.useMemo(() => {
    if (!searchTerm) return sortedCalculations;

    const filter = normalize(searchTerm);

    // We'll assign a score to prioritize matches (optional)
    const scored = sortedCalculations
      .map((calc) => {
        const project = normalize(calc.calcName);
        const printer = normalize(calc.printer?.name || "");
        const material = normalize(calc.material?.name || "");

        const combined = `${project} ${printer} ${material}`;

        let score = 0;
        if (project.startsWith(filter)) score += 3;
        if (printer.startsWith(filter)) score += 2;
        if (material.startsWith(filter)) score += 1;
        if (combined.includes(filter)) score += 0.5;

        return { calc, score };
      })
      .filter(({ score }) => score > 0)
      .sort((a, b) => b.score - a.score)
      .map(({ calc }) => calc);

    return scored;
  }, [searchTerm, sortedCalculations]);

  // Select row handler
  const handleSelectRow = (id) => {
    setSelectedRowId(id);
  };

  // Sort header click
  const handleSort = (column) => {
    setSortConfig((prev) => {
      if (prev.column === column) {
        return { column, asc: !prev.asc };
      }
      return { column, asc: true };
    });
  };

  // Delete selected row
  const deleteSelectedRow = async () => {
    if (!selectedRowId) {
      alert("Please select a row first.");
      return;
    }
    if (!window.confirm("Are you sure you want to delete this calculation?"))
      return;

    try {
      const response = await fetch(
        `http://localhost:5077/api/calculations/${selectedRowId}`,
        {
          method: "DELETE",
        }
      );

      if (response.ok) {
        alert("Calculation deleted successfully.");
        setSelectedRowId(null);
        // Refresh data
        const res = await fetch("http://localhost:5077/api/calculations");
        const data = await res.json();
        setCalculations(data);
      } else {
        alert("Failed to delete calculation.");
      }
    } catch (error) {
      console.error("Error deleting calculation:", error);
    }
  };

  // Upload photo handler
  const uploadPhoto = async (e, id) => {
    const file = e.target.files[0];
    if (!file) return;

    const formData = new FormData();
    formData.append("photo", file);

    try {
      const res = await fetch(
        `http://localhost:5077/api/calculations/upload-photo/${id}`,
        {
          method: "POST",
          body: formData,
        }
      );
      if (!res.ok) throw new Error("Upload failed");
      const data = await res.json();
      // Update local image URL in state
      setCalculations((prev) =>
        prev.map((calc) =>
          calc.id === id ? { ...calc, productImage: data.productImage } : calc
        )
      );
    } catch (err) {
      console.error("Upload error:", err);
    }
  };

  // Delete photo handler
  const deletePhoto = async (id) => {
    if (!window.confirm("Are you sure you want to delete this photo?")) return;

    try {
      const res = await fetch(
        `http://localhost:5077/api/calculations/delete-photo/${id}`,
        {
          method: "DELETE",
        }
      );

      if (res.ok) {
        setCalculations((prev) =>
          prev.map((calc) =>
            calc.id === id ? { ...calc, productImage: null } : calc
          )
        );
      } else {
        console.error("Failed to delete image");
      }
    } catch (err) {
      console.error("Error deleting photo:", err);
    }
  };

  return (
    <div className="calculation-history-container">
      <h1>Calculation History</h1>
      <a href="/">Go Home</a>
      <button onClick={deleteSelectedRow}>Delete Selected</button>
      <br />
      <input
        type="text"
        id="searchInput"
        placeholder="Search by project name, printer, or material..."
        value={searchTerm}
        onChange={(e) => setSearchTerm(e.target.value)}
      />

      <table id="historyTable" border="1">
        <thead>
          <tr>
            <th>#</th>
            <th>Product Image</th>
            <th
              data-sort="calc.id"
              className={
                sortConfig.column === "calc.id"
                  ? sortConfig.asc
                    ? "sorted-asc"
                    : "sorted-desc"
                  : ""
              }
              onClick={() => handleSort("calc.id")}
            >
              Calculation ID
            </th>
            <th
              data-sort="calcName"
              className={
                sortConfig.column === "calcName"
                  ? sortConfig.asc
                    ? "sorted-asc"
                    : "sorted-desc"
                  : ""
              }
              onClick={() => handleSort("calcName")}
            >
              Calc Name
            </th>
            <th
              data-sort="printer"
              className={
                sortConfig.column === "printer"
                  ? sortConfig.asc
                    ? "sorted-asc"
                    : "sorted-desc"
                  : ""
              }
              onClick={() => handleSort("printer")}
            >
              Printer
            </th>
            <th
              data-sort="material"
              className={
                sortConfig.column === "material"
                  ? sortConfig.asc
                    ? "sorted-asc"
                    : "sorted-desc"
                  : ""
              }
              onClick={() => handleSort("material")}
            >
              Material
            </th>
            <th
              data-sort="totalCost"
              className={
                sortConfig.column === "totalCost"
                  ? sortConfig.asc
                    ? "sorted-asc"
                    : "sorted-desc"
                  : ""
              }
              onClick={() => handleSort("totalCost")}
            >
              Total Cost ($)
            </th>
            <th
              data-sort="createdAt"
              className={
                sortConfig.column === "createdAt"
                  ? sortConfig.asc
                    ? "sorted-asc"
                    : "sorted-desc"
                  : ""
              }
              onClick={() => handleSort("createdAt")}
            >
              Created At
            </th>
            <th>Details</th>
          </tr>
        </thead>
        <tbody>
          {filteredCalculations.map((calc, index) => (
            <tr
              key={calc.id}
              className={calc.id === selectedRowId ? "selected" : ""}
              onClick={() => handleSelectRow(calc.id)}
            >
              <td>{index + 1}</td>
              <td>
                <img
                  src={calc.productImage
                  ? `http://localhost:5077${calc.productImage}`
                   : 'assets/placeholder.png' // Or a local image inside /public
                     }
                  alt="Product"
                  className="product-image"
                  id={`img-${calc.id}`}
                />
                <br />
                <input
                  type="file"
                  id={`fileInput-${calc.id}`}
                  accept="image/*"
                  className="hidden-file-input"
                  onChange={(e) => uploadPhoto(e, calc.id)}
                />
                <button
                  onClick={(e) => {
                    e.stopPropagation();
                    document.getElementById(`fileInput-${calc.id}`).click();
                  }}
                  className="pointer-cursor"
                >
                  Upload Photo
                </button>
                <button
                  onClick={(e) => {
                    e.stopPropagation();
                    deletePhoto(calc.id);
                  }}
                  className="pointer-cursor"
                >
                  Delete Photo
                </button>
              </td>
              <td>{calc.id}</td>
              <td>{calc.calcName}</td>
              <td>{calc.printer?.name || "N/A"}</td>
              <td>{calc.material?.name || "N/A"}</td>
              <td>${calc.totalCost.toFixed(2)}</td>
              <td>{new Date(calc.createdAt).toLocaleString()}</td>
              <td>
                <Link to={`/calculationdetails?id=${calc.id}`}>[details]</Link>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default CalculationHistory;
