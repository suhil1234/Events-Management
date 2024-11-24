function Search(dropDown) {
    var id = $(dropDown).val(); // Get the selected value from the dropdown
    window.location.href = "/admin/Home/Search?id=" + id; // Redirect to the search page with the selected ID
}

function initializeChart(currentParticipants, availableCapacity) {
    var ctx = document.getElementById('traffic-chart').getContext('2d'); // Get the context for the chart
    var trafficChart = new Chart(ctx, {
        type: 'doughnut', // Set the chart type to doughnut
        data: {
            labels: ['Current Participants', 'Available Capacity'], // Labels for the data
            datasets: [{
                data: [currentParticipants, availableCapacity], // Data for the chart
                backgroundColor: [
                    'rgba(255, 0, 0, 1)', // Red for Current Participants
                    'rgba(0, 255, 0, 1)'  // Green for Available Capacity
                ],
                hoverBackgroundColor: [
                    'rgba(255, 100, 100, 1)', // Lighter red for hover
                    'rgba(100, 255, 100, 1)'  // Lighter green for hover
                ]
            }]
        },
        options: {
            responsive: true, // Make the chart responsive
            maintainAspectRatio: true, // Maintain the aspect ratio
            elements: {
                line: {
                    tension: 0.4, // Smooth lines 
                }
            },
            plugins: {
                legend: {
                    display: true, // Show the legend
                },
            },
            devicePixelRatio: window.devicePixelRatio || 1, // Improve rendering quality
        }
    });
}