// Customers management
let allCustomers = [];

async function loadCustomers() {
    try {
        allCustomers = await apiCall('/customers');
        displayCustomers();
    } catch (error) {
        console.error('Error loading customers:', error);
        showNotification('Không thể tải danh sách khách hàng', 'error');
    }
}

function displayCustomers() {
    const tableBody = document.getElementById('customersTable');
    if (!tableBody) return;

    if (allCustomers.length === 0) {
        tableBody.innerHTML = '<tr><td colspan="8" class="text-center">Chưa có khách hàng nào</td></tr>';
        return;
    }

    tableBody.innerHTML = allCustomers.map(customer => `
        <tr>
            <td>${customer.fullName}</td>
            <td>${customer.email}</td>
            <td>${customer.phoneNumber}</td>
            <td>${customer.identityNumber}</td>
            <td>${customer.nationality}</td>
            <td>
                ${customer.isVIP ? '<span class="status-badge status-available">VIP</span>' : ''}
            </td>
            <td>${customer.specialNotes || '-'}</td>
            <td>
                <div style="display: flex; gap: 5px;">
                    <button class="action-btn action-btn-view" onclick="viewCustomerDetails('${customer.id}')" title="Xem chi tiết">
                        <i class="fas fa-eye"></i>
                    </button>
                    <button class="action-btn action-btn-edit" onclick="editCustomer('${customer.id}')" title="Sửa">
                        <i class="fas fa-edit"></i>
                    </button>
                    <button class="action-btn action-btn-delete" onclick="deleteCustomer('${customer.id}')" title="Xóa">
                        <i class="fas fa-trash"></i>
                    </button>
                </div>
            </td>
        </tr>
    `).join('');
}

function searchCustomers() {
    const keyword = document.getElementById('customerSearch').value;
    if (keyword.trim()) {
        apiCall(`/customers/search?keyword=${encodeURIComponent(keyword)}`)
            .then(customers => {
                allCustomers = customers;
                displayCustomers();
            })
            .catch(error => {
                console.error('Error searching customers:', error);
            });
    } else {
        loadCustomers();
    }
}

function openAddCustomerModal() {
    document.getElementById('customerModalTitle').textContent = 'Thêm khách hàng mới';
    document.getElementById('customerForm').reset();
    document.getElementById('customerId').value = '';
    document.getElementById('customerModal').classList.add('show');
}

function editCustomer(customerId) {
    const customer = allCustomers.find(c => c.id === customerId);
    if (!customer) return;

    document.getElementById('customerModalTitle').textContent = 'Chỉnh sửa khách hàng';
    document.getElementById('customerId').value = customer.id;
    document.getElementById('fullName').value = customer.fullName;
    document.getElementById('email').value = customer.email;
    document.getElementById('phoneNumber').value = customer.phoneNumber;
    document.getElementById('identityNumber').value = customer.identityNumber;
    document.getElementById('address').value = customer.address || '';
    document.getElementById('dateOfBirth').value = customer.dateOfBirth.split('T')[0];
    document.getElementById('nationality').value = customer.nationality;
    document.getElementById('specialNotes').value = customer.specialNotes || '';
    document.getElementById('isVIP').checked = customer.isVIP;

    document.getElementById('customerModal').classList.add('show');
}

async function viewCustomerDetails(customerId) {
    const customer = allCustomers.find(c => c.id === customerId);
    if (!customer) return;

    try {
        const bookings = await apiCall(`/customers/${customerId}/bookings`);
        
        const detailsHtml = `
            <div style="padding: 20px;">
                <h3 style="color: var(--primary-navy); margin-bottom: 20px;">Thông tin cá nhân</h3>
                <div style="display: grid; grid-template-columns: repeat(2, 1fr); gap: 15px; margin-bottom: 30px;">
                    <div><strong>Họ tên:</strong> ${customer.fullName}</div>
                    <div><strong>Ngày sinh:</strong> ${formatDate(customer.dateOfBirth)}</div>
                    <div><strong>Email:</strong> ${customer.email}</div>
                    <div><strong>SĐT:</strong> ${customer.phoneNumber}</div>
                    <div><strong>CMND/CCCD:</strong> ${customer.identityNumber}</div>
                    <div><strong>Quốc tịch:</strong> ${customer.nationality}</div>
                    <div style="grid-column: span 2;"><strong>Địa chỉ:</strong> ${customer.address || 'N/A'}</div>
                    <div style="grid-column: span 2;"><strong>Ghi chú:</strong> ${customer.specialNotes || 'Không có'}</div>
                </div>

                <h3 style="color: var(--primary-navy); margin-bottom: 15px;">Lịch sử đặt phòng (${bookings.length} booking)</h3>
                ${bookings.length > 0 ? `
                    <div class="table-responsive">
                        <table class="data-table">
                            <thead>
                                <tr>
                                    <th>Ngày đặt</th>
                                    <th>Check-in</th>
                                    <th>Check-out</th>
                                    <th>Trạng thái</th>
                                    <th>Số tiền</th>
                                </tr>
                            </thead>
                            <tbody>
                                ${bookings.map(b => `
                                    <tr>
                                        <td>${formatDate(b.bookingDate)}</td>
                                        <td>${formatDate(b.checkInDate)}</td>
                                        <td>${formatDate(b.checkOutDate)}</td>
                                        <td><span class="status-badge ${getBookingStatusClass(b.status)}">${getBookingStatusText(b.status)}</span></td>
                                        <td>${formatCurrency(b.totalAmount)}</td>
                                    </tr>
                                `).join('')}
                            </tbody>
                        </table>
                    </div>
                ` : '<p style="text-align: center; color: var(--text-gray);">Chưa có lịch sử đặt phòng</p>'}
            </div>
        `;

        document.getElementById('customerDetailsContent').innerHTML = detailsHtml;
        document.getElementById('customerDetailsModal').classList.add('show');
    } catch (error) {
        showNotification('Không thể tải thông tin chi tiết', 'error');
    }
}

function closeCustomerDetailsModal() {
    document.getElementById('customerDetailsModal').classList.remove('show');
}

async function deleteCustomer(customerId) {
    if (!confirm('Bạn có chắc chắn muốn xóa khách hàng này?')) return;

    try {
        await apiCall(`/customers/${customerId}`, 'DELETE');
        showNotification('Xóa khách hàng thành công', 'success');
        loadCustomers();
    } catch (error) {
        showNotification(error.message, 'error');
    }
}

function closeCustomerModal() {
    document.getElementById('customerModal').classList.remove('show');
}

// Form submission
document.addEventListener('DOMContentLoaded', function() {
    if (window.location.pathname.includes('Customers')) {
        loadCustomers();

        const customerForm = document.getElementById('customerForm');
        if (customerForm) {
            customerForm.addEventListener('submit', async function(e) {
                e.preventDefault();

                const customerId = document.getElementById('customerId').value;
                const customerData = {
                    fullName: document.getElementById('fullName').value,
                    email: document.getElementById('email').value,
                    phoneNumber: document.getElementById('phoneNumber').value,
                    identityNumber: document.getElementById('identityNumber').value,
                    address: document.getElementById('address').value,
                    dateOfBirth: new Date(document.getElementById('dateOfBirth').value).toISOString(),
                    nationality: document.getElementById('nationality').value,
                    specialNotes: document.getElementById('specialNotes').value,
                    isVIP: document.getElementById('isVIP').checked
                };

                try {
                    if (customerId) {
                        await apiCall(`/customers/${customerId}`, 'PUT', customerData);
                        showNotification('Cập nhật khách hàng thành công', 'success');
                    } else {
                        await apiCall('/customers', 'POST', customerData);
                        showNotification('Thêm khách hàng mới thành công', 'success');
                    }
                    
                    closeCustomerModal();
                    loadCustomers();
                } catch (error) {
                    showNotification(error.message, 'error');
                }
            });
        }

        // Close modals when clicking outside
        document.getElementById('customerModal')?.addEventListener('click', function(e) {
            if (e.target === this) closeCustomerModal();
        });

        document.getElementById('customerDetailsModal')?.addEventListener('click', function(e) {
            if (e.target === this) closeCustomerDetailsModal();
        });
    }
});
