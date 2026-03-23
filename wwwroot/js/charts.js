// Chart.js helper utilities for Hotel DB Dashboard
// Loaded globally via _Layout.cshtml

window.HotelCharts = {
    defaultColors: [
        '#0d6efd', '#198754', '#ffc107', '#dc3545', 
        '#0dcaf0', '#6c757d', '#fd7e14', '#6610f2'
    ],
    
    createBarChart(canvasId, labels, data, label, color) {
        const ctx = document.getElementById(canvasId);
        if (!ctx) return;
        return new Chart(ctx, {
            type: 'bar',
            data: {
                labels: labels,
                datasets: [{
                    label: label || 'Data',
                    data: data,
                    backgroundColor: (color || '#0d6efd') + 'b3',
                    borderColor: color || '#0d6efd',
                    borderWidth: 2,
                    borderRadius: 4
                }]
            },
            options: {
                responsive: true,
                plugins: { legend: { display: false } },
                scales: { y: { beginAtZero: true } }
            }
        });
    },
    
    createDoughnutChart(canvasId, labels, data) {
        const ctx = document.getElementById(canvasId);
        if (!ctx) return;
        return new Chart(ctx, {
            type: 'doughnut',
            data: {
                labels: labels,
                datasets: [{
                    data: data,
                    backgroundColor: this.defaultColors.slice(0, data.length),
                    hoverOffset: 4
                }]
            },
            options: {
                responsive: true,
                plugins: { legend: { position: 'bottom' } }
            }
        });
    }
};
