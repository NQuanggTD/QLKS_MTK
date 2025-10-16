// Bookings management
let allBookings = [];
let allCustomers = [];
let allRooms = [];

async function loadBookings() {
    try {
        allBookings = await apiCall('/bookings');
        await loadCustomersForBookings();
        await loadRoomsForBookings();
        displayBookings();
    } catch (error) {
        console.error('Error loading bookings:', error);
        showNotification('Không thể tải danh sách booking', 'error');
    }
}

async function loadCustomersForBookings() {
    try {
        allCustomers = await apiCall('/customers');
    } catch (error) {
        console.error('Error loading customers:', error);
    }
}

async function loadRoomsForBookings() {
    try {
        allRooms = await apiCall('/rooms');
    } catch (error) {
        console.error('Error loading rooms:', error);
    }
}

function displayBookings() {
    const tableBody = document.getElementById('bookingsTable');
    if (!tableBody) return;

    if (allBookings.length === 0) {
        tableBody.innerHTML = '<tr><td colspan="9" class="text-center">Chưa có booking nào</td></tr>';
        return;
    }

    tableBody.innerHTML = allBookings.map(booking => {
        const customer = allCustomers.find(c => c.id === booking.customerId);
        const room = allRooms.find(r => r.id === booking.roomId);

        return `
            <tr>
                <td>${booking.id.substring(0, 8)}</td>
                <td>${customer ? customer.fullName : 'N/A'}</td>
                <td>${room ? room.roomNumber : 'N/A'}</td>
                <td>${formatDate(booking.checkInDate)}</td>
                <td>${formatDate(booking.checkOutDate)}</td>
                <td>${booking.numberOfGuests}</td>
                <td>
                    <span class="status-badge ${getBookingStatusClass(booking.status)}">
                        ${getBookingStatusText(booking.status)}
                    </span>
                </td>
                <td>${formatCurrency(booking.totalAmount)}</td>
                <td>
                    <div style="display: flex; gap: 5px;">
                        ${booking.status === 0 || booking.status === 1 ? `
                            <button class="action-btn action-btn-view" onclick="checkIn('${booking.id}')" title="Check-in">
                                <i class="fas fa-sign-in-alt"></i>
                            </button>
                        ` : ''}
                        ${booking.status === 2 ? `
                            <button class="action-btn action-btn-edit" onclick="checkOut('${booking.id}')" title="Check-out">
                                <i class="fas fa-sign-out-alt"></i>
                            </button>
                        ` : ''}
                        ${booking.status !== 3 && booking.status !== 4 ? `
                            <button class="action-btn action-btn-delete" onclick="cancelBooking('${booking.id}')" title="Hủy">
                                <i class="fas fa-times"></i>
                            </button>
                        ` : ''}
                    </div>
                </td>
            </tr>
        `;
    }).join('');
}

function filterBookings() {
    const statusFilter = document.getElementById('bookingStatusFilter').value;
    const dateFrom = document.getElementById('dateFrom').value;
    const dateTo = document.getElementById('dateTo').value;

    let filtered = [...allBookings];

    if (statusFilter) {
        filtered = filtered.filter(b => b.status.toString() === statusFilter);
    }

    if (dateFrom) {
        filtered = filtered.filter(b => new Date(b.checkInDate) >= new Date(dateFrom));
    }

    if (dateTo) {
        filtered = filtered.filter(b => new Date(b.checkOutDate) <= new Date(dateTo));
    }

    // Temporarily replace allBookings for display
    const temp = allBookings;
    allBookings = filtered;
    displayBookings();
    allBookings = temp;
}

function openAddBookingModal() {
    document.getElementById('bookingModalTitle').textContent = 'Tạo booking mới';
    document.getElementById('bookingForm').reset();
    document.getElementById('bookingId').value = '';
    
    // Populate customer dropdown
    const customerSelect = document.getElementById('customerId');
    customerSelect.innerHTML = '<option value="">Chọn khách hàng...</option>' +
        allCustomers.map(c => `<option value="${c.id}">${c.fullName}</option>`).join('');

    // Populate room dropdown (only available rooms)
    const roomSelect = document.getElementById('roomId');
    const availableRooms = allRooms.filter(r => r.status === 0);
    roomSelect.innerHTML = '<option value="">Chọn phòng...</option>' +
        availableRooms.map(r => `<option value="${r.id}">${r.roomNumber} - ${getRoomTypeText(r.type)}</option>`).join('');

    document.getElementById('bookingModal').classList.add('show');
}

function closeBookingModal() {
    document.getElementById('bookingModal').classList.remove('show');
}

async function checkIn(bookingId) {
    if (!confirm('Xác nhận check-in cho booking này?')) return;

    try {
        await apiCall(`/bookings/${bookingId}/checkin`, 'POST');
        showNotification('Check-in thành công', 'success');
        loadBookings();
    } catch (error) {
        showNotification(error.message, 'error');
    }
}

async function checkOut(bookingId) {
    if (!confirm('Xác nhận check-out cho booking này?')) return;

    try {
        await apiCall(`/bookings/${bookingId}/checkout`, 'POST');
        showNotification('Check-out thành công', 'success');
        loadBookings();
    } catch (error) {
        showNotification(error.message, 'error');
    }
}

async function cancelBooking(bookingId) {
    if (!confirm('Bạn có chắc chắn muốn hủy booking này?')) return;

    try {
        await apiCall(`/bookings/${bookingId}`, 'DELETE');
        showNotification('Hủy booking thành công', 'success');
        loadBookings();
    } catch (error) {
        showNotification(error.message, 'error');
    }
}

// Form submission
document.addEventListener('DOMContentLoaded', function() {
    if (window.location.pathname.includes('Bookings')) {
        loadBookings();

        const bookingForm = document.getElementById('bookingForm');
        if (bookingForm) {
            bookingForm.addEventListener('submit', async function(e) {
                e.preventDefault();

                const bookingData = {
                    customerId: document.getElementById('customerId').value,
                    roomId: document.getElementById('roomId').value,
                    checkInDate: new Date(document.getElementById('checkInDate').value).toISOString(),
                    checkOutDate: new Date(document.getElementById('checkOutDate').value).toISOString(),
                    numberOfGuests: parseInt(document.getElementById('numberOfGuests').value),
                    paymentMethod: parseInt(document.getElementById('paymentMethod').value),
                    specialRequests: document.getElementById('specialRequests').value,
                    serviceIds: []
                };

                try {
                    await apiCall('/bookings', 'POST', bookingData);
                    showNotification('Tạo booking thành công', 'success');
                    closeBookingModal();
                    loadBookings();
                } catch (error) {
                    showNotification(error.message, 'error');
                }
            });
        }

        // Close modal when clicking outside
        document.getElementById('bookingModal')?.addEventListener('click', function(e) {
            if (e.target === this) {
                closeBookingModal();
            }
        });
    }
});
