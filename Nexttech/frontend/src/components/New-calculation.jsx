import { useState, useEffect } from 'react';
import './New-calculation.css';

const PrintCostCalculator = () => {
  const [printers, setPrinters] = useState([]);
  const [materials, setMaterials] = useState([]);
  const [calcName, setCalcName] = useState('');
  const [printerId, setPrinterId] = useState('');
  const [materialId, setMaterialId] = useState('');
  const [customMatName, setCustomMatName] = useState('');
  const [customMatDensity, setCustomMatDensity] = useState('');
  const [customMatCost, setCustomMatCost] = useState('');
  const [partsProduced, setPartsProduced] = useState('');
  const [numberOfBuilds, setNumberOfBuilds] = useState('');
  const [partMass, setPartMass] = useState('');
  const [partHeight, setPartHeight] = useState('');
  const [partArea, setPartArea] = useState('');
  const [supportMat, setSupportMat] = useState('');
  const [stlFile, setStlFile] = useState(null);
  const [result, setResult] = useState(null);
  const [loading, setLoading] = useState(false);
  const [lastCalculationInput, setLastCalculationInput] = useState(null);
  const [lastCalculationResult, setLastCalculationResult] = useState(null);

  useEffect(() => {
    const loadData = async () => {
      try {
        const printerResponse = await fetch('http://localhost:5077/api/printers');
        const materialResponse = await fetch('http://localhost:5077/api/materials');

        if (!printerResponse.ok || !materialResponse.ok) {
          console.error("Failed to fetch printers or materials");
          return;
        }

        const printersData = await printerResponse.json();
        const materialsData = await materialResponse.json();

        setPrinters(printersData);
        setMaterials(materialsData);
      } catch (error) {
        console.error("Error loading data:", error);
      }
    };

    loadData();
  }, []);

  const safeToFixed = (num, decimals = 2) => {
    return (typeof num === 'number' ? num : 0).toFixed(decimals);
  };

  const handleCalculate = async (e) => {
    e.preventDefault();

    let selectedMaterialId = materialId;
    let tempMaterialCreated = false;

    if (materialId === 'custom') {
      if (!customMatName || !customMatDensity || !customMatCost) {
        alert("Please fill in all custom material fields.");
        return;
      }

      try {
        const tempMaterialRes = await fetch("http://localhost:5077/api/materials/temp", {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify({
            name: customMatName,
            materialCost: parseFloat(customMatCost),
            materialDensity: parseFloat(customMatDensity)
          })
        });

        if (!tempMaterialRes.ok) {
          throw new Error(await tempMaterialRes.text());
        }

        const tempData = await tempMaterialRes.json();
        selectedMaterialId = tempData.id;
        tempMaterialCreated = true;
      } catch (err) {
        alert("Failed to create temporary material: " + err.message);
        return;
      }
    }

    const input = {
      calcName: calcName.trim(),
      printerId: parseInt(printerId),
      materialId: parseInt(selectedMaterialId),
      partsProduced: parseInt(partsProduced),
      numberOfBuilds: parseInt(numberOfBuilds),
      partMass: parseFloat(partMass),
      partHeight: parseFloat(partHeight),
      partArea: parseFloat(partArea),
      supportMat: parseFloat(supportMat)
    };

    if (
      !input.calcName || isNaN(input.printerId) ||
      isNaN(input.partsProduced) || isNaN(input.numberOfBuilds) ||
      isNaN(input.partMass) || isNaN(input.partHeight) ||
      isNaN(input.partArea) || isNaN(input.supportMat)
    ) {
      alert("Please fill in all required fields before calculating.");
      return;
    }

    try {
      const response = await fetch('http://localhost:5077/api/calculations/preview', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(input)
      });

      if (!response.ok) {
        throw new Error(await response.text());
      }

      const resultData = await response.json();
      setLastCalculationInput(input);
      setLastCalculationResult(resultData);
      setResult(resultData);
    } catch (error) {
      setResult({ error: error.message });
    }
  };

  const handleSave = async () => {
    if (!lastCalculationInput || !lastCalculationResult) {
      alert("Please run a calculation first before saving.");
      return;
    }

    const payload = {
      ...lastCalculationInput,
      ...{
        materialCost: lastCalculationResult.materialCost,
        buildPrepCost: lastCalculationResult.buildPrepCost,
        postProcessCost: lastCalculationResult.postProcessCost,
        machineCost: lastCalculationResult.machineCost,
        consumables: lastCalculationResult.consumables,
        labor: lastCalculationResult.labor,
        totalCost: lastCalculationResult.totalCost
      }
    };

    if (!payload.calcName || payload.calcName.trim() === "") {
      alert("Calculation name is required.");
      return;
    }

    try {
      const response = await fetch('http://localhost:5077/api/calculations', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(payload)
      });

      if (!response.ok) throw new Error(await response.text());
      alert("Calculation saved successfully!");
    } catch (error) {
      alert("Save failed: " + error.message);
    }
  };

  const handleReset = () => {
    setCalcName('');
    setPrinterId('');
    setMaterialId('');
    setCustomMatName('');
    setCustomMatDensity('');
    setCustomMatCost('');
    setPartsProduced('');
    setNumberOfBuilds('');
    setPartMass('');
    setPartHeight('');
    setPartArea('');
    setSupportMat('');
    setStlFile(null);
    setResult(null);
    setLastCalculationInput(null);
    setLastCalculationResult(null);
  };

  const handleUploadStl = async (e) => {
    e.preventDefault();

    if (!stlFile || !stlFile.name.toLowerCase().endsWith('.stl')) {
      alert("Please select a valid STL file.");
      return;
    }

    if (materialId === "custom") {
      if (!customMatName || !customMatDensity || !customMatCost) {
        alert("Please fill in all custom material fields before uploading.");
        return;
      }
    } else if (!materialId) {
      alert("Please select a material before uploading an STL file.");
      return;
    }

    try {
      setLoading(true);
      const formData = new FormData();
      formData.append('stlFileInput', stlFile);
      formData.append('materialId', materialId);

      const response = await fetch('http://localhost:5077/api/calculations/upload-stl', {
        method: 'POST',
        body: formData
      });

      if (!response.ok) {
        throw new Error(await response.text());
      }

      const stlData = await response.json();

      if (stlData.height != null && !isNaN(stlData.height)) {
        setPartHeight(safeToFixed(Number(stlData.height)));
      }

      if (stlData.partArea != null && !isNaN(stlData.partArea)) {
        setPartArea(safeToFixed(Number(stlData.partArea)));
      }

      if (stlData.partMass != null && !isNaN(stlData.partMass)) {
        setPartMass(safeToFixed(Number(stlData.partMass), 5));
      } else {
        console.warn('Invalid partMass value:', stlData.partMass);
      }
    } catch (error) {
      console.error('Error uploading STL:', error);
      alert('Failed to upload STL file: ' + error.message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="calculator-container">
    <div style={{ fontFamily: 'Arial', maxWidth: '800px', margin: 'auto' }}>
      <h1>Calculator</h1>
   
        <div className="input-section">

          <label>
            Calculation Name:
            <input type="text" value={calcName} onChange={(e) => setCalcName(e.target.value)} required />
          </label>

          <label>
            Select a printer:
            <select value={printerId} onChange={(e) => setPrinterId(e.target.value)} required>
              <option value="" disabled>Select a printer</option>
              {printers.map((printer) => (
                <option key={printer.id} value={printer.id}>{printer.name}</option>
              ))}
            </select>
          </label>

          <label>
            Select a material:
            <select value={materialId} onChange={(e) => setMaterialId(e.target.value)} required>
              <option value="" disabled>Select a material</option>
              {materials.map((material) => (
                <option key={material.id} value={material.id}>{material.name}</option>
              ))}
              <option value="custom">-- Custom Material --</option>
            </select>
          </label>

          {materialId === 'custom' && (
            <div style={{ paddingLeft: '10px', borderLeft: '2px solid #ccc' }}>
              <label>
                Material Name:
                <input type="text" value={customMatName} onChange={(e) => setCustomMatName(e.target.value)} />
              </label>

              <label>
                Density (g/cm³):
                <input type="number" step="0.01" value={customMatDensity} onChange={(e) => setCustomMatDensity(e.target.value)} />
              </label>

              <label>
                Cost per kg ($):
                <input type="number" step="0.01" value={customMatCost} onChange={(e) => setCustomMatCost(e.target.value)} />
              </label>
            </div>
          )}
                <label>
            Upload STL file:
            <input
              type="file"
              accept=".stl"
              onChange={(e) => setStlFile(e.target.files[0])}
            />
          </label>

          <button
            type="button"
            onClick={handleUploadStl}
            disabled={loading || !stlFile}
            style={{ marginTop: '10px' }}
          >
            {loading ? 'Uploading...' : 'Upload'}
          </button>
          
          <label>
            Parts Produced:
            <input type="number" min="1" step="1" value={partsProduced} onChange={(e) => setPartsProduced(e.target.value)} />
          </label>

                <label>
            Number of Builds:
            <input
              type="number"
              min="1"
              step="1"
              value={numberOfBuilds}
              onChange={(e) => setNumberOfBuilds(e.target.value)}
            />
          </label>

          <label>
            Part Mass (grams):
            <input
              type="number"
              step="0.00001"
              value={partMass}
              onChange={(e) => setPartMass(e.target.value)}
            />
          </label>

          <label>
            Part Height (mm):
            <input
              type="number"
              step="0.01"
              value={partHeight}
              onChange={(e) => setPartHeight(e.target.value)}
            />
          </label>

          <label>
            Part Base Area (cm²):
            <input
              type="number"
              step="0.01"
              value={partArea}
              onChange={(e) => setPartArea(e.target.value)}
            />
          </label>

          <label>
            Support Material (grams):
            <input
              type="number"
              step="0.01"
              value={supportMat}
              onChange={(e) => setSupportMat(e.target.value)}
            />
          </label>
        
        <button onClick={handleCalculate}>Calculate</button>{' '}
        <button onClick={handleSave} disabled={!result}>
          Save Calculation
        </button>{' '}
        <button onClick={handleReset}>Reset</button>
      </div>
      </div>
 
      <div className="result-section">
        <div className="resultsub">
      {result && (
        <div>
          <h1>Calculation Results</h1>
          {result.error ? (
            <p style={{ color: 'red' }}>Error: {result.error}</p>
          ) : (
            <table style={{ width: '100%', borderCollapse: 'collapse' }}>
              <tbody>
                <tr>
                  <td>Material Cost</td>
                  <td>${safeToFixed(result.materialCost)}</td>
                </tr>
                <tr>
                  <td>Build Preparation Cost</td>
                  <td>${safeToFixed(result.buildPrepCost)}</td>
                </tr>
                <tr>
                  <td>Post Processing Cost</td>
                  <td>${safeToFixed(result.postProcessCost)}</td>
                </tr>
                <tr>
                  <td>Machine Cost</td>
                  <td>${safeToFixed(result.machineCost)}</td>
                </tr>
                <tr>
                  <td>Consumables Cost</td>
                  <td>${safeToFixed(result.consumables)}</td>
                </tr>
                <tr>
                  <td>Labor Cost</td>
                  <td>${safeToFixed(result.labor)}</td>
                </tr>
                <tr style={{ fontWeight: 'bold' }}>
                  <td>Total Cost</td>
                  <td>${safeToFixed(result.totalCost)}</td>
                </tr>
                <tr>
                  <td>Cost per Part</td>
                  <td>${safeToFixed(result.costPerPart)}</td>
                </tr>
              </tbody>
            </table>
          )}
        </div>
        
      )}
    </div>
    </div>
    </div>
  
    
  );
};

export default PrintCostCalculator;

