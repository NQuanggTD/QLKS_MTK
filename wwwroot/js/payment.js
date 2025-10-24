// Payment management
let allBookings = [];
let allCustomers = [];
let allRooms = [];
let currentBookingId = null;

document.addEventListener('DOMContentLoaded', function() {
    loadAllData();
});

async function loadAllData() {
    try {
        const [bookingsRes, customersRes, roomsRes] = await Promise.all([
            fetch('/api/bookings'),
            fetch('/api/customers'),
            fetch('/api/rooms')
        ]);

        allBookings = await bookingsRes.json();
        allCustomers = await customersRes.json();
        allRooms = await roomsRes.json();

        displayPayments();
    } catch (error) {
        console.error('Error loading data:', error);
        showNotification('Lỗi khi tải dữ liệu!', 'error');
    }
}

function displayPayments() {
    const tbody = document.getElementById('paymentsTable');
    const statusFilter = document.getElementById('paymentStatusFilter').value;
    const searchTerm = document.getElementById('searchInput').value.toLowerCase();

    let filteredBookings = allBookings.filter(booking => {
        // Filter by payment status
        if (statusFilter && booking.PaymentStatus.toString() !== statusFilter) {
            return false;
        }

        // Filter by search term
        if (searchTerm) {
            const customer = allCustomers.find(c => c.Id === booking.CustomerId);
            const room = allRooms.find(r => r.Id === booking.RoomId);
            const customerName = customer ? customer.FullName.toLowerCase() : '';
            const roomNumber = room ? room.RoomNumber.toLowerCase() : '';
            
            if (!customerName.includes(searchTerm) && !roomNumber.includes(searchTerm)) {
                return false;
            }
        }

        return true;
    });

    if (filteredBookings.length === 0) {
        tbody.innerHTML = '<tr><td colspan="9" class="text-center">Không có booking nào</td></tr>';
        return;
    }

    tbody.innerHTML = filteredBookings.map(booking => {
        const customer = allCustomers.find(c => c.Id === booking.CustomerId) || {};
        const room = allRooms.find(r => r.Id === booking.RoomId) || {};
        const remaining = booking.TotalAmount - booking.Deposit;
        const paymentStatusText = getPaymentStatusText(booking.PaymentStatus);
        const paymentStatusClass = getPaymentStatusClass(booking.PaymentStatus);
        
        return `
            <tr>
                <td><code>${booking.Id.substring(0, 8)}</code></td>
                <td>${customer.FullName || 'N/A'}</td>
                <td>${room.RoomNumber || 'N/A'}</td>
                <td>${formatDate(booking.CheckOutDate)}</td>
                <td>${formatCurrency(booking.TotalAmount)}</td>
                <td>${formatCurrency(booking.Deposit)}</td>
                <td><strong>${formatCurrency(remaining)}</strong></td>
                <td><span class="status-badge status-${paymentStatusClass}">${paymentStatusText}</span></td>
                <td>
                    <button class="btn btn-sm btn-info" onclick="openPaymentDetail('${booking.Id}')" title="Xem chi tiết">
                        <i class="fas fa-eye"></i>
                    </button>
                </td>
            </tr>
        `;
    }).join('');
}

function filterPayments() {
    displayPayments();
}

function getPaymentStatusText(status) {
    switch(status) {
        case 0: return 'Chưa thanh toán';
        case 1: return 'Thanh toán một phần';
        case 2: return 'Đã thanh toán';
        default: return 'Không xác định';
    }
}

function getPaymentStatusClass(status) {
    switch(status) {
        case 0: return 'pending';
        case 1: return 'confirmed';
        case 2: return 'completed';
        default: return 'cancelled';
    }
}

async function openPaymentDetail(bookingId) {
    currentBookingId = bookingId;
    
    try {
        // Fetch payment summary
        const response = await fetch(`/api/payment/summary/${bookingId}`);
        if (!response.ok) {
            throw new Error('Không thể tải thông tin thanh toán');
        }

        const summary = await response.json();
        const booking = allBookings.find(b => b.Id === bookingId);
        const customer = allCustomers.find(c => c.Id === booking.CustomerId);
        const room = allRooms.find(r => r.Id === booking.RoomId);

        // Fill modal data
        document.getElementById('modalBookingId').textContent = bookingId.substring(0, 8);
        document.getElementById('modalCustomerName').textContent = customer?.FullName || 'N/A';
        document.getElementById('modalRoomNumber').textContent = room?.RoomNumber || 'N/A';
        document.getElementById('modalCheckInDate').textContent = formatDate(booking.CheckInDate);
        document.getElementById('modalCheckOutDate').textContent = formatDate(booking.CheckOutDate);
        document.getElementById('modalNumberOfNights').textContent = summary.NumberOfNights || calculateNights(booking.CheckInDate, booking.CheckOutDate);

        // Build charges table
        const chargesTable = document.getElementById('modalChargesTable');
        let chargesHtml = '';

        // Room charge
        chargesHtml += `
            <tr>
                <td>Tiền phòng</td>
                <td>${summary.NumberOfNights} đêm</td>
                <td>${formatCurrency(summary.PricePerNight)}</td>
                <td>${formatCurrency(summary.RoomCharge)}</td>
            </tr>
        `;

        // Service charges (if any)
        if (summary.ServiceCharges && summary.ServiceCharges.length > 0) {
            summary.ServiceCharges.forEach(service => {
                chargesHtml += `
                    <tr>
                        <td>${service.ServiceName || 'Dịch vụ'}</td>
                        <td>${service.Quantity || 1}</td>
                        <td>${formatCurrency(service.UnitPrice || 0)}</td>
                        <td>${formatCurrency(service.TotalPrice || 0)}</td>
                    </tr>
                `;
            });
        }

        // Discount row
        if (summary.DiscountAmount > 0) {
            chargesHtml += `
                <tr style="color: #27ae60;">
                    <td>Giảm giá${summary.DiscountReason ? ` (${summary.DiscountReason})` : ''}</td>
                    <td>-</td>
                    <td>-</td>
                    <td>-${formatCurrency(summary.DiscountAmount)}</td>
                </tr>
            `;
        }

        chargesTable.innerHTML = chargesHtml;

        // Fill totals
        document.getElementById('modalTotalAmount').textContent = formatCurrency(summary.TotalAmount);
        document.getElementById('modalDepositAmount').textContent = formatCurrency(summary.Deposit);
        document.getElementById('modalRemainingAmount').textContent = formatCurrency(summary.AmountDue);

        // Set payment method
        document.getElementById('modalPaymentMethod').value = booking.PaymentMethod || 0;

        // Show/hide confirm button based on payment status
        const confirmBtn = document.getElementById('confirmPayBtn');
        if (booking.PaymentStatus === 2) {
            confirmBtn.style.display = 'none';
        } else {
            confirmBtn.style.display = 'inline-block';
        }

        // Show modal
        document.getElementById('paymentModal').style.display = 'flex';

    } catch (error) {
        console.error('Error loading payment detail:', error);
        showNotification('Lỗi khi tải chi tiết thanh toán!', 'error');
    }
}

function closePaymentModal() {
    document.getElementById('paymentModal').style.display = 'none';
    currentBookingId = null;
}

async function confirmPayment() {
    if (!currentBookingId) return;

    const paymentMethod = parseInt(document.getElementById('modalPaymentMethod').value);
    
    if (!confirm('Xác nhận đã nhận thanh toán đầy đủ?')) {
        return;
    }

    try {
        const booking = allBookings.find(b => b.Id === currentBookingId);
        
        // Update booking with payment info
        const updatedBooking = {
            ...booking,
            PaymentStatus: 2, // Paid
            PaymentMethod: paymentMethod
        };

        const response = await fetch(`/api/bookings/${currentBookingId}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(updatedBooking)
        });

        if (!response.ok) {
            throw new Error('Không thể cập nhật trạng thái thanh toán');
        }

        showNotification('Đã cập nhật trạng thái thanh toán thành công!', 'success');
        closePaymentModal();
        
        // Reload data
        await loadAllData();

    } catch (error) {
        console.error('Error confirming payment:', error);
        showNotification('Lỗi khi xác nhận thanh toán!', 'error');
    }
}

function calculateNights(checkIn, checkOut) {
    const start = new Date(checkIn);
    const end = new Date(checkOut);
    const diff = end - start;
    return Math.ceil(diff / (1000 * 60 * 60 * 24));
}

function formatDate(dateString) {
    if (!dateString) return 'N/A';
    const date = new Date(dateString);
    return date.toLocaleDateString('vi-VN', {
        year: 'numeric',
        month: '2-digit',
        day: '2-digit',
        hour: '2-digit',
        minute: '2-digit'
    });
}

function formatCurrency(amount) {
    if (amount === null || amount === undefined) return '0₫';
    return new Intl.NumberFormat('vi-VN', {
        style: 'currency',
        currency: 'VND'
    }).format(amount);
}

function showNotification(message, type = 'info') {
    alert(message);
}

// Close modal when clicking outside
window.onclick = function(event) {
    const modal = document.getElementById('paymentModal');
    if (event.target === modal) {
        closePaymentModal();
    }
}
