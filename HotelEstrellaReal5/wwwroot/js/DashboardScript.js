
const feedbackCtx = document.getElementById('feedbackChart').getContext('2d');
const feedbackChart = new Chart(feedbackCtx, {
    type: 'bar',
    data: {
        labels: ['Bueno', 'Regular', 'Malo'],
        datasets: [{
            label: 'Nivel de Satisfacción',
            data: [50, 30, 20], // Ejemplo de datos
            backgroundColor: ['#4CAF50', '#FFC107', '#F44336'],
        }]
    },
    options: {
        scales: {
            y: {
                beginAtZero: true
            }
        }
    }
});

// Gráfica de Ventas Netas
const salesCtx = document.getElementById('salesChart').getContext('2d');
const salesChart = new Chart(salesCtx, {
    type: 'line',
    data: {
        labels: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio'],
        datasets: [{
            label: 'Ventas Netas',
            data: [12000, 15000, 13000, 17000, 20000, 19000], // Ejemplo de datos
            borderColor: '#2196F3',
            fill: false,
        }]
    },
    options: {
        scales: {
            y: {
                beginAtZero: true
            }
        }
    }
});

// Gráfica de Ocupación Hotelera
const occupancyCtx = document.getElementById('occupancyChart').getContext('2d');
const occupancyChart = new Chart(occupancyCtx, {
    type: 'doughnut',
    data: {
        labels: ['Habitaciones Ocupadas', 'Habitaciones Disponibles'],
        datasets: [{
            label: 'Ocupación Hotelera',
            data: [70, 30], // Ejemplo de datos
            backgroundColor: ['#8BC34A', '#FFEB3B'],
        }]
    },
    options: {
        responsive: true,
    }
});
// Generar Informe Personalizado
document.getElementById('reportForm').addEventListener('submit', function (event) {
    event.preventDefault();

    const startDate = document.getElementById('startDate').value;
    const endDate = document.getElementById('endDate').value;

    // Aquí puedes realizar la lógica para generar el informe basado en las fechas
    const reportData = `
        <h3>Informe Personalizado</h3>
        <p>Rango de Fechas: ${startDate} a ${endDate}</p>
        <p>Ejemplo de datos: Total de huéspedes: 100</p>
        <p>Ejemplo de datos: Ingresos totales: $50,000</p>
    `;

    document.getElementById('reportOutput').innerHTML = reportData;
});