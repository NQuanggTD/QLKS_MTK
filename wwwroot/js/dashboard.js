// Dashboard functionality
let dashboardData = null;

async function loadDashboardData() {
    try {
        dashboardData = await apiCall('/dashboard');
        updateDashboardUI();
    } catch (error) {
        console.error('Error loading dashboard:', error);
        showNotification('Không thể tải dữ liệu dashboard', 'error');
    }
}

function updateDashboardUI() {
    if (!dashboardData) return;

    // Update stats cards
    document.getElementById('availableRooms').textContent = dashboardData.availableRooms;
    document.getElementById('occupiedRooms').textContent = dashboardData.occupiedRooms;
    document.getElementById('newBookings').textContent = dashboardData.newBookingsToday;
    document.getElementById('revenue').textContent = formatCurrency(dashboardData.estimatedRevenue);

    // Update room status list
    updateRoomStatusList();

    // Update recent bookings table
    updateRecentBookingsTable();

    // Update room occupancy chart
    updateRoomOccupancyChart();
}

function updateRoomStatusList() {
    const statusList = document.getElementById('roomStatusList');
    if (!statusList) return;

    const statuses = [
        { label: 'Phòng trống', value: dashboardData.availableRooms, icon: 'fa-door-open', color: 'var(--success)' },
        { label: 'Đang sử dụng', value: dashboardData.occupiedRooms, icon: 'fa-bed', color: 'var(--danger)' },
        { label: 'Đã đặt', value: dashboardData.reservedRooms, icon: 'fa-calendar-check', color: 'var(--info)' },
        { label: 'Bảo trì', value: dashboardData.maintenanceRooms, icon: 'fa-tools', color: 'var(--warning)' }
    ];

    statusList.innerHTML = statuses.map(status => `
        <div class="status-item">
            <div class="status-item-label">
                <i class="fas ${status.icon}" style="color: ${status.color}"></i>
                <span>${status.label}</span>
            </div>
            <div class="status-item-value">${status.value}</div>
        </div>
    `).join('');
}

function updateRecentBookingsTable() {
    const tableBody = document.getElementById('recentBookingsTable');
    if (!tableBody) return;

    if (!dashboardData.recentBookings || dashboardData.recentBookings.length === 0) {
        tableBody.innerHTML = '<tr><td colspan="6" class="text-center">Chưa có booking nào</td></tr>';
        return;
    }

    tableBody.innerHTML = dashboardData.recentBookings.map(booking => `
        <tr>
            <td>${booking.customerName}</td>
            <td>${booking.roomNumber}</td>
            <td>${formatDate(booking.checkInDate)}</td>
            <td>${formatDate(booking.checkOutDate)}</td>
            <td>
                <span class="status-badge ${getBookingStatusClass(getStatusValue(booking.status))}">
                    ${booking.status}
                </span>
            </td>
            <td>${formatCurrency(booking.amount)}</td>
        </tr>
    `).join('');
}

function getStatusValue(statusText) {
    const statusMap = {
        'Chờ xác nhận': 0,
        'Đã xác nhận': 1,
        'Đã nhận phòng': 2,
        'Đã trả phòng': 3,
        'Đã hủy': 4
    };
    return statusMap[statusText] || 0;
}

function updateRoomOccupancyChart() {
    const chartDiv = document.getElementById('roomOccupancyChart');
    if (!chartDiv || !dashboardData.roomOccupancyByType) return;

    // Simple bar chart visualization
    chartDiv.innerHTML = dashboardData.roomOccupancyByType.map(room => `
        <div style="margin-bottom: 20px;">
            <div style="display: flex; justify-content: space-between; margin-bottom: 8px;">
                <span style="font-weight: 500; color: var(--primary-navy);">${room.roomType}</span>
                <span style="color: var(--accent-gold); font-weight: 600;">${room.occupancyRate.toFixed(1)}%</span>
            </div>
            <div style="background: var(--bg-light); height: 30px; border-radius: 15px; overflow: hidden;">
                <div style="background: linear-gradient(90deg, var(--accent-gold), var(--primary-navy)); 
                            height: 100%; width: ${room.occupancyRate}%; transition: width 0.5s ease;
                            display: flex; align-items: center; justify-content: center; color: white; font-size: 12px;">
                    ${room.occupied}/${room.total}
                </div>
            </div>
        </div>
    `).join('');
}

// Initialize dashboard
document.addEventListener('DOMContentLoaded', function() {
    if (window.location.pathname.includes('Dashboard')) {
        loadDashboardData();
        
        // Refresh dashboard every 30 seconds
        setInterval(loadDashboardData, 30000);
    }
});
