import React, { useEffect, useState, useRef} from 'react';
import Chart from 'chart.js/auto';
import ChartDataLabels from 'chartjs-plugin-datalabels';
import './Results.css';
Chart.register(ChartDataLabels);

export default function CalculationDetails() {
  const [data, setData] = useState(null);

  useEffect(() => {
    const urlParams = new URLSearchParams(window.location.search);
    const id = urlParams.get("id");

    if (!id) {
      alert("No calculation ID provided.");
      return;
    }

    fetch(`http://localhost:5077/api/calculations/${id}`)
      .then(res => res.json())
      .then(data => {
        setData(data);

        const ctx = document.getElementById('costDoughnutChart').getContext('2d');

        const centerTextPlugin = {
          id: 'centerText',
          beforeDraw(chart) {
            const { width, height, ctx } = chart;
            const total = chart.data.datasets[0].data.reduce((a, b) => a + b, 0);

            ctx.save();
            ctx.font = 'bold 30px Arial';
            ctx.fillStyle = '#666';
            ctx.textAlign = 'center';
            ctx.textBaseline = 'middle';
            ctx.fillText('Total', width / 2, height / 2 - 12);

            ctx.font = 'bold 20px Arial';
            ctx.fillStyle = '#333';
            ctx.fillText(`$${total.toFixed(2)}`, width / 2, height / 2 + 12);

            ctx.restore();
          }
        };

        new Chart(ctx, {
          type: 'doughnut',
          data: {
            labels: ["Material", "Build Prep", "Machine Cost", "Consumables", "Labor", "Post Process"],
            datasets: [{
              data: [
                data.materialCost,
                data.buildPrepCost,
                data.machineCost,
                data.consumablesCost,
                data.laborCost,
                data.postProcessCost
              ],
              backgroundColor: [
                "#4caf50", "#2196f3", "#ff9800", "#9c27b0", "#f44336", "#795548"
              ]
            }]
          },
          options: {
            cutout: '70%',
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
              legend: { position: 'bottom' },
              datalabels: {
                color: '#fff',
                formatter: (value, context) => {
                  const data = context.chart.data.datasets[0].data;
                  const total = data.reduce((a, b) => a + b, 0);
                  const percent = ((value / total) * 100).toFixed(1);
                  return `${percent}%`;
                },
                font: {
                  weight: 'bold',
                  size: 14
                }
              }
            }
          },
          plugins: [ChartDataLabels, centerTextPlugin]
        });

      })
      .catch(err => alert("Failed to load calculation data."));
  }, []);

  if (!data) return <div>Loading...</div>;

  return (
    <>
      <h2>Calculation Details</h2>
      <div className="resultscontainer">

        <div className="inputs">
          <h3>Inputs</h3>
          <p><strong>Calculation Name:</strong> {data.calcName}</p>
          <p><strong>Printer ID:</strong> {data.printerId}</p>
          <p><strong>Material ID:</strong> {data.materialId}</p>
          <p><strong>Parts Produced:</strong> {data.partsProduced}</p>
          <p><strong>Number of Builds:</strong> {data.numberOfBuilds}</p>
          <p><strong>Part Mass (g):</strong> {data.partMass}</p>
          <p><strong>Part Height:</strong> {data.partHeight}</p>
          <p><strong>Part Area:</strong> {data.partArea}</p>
          <p><strong>Support Material:</strong> {data.supportMat}</p>
        </div>

        <div className="outputs">
          <h3>Outputs</h3>
          <p><strong>Material Cost:</strong> ${data.materialCost.toFixed(2)}</p>
          <p><strong>Prep Time:</strong> {data.buildPrepTime || 'N/A'}</p>
          <p><strong>Prep Cost:</strong> ${data.buildPrepCost.toFixed(2)}</p>
          <p><strong>Post Time per Part:</strong> {data.postTimePerPart || 'N/A'}</p>
          <p><strong>Total Post Time:</strong> {data.totalPostTime || 'N/A'}</p>
          <p><strong>Post Cost:</strong> ${data.postProcessCost.toFixed(2)}</p>
          <p><strong>Up Front Cost:</strong> ${data.upFront.toFixed(2)}</p>
          <p><strong>Annual Depreciation:</strong> ${data.annualDepreciation.toFixed(2)}</p>
          <p><strong>Annual Maintenance:</strong> ${data.annualMaintenance.toFixed(2)}</p>
          <p><strong>Annual Machine Cost:</strong> ${data.annualMachineCost.toFixed(2)}</p>
          <p><strong>Hours per Year:</strong> {data.hoursPerYear}</p>
          <p><strong>Machine Cost per Hour:</strong> ${data.machineCostPerHour.toFixed(2)}</p>
          <p><strong>Warmup Total:</strong> {data.warmupTotal || 'N/A'}</p>
          <p><strong>Part Volume:</strong> {data.partVolume || 'N/A'}</p>
          <p><strong>Print Time:</strong> {data.printTime || 'N/A'}</p>
          <p><strong>Cooldown Total:</strong> {data.cooldownTotal || 'N/A'}</p>
          <p><strong>Exchange Time:</strong> {data.exchangeTime || 'N/A'}</p>
          <p><strong>Machine Time:</strong> {data.machineTime || 'N/A'}</p>
          <p><strong>Machine Usage Cost:</strong> ${data.machineCost.toFixed(2)}</p>
          <p><strong>Operational Total Cost:</strong> ${data.operatingTotalCost.toFixed(2)}</p>
          <p><strong>Consumables:</strong> ${data.consumables.toFixed(2)}</p>
          <p><strong>Total Consumables:</strong> ${data.consumablesCost.toFixed(2)}</p>
          <p><strong>Labor Build Cost:</strong> ${data.laborBuildTime.toFixed(2)}</p>
          <p><strong>Labor Exchange Cost:</strong> ${data.laborExchangeTime.toFixed(2)}</p>
          <p><strong>Total Labor Cost:</strong> ${data.laborCost.toFixed(2)}</p>
        </div>

        <div className="canvas">
          <canvas id="costDoughnutChart"></canvas>
        </div>

      </div>
    </>
  );
}
